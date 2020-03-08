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
                    offset = new Vector2(0.2f, 0),
                    direction = 0.0f,
                    cost = 100,
                    subContexts = new List<Context>{
                        new Context {
                            offset = new Vector2(0.2f, 0),
                            direction = 45.0f,
                            cost = 100,
                            subContexts = new List<Context>{},
                        },
                        new Context {
                            offset = new Vector2(0.2f, 0),
                            direction = -45.0f,
                            cost = 100,
                            subContexts = new List<Context>{},
                        }
                    }
                },
            };
        }
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
        // TODO: find a better way to visually represent the branch's maturity and resourceDeposit
        float maturity = 1.0f;
        if (GetComponent<BranchController>().type.GetBType() == BranchType.BType.Old) {
            maturity = 0.0f;
        }
        float deposit = (float) resourcesDeposit / ResourcesNeeded();
        float hp = (float) hits / maxHits;
        branchSprite.GetComponent<SpriteRenderer>().color = new Color(deposit, maturity, hp);
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
            }
            return consumed;
        }
    }
    
    public int Damage(int damage)
    {
        if (root)
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
        Destroy(this);
        return resources + cost;
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