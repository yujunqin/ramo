using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    Rigidbody rb;
    public float MoveSpeed = 4f;
    public bool pruning = false;
    public bool growing = false;
    public HashSet<BranchController> selected_branches;
    public GameObject bomb;

    Subscription<BuffEvent> buffSubscription;
    bool isSpeedingUp = false;
    bool isSpeedingDown = false;
    float curBuffTime = 0f;
    float duration = 0f;

    static int nextPlayerID = 1;
    public int PlayerID;

    int resource;
    Subscription<ResourceChangeEvent> resSub;
    void Start() {
        PlayerID = nextPlayerID;
        nextPlayerID++;
        Debug.Log(nextPlayerID);
        // Assign sprites; blue is 1, yellow is 2
        if (PlayerID == 1)
        {
            Sprite sprite = Resources.LoadAll<Sprite>("External/Cute Birds/PNG Files/Blue Bird")[0];
            transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = sprite;
        } else
        {
            Sprite sprite = Resources.LoadAll<Sprite>("External/Cute Birds/PNG Files/Yellow Bird")[0];
            transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = sprite;
        }
        rb = GetComponent<Rigidbody>();
        selected_branches = new HashSet<BranchController>();
        buffSubscription = EventBus.Subscribe<BuffEvent>(_OnBuffUpdated);
        resSub = EventBus.Subscribe<ResourceChangeEvent>(ResourceChangeHandler);
    }

    void Update() {
        // Prune();
        // Grow();
        Bomb();
        if (isSpeedingUp && curBuffTime + duration > Time.time)
        {
            MoveSpeed = 6f;
            EventBus.Publish<BuffStatusEvent>(new BuffStatusEvent("SPEED UP", PlayerID));
        }
        else if (isSpeedingDown && curBuffTime + duration > Time.time)
        {
            MoveSpeed = 2f;
            EventBus.Publish<BuffStatusEvent>(new BuffStatusEvent("SPEED DOWN", PlayerID));
        }
        else 
        {
            MoveSpeed = 4f;
            EventBus.Publish<BuffStatusEvent>(new BuffStatusEvent("", PlayerID));
        }
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        Vector2 velocity = context.ReadValue<Vector2>();
        // rb.velocity = MoveSpeed * velocity.normalized;
        // rb.velocity = MoveSpeed * velocity;
        GetComponent<Rigidbody>().velocity = MoveSpeed * velocity;
    }

    public void OnGrow(InputAction.CallbackContext context)
    {
        List<BranchController> deletion_list = new List<BranchController>();
        foreach (var branch in selected_branches) {
            if (!branch) {
                deletion_list.Add(branch);
                continue;
            }
            resource -= branch.Grow(resource);
            EventBus.Publish<ResourceChangeEvent>(new ResourceChangeEvent(PlayerID, resource));
        }
        foreach (var branch in deletion_list) {
            selected_branches.Remove(branch);
        }
    }
    
    void Prune() {
        if (InputController.PrunePressed(PlayerID)){
            List<BranchController> deletion_list = new List<BranchController>();
            foreach (var branch in selected_branches) {
                if (!branch) {
                    deletion_list.Add(branch);
                    continue;
                }
                resource += branch.Damage(1000);
                EventBus.Publish<ResourceChangeEvent>(new ResourceChangeEvent(PlayerID, resource));
            }
            foreach (var branch in deletion_list) {
                selected_branches.Remove(branch);
            }
        }
    }

    void Grow() {
        if (InputController.GrowPressed(PlayerID)){
            List<BranchController> deletion_list = new List<BranchController>();
            foreach (var branch in selected_branches) {
                if (!branch) {
                    deletion_list.Add(branch);
                    continue;
                }
                resource -= branch.Grow(resource);
                EventBus.Publish<ResourceChangeEvent>(new ResourceChangeEvent(PlayerID, resource));
            }
            foreach (var branch in deletion_list) {
                selected_branches.Remove(branch);
            }
        }
    }

    void Bomb() {
        if (InputController.BombPressed(PlayerID)) {
            GameObject bombIns = Instantiate(bomb, transform.position, Quaternion.identity);
            bombIns.GetComponent<BombController>().PlayerID = PlayerID;
        }
    }

    void _OnBuffUpdated(BuffEvent e)
    {
        if (e.playerIndex == PlayerID)
        {
            switch(e.type)
            {
                // make sure only one speeding buff is exist at one time
                case BuffController.buffType.speedDown:
                    if (isSpeedingUp) {
                        isSpeedingUp = false;
                        curBuffTime = 0f;
                    }
                    if (isSpeedingDown)
                    {
                        curBuffTime = e.effectiveTime;
                        duration = e.duration;
                    }
                    else {
                        isSpeedingDown = true;
                        curBuffTime = e.effectiveTime;
                        duration = e.duration;
                    }
                break;
                case BuffController.buffType.speedUp:
                    if (isSpeedingDown) {
                        isSpeedingDown = false;
                        curBuffTime = 0f;
                    }
                    if (isSpeedingUp)
                    {
                        curBuffTime = e.effectiveTime;
                        duration = e.duration;
                    }
                    else {
                        isSpeedingUp = true;
                        curBuffTime = e.effectiveTime;
                        duration = e.duration;
                    }
                break;
            }
        }
    }

    void ResourceChangeHandler(ResourceChangeEvent rc) {
        if (rc.PlayerID == PlayerID) {
            resource = rc.resource;
        }
    }

}




class PruneEvent {
    public int PlayerID;
    //More variables here if necessary

    public PruneEvent(int id) {
        PlayerID = id;
    }
}