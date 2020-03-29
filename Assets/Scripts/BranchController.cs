using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BranchController : MonoBehaviour
{
    public Vector2 offset;
    public float direction;

    public int cost;

    public bool root = false;
    // Start is called before the first frame update

    public List<GameObject> subBranches;
    public int hits;

    public BranchType type;

    static int maxHits = 100;

    public int resourcesDeposit = 0;

    public List<Context> gene;

    int resources;
    public int PlayerID;
    Subscription<ResourceChangeEvent> resSub;

    // checkpoint-related variables
    bool isChecked;
    float createdTime;
    Subscription<ShieldEvent> checkpointSub;

    // Visual highlighting
    bool isSelected;

    void Start()
    {
        // if (root)
        // {
        //     // a test context
        //     var cells = "0";
        //     var rules = new Dictionary<char, string>();
        //     rules['0'] = "1[0]0";
        //     rules['1'] = "11";
        //     cells = PrimitiveTree.Grow(cells, rules);
        //     cells = PrimitiveTree.Grow(cells, rules);
        //     cells = PrimitiveTree.Grow(cells, rules);
        //     cells = PrimitiveTree.Grow(cells, rules);
        //     cells = PrimitiveTree.Grow(cells, rules);
        //     var testPrimitiveParseData = PrimitiveTree.makePrimitiveForest(cells);
        //     // var testPrimitiveTree = PrimitiveTree.Tree(Primitive.Move(1.0f), new List<PrimitiveTree>());
        //     var testPrimitiveTree = testPrimitiveParseData.trees[0];
        //     var testContext = Context.reducePrimitiveTree(testPrimitiveTree);
        //     Context.SpawnBranchesWithContext(gameObject, testContext);
        // }
        hits = maxHits;
        if (subBranches.Count != 0)
        {
            type.Mature();
        }
        if (gene == null)
        {
            // initialize default gene
            // Y-shaped
            gene = new List<Context>{
                new Context {
                    offset = new Vector2(0.3f, 0),
                    direction = 0.0f,
                    cost = 100,
                    subContexts = new List<Context>{
                        new Context {
                            offset = new Vector2(0.3f, 0),
                            direction = 25.0f,
                            cost = 100,
                            subContexts = new List<Context>{},
                        },
                        new Context {
                            offset = new Vector2(0.3f, 0),
                            direction = -25.0f,
                            cost = 100,
                            subContexts = new List<Context>{},
                        }
                    }
                },
            };
        }
        resSub = EventBus.Subscribe<ResourceChangeEvent>(ResourceChangeHandler);
        PlayerID = GetPlayerID();
        //StartCoroutine(AutoGrow());

        isChecked = false;
        createdTime = Time.time;
        checkpointSub = EventBus.Subscribe<ShieldEvent>(CheckPointHandler);
    }

    // Update is called once per frame
    void Update()
    {
        Render();
    }
    
    void Render()
    {
        var branchSprite = GetComponent<Transform>().GetChild(0);
        var localScale = branchSprite.GetComponent<Transform>().localScale;
        branchSprite.GetComponent<Transform>().localScale = new Vector2(100.0f * offset.x, localScale.y);
        GetComponent<Transform>().localEulerAngles = new Vector3(0.0f, 0.0f, direction);
        branchSprite.GetComponent<SpriteRenderer>().color = GetColor();
    }

    Color GetColor()
    {
        byte hp = (byte) (((float) hits / maxHits) * 255);
        Color32[] palette = {
            new Color32(0x60, 0x5B, 0x56, hp),
            new Color32(0x83, 0x7A, 0x75, hp),
            new Color32(0xAC, 0xC1, 0x8A, hp),
            new Color32(0xDA, 0xFE, 0xB7, hp),
            new Color32(0xF2, 0xFB, 0xE0, hp),
        };
        if (isSelected)
        {
            //return UnityEngine.Random.ColorHSV(0.0f, 1.0f, 0.5f, 1.0f, 0.8f, 0.8f);
            return palette[4];
        }
        // float maturity = 1.0f;
        if (GetComponent<BranchController>().type.GetBType() == BranchType.BType.Old) {
            // maturity = 0.0f;
            return palette[0];
        }
        float deposit = (float) resourcesDeposit / ResourcesNeeded();
        return palette[1 + (int)Mathf.Floor(deposit * 3.0f)];
        // return new Color(deposit, maturity, hp);
    }

    public int ResourcesNeeded()
    {
        var resourcesNeeded = 0;
        foreach (var genome in gene)
        {
            resourcesNeeded += Context.countCost(genome);
        }
        return resourcesNeeded;
    }

    [ContextMenu("Grow")]
    public void Grow100()
    {
        Grow(100);
    }

    public int Grow(int resources)
    {
        // grow branches according to the current branch's properties
        // return the amount of resources consumed
        subBranches.RemoveAll(obj => obj == null);
        if (type.GetBType() == BranchType.BType.Old && subBranches.Count != 0)
        {
            // a non-leaf branch cannot be grown or revitalized
            return 0;
        } else if (type.GetBType() == BranchType.BType.Old && subBranches.Count == 0)
        {
            // a leaf branch can be revitalized if it's old
            return type.Revitalize(resources);
        } else
        {
            // a growing leaf branch needs enough resources to sprout
            // first, deposit
            var consumed = Math.Min(ResourcesNeeded() - resourcesDeposit, resources);
            resourcesDeposit += consumed;
            if (resourcesDeposit == ResourcesNeeded())
            {
                resourcesDeposit = 0;
                foreach (var genome in gene)
                {
                    subBranches.Add(Context.SpawnBranchesWithContext(gameObject, genome));
                }
                type.Mature();

                EventBus.Publish<GrowProgressEvent>(new GrowProgressEvent(transform,
                    10, 10, false, GetInstanceID()));
            }
            else {
                EventBus.Publish<GrowProgressEvent>(new GrowProgressEvent(transform,
                    ResourcesNeeded() - resourcesDeposit, ResourcesNeeded(), true, GetInstanceID()));
            }
            return consumed;
        }
    }

    [ContextMenu("Damage")]
    public void Damage50()
    {
        Damage(50);
    }
    
    public int Damage(int damage)
    {
        if (root || isChecked)
        {
            // root branch cannot be damaged
            return 0;
        }

        // damage the branch's HP
        hits -= damage;

        // return the amount of resources dropped if the branch is destroyed
        if (hits <= 0)
        {
            return DestroyBranch();
        }

        // otherwise return zero
        return 0;
    }

    int DestroyBranch()
    {
        subBranches.RemoveAll(obj => obj == null);
        int resources = 0;
        foreach (var subBranch in subBranches)
        {
            resources += subBranch.GetComponent<BranchController>().DestroyBranch();
        }

        EventBus.Publish<GrowProgressEvent>(new GrowProgressEvent(transform,
                    10, 10, false, GetInstanceID()));

        Destroy(gameObject);
        return resources + Mathf.FloorToInt(cost * 0.5f);
    }

     IEnumerator AutoGrow() {
        while (true){
            yield return new WaitForSeconds(1f);
            if (resources == 0) continue;
            Debug.Log("Before: " + resources.ToString());
            resources -= Grow(resources);
            Debug.Log("After: " + resources.ToString());
            EventBus.Publish<ResourceChangeEvent>(new ResourceChangeEvent(PlayerID, resources));
        }
    }

    private void OnTriggerEnter(Collider other) {
        PlayerMovement player = other.GetComponent<PlayerMovement>();
        if (player)
        {
            if (root) {
                EventBus.Publish<PlayerProgressEvent>(new PlayerProgressEvent("reach root", GetPlayerID()));
            }
            player.selected_branches.Add(this);
            isSelected = true;
            if (type.GetBType() != BranchType.BType.Old) {
                EventBus.Publish<GrowProgressEvent>(new GrowProgressEvent(transform,
                    ResourcesNeeded()-resourcesDeposit, ResourcesNeeded(), true, GetInstanceID()));
            }
        } 
    }

    private void OnTriggerStay(Collider other) {
        PlayerMovement player = other.GetComponent<PlayerMovement>();
        if (player)
        {
            var branchSprite = GetComponent<Transform>().GetChild(0);
            branchSprite.GetComponent<SpriteRenderer>().color = UnityEngine.Random.ColorHSV(0.0f, 1.0f, 0.5f, 1.0f, 0.8f, 0.8f);
        } 
    }

    private void OnTriggerExit(Collider other) {
        PlayerMovement player = other.GetComponent<PlayerMovement>();
        if (player)
        {
            player.selected_branches.Remove(this);
            isSelected = false;
            EventBus.Publish<GrowProgressEvent>(new GrowProgressEvent(transform,
                10, 10, false, GetInstanceID()));
        }
    }


    void ResourceChangeHandler(ResourceChangeEvent rc) {
        if (rc.PlayerID == PlayerID) {
            resources = rc.resource;
        }
    }

    public int GetPlayerID() {
        if (PlayerID != 0) {
            return PlayerID;
        } else {
            BranchController bc = transform.parent.gameObject.GetComponent<BranchController>();
            if (bc) return bc.GetPlayerID();
            GameController gc = transform.parent.gameObject.GetComponent<GameController>();
            if (gc) return gc.PlayerID;
            Debug.Log("Fail to find player ID!");
            return 0;
        }
    }

    void CheckPointHandler(ShieldEvent e)
    {
        //check playerID if multiple mode
        if (GetPlayerID() != e.playerID)
        {
            return;
        }
        if (createdTime < e.checkedTime)
        {
            isChecked = true;
            var jointSprite = GetComponent<Transform>().GetChild(1);
            jointSprite.GetComponent<SpriteRenderer>().color = new Color(0.85f, 0.85f, 0.85f, 1f);
        }
    }

}

public class BranchType {
    public enum BType {
        Growing,
        Old,
    }
    BType type = BType.Growing;
    int progress = 0;
    static int maxProgress = 100;

    public BType GetBType()
    {
        return type;
    }

    public int Revitalize(int resources)
    {
        // revitalize/grow a leaf branch
        // return the amount of resources consumed
        if (type != BType.Old)
        {
            return 0;
        }
        var consumed = Math.Min(maxProgress - progress, resources);
        progress += consumed;
        if (progress == maxProgress)
        {
            type = BType.Growing;
            progress = 0;
        }
        return consumed;
    }

    public void Mature()
    {
        type = BType.Old;
    }
}