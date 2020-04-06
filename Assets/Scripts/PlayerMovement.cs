using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.Analytics;
using TMPro;

public class PlayerMovement : MonoBehaviour
{
    Rigidbody rb;
    public float MoveSpeed = 4f;
    public bool pruning = false;
    public bool growing = false;
    bool first = true;
    public HashSet<BranchController> selected_branches;
    public GameObject bomb;

    GameObject bombIns;

    Subscription<BuffEvent> buffSubscription;
    bool isSpeedingUp = false;
    bool isSpeedingDown = false;
    float curBuffTime = 0f;
    float duration = 0f;

    static int nextPlayerID = 1;
    public int PlayerID;
    public GameObject player1;
    public GameObject player2;
    private GameObject playerIns;
    private Animator playerAnim;

    int resource;
    int free_bomb = 0;
    Subscription<ResourceChangeEvent> resSub;
    Subscription<FreeBombEvent> fbs;

    float movementTime = 0f;

    bool analytics;

    void Awake()
    {
        PlayerID = nextPlayerID;
        nextPlayerID++;
        Debug.Log(PlayerID);
    }

    void Start() {
        // Assign sprites; blue is 1, yellow is 2
        if (PlayerID == 1)
        {
        //    Sprite sprite = Resources.LoadAll<Sprite>("External/Cute Birds/PNG Files/Blue Bird")[0];
        //    transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = sprite;
            playerIns = Instantiate(player1, transform.position, Quaternion.identity);
            playerIns.transform.SetParent(transform);
            playerIns.transform.localPosition = new Vector3(0f, -0.63f, 0f);
            playerIns.transform.localScale = new Vector3(0.1f, 0.1f, 1f);
        } else
        {
            // Sprite sprite = Resources.LoadAll<Sprite>("External/Cute Birds/PNG Files/Yellow Bird")[0];
            // transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = sprite;
            playerIns = Instantiate(player2, transform.position, Quaternion.identity);
            playerIns.transform.SetParent(transform);
            playerIns.transform.localPosition = new Vector3(0f, -0.66f, 0f);
            playerIns.transform.localScale = new Vector3(0.09f, 0.09f, 1f);
        }
        
        rb = GetComponent<Rigidbody>();
        selected_branches = new HashSet<BranchController>();
        buffSubscription = EventBus.Subscribe<BuffEvent>(_OnBuffUpdated);
        resSub = EventBus.Subscribe<ResourceChangeEvent>(ResourceChangeHandler);
        fbs = EventBus.Subscribe<FreeBombEvent>(FreeBombEventHandler);
        playerAnim = playerIns.GetComponent<Animator>();

        analytics = GameObject.FindWithTag("GameController").GetComponent<GameMaster>().analytics;
    }

    void Update() {
        // Move();
        // Prune();
        // Grow();
        // Bomb();

        if (bombIns)
        {
            // Aim the bomb
            GetComponent<Rigidbody>().velocity = Vector3.zero;
            var action = GetComponent<PlayerInput>().actions.FindAction("Aim");
            bombIns.GetComponent<BombController>().direction.x += 0.2f * Time.deltaTime * action.ReadValue<Vector2>().x;
        } else {
            // Move the player
            var action = GetComponent<PlayerInput>().actions.FindAction("Move");
            var velocity = action.ReadValue<Vector2>();
            if (velocity != new Vector2(0f, 0f))
            {
                movementTime += Time.deltaTime;
                GetComponent<Rigidbody>().velocity = MoveSpeed * velocity * Mathf.Min((movementTime * 1f + 0.4f), 5f);
            }
            else
            {
                movementTime = 0f;
                GetComponent<Rigidbody>().velocity = MoveSpeed * velocity;
            }
        }

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
        playerAnim.SetFloat("vel_x", rb.velocity.x);
        playerAnim.SetFloat("vel_y", rb.velocity.y); 
    }

    public void OnMove(InputAction.CallbackContext context)
    {
    }

    public void OnGrow(InputAction.CallbackContext context)
    {
        if (!context.performed)
        {
            return;
        }
        List<BranchController> deletion_list = new List<BranchController>();
        if (selected_branches != null)
        {
            if (selected_branches.Count > 0 && first) {
                EventBus.Publish<PlayerProgressEvent>(new PlayerProgressEvent("first grow", PlayerID));
                first = false;
            }
            foreach (var branch in selected_branches) {
                if (!branch) {
                    deletion_list.Add(branch);
                    continue;
                }

                if (resource == 0)
                {
                    AudioClip clip = Resources.Load<AudioClip>("Sound Effects/GrowError");
                    AudioSource.PlayClipAtPoint(clip, Camera.main.transform.position, 1f);
                }
                else
                {
                    AudioClip clip = Resources.Load<AudioClip>("Sound Effects/Grow");
                    AudioSource.PlayClipAtPoint(clip, Camera.main.transform.position, 1f);
                }

                resource -= branch.Grow(resource);
                EventBus.Publish<ResourceChangeEvent>(new ResourceChangeEvent(PlayerID, resource));
            }
        }
        foreach (var branch in deletion_list) {
            selected_branches.Remove(branch);
        }
    }

    public void OnPrune(InputAction.CallbackContext context)
    {
        if (!context.performed)
        {
            return;
        }
        List<BranchController> deletion_list = new List<BranchController>();
        if (selected_branches != null)
        {
            int total_farmed = 0;
            foreach (var branch in selected_branches) {
                if (!branch) {
                    deletion_list.Add(branch);
                    continue;
                }

                AudioClip clip = Resources.Load<AudioClip>("Sound Effects/Prune");
                AudioSource.PlayClipAtPoint(clip, Camera.main.transform.position, 1f);
                total_farmed += branch.Damage(1000);
            }

            resource += total_farmed;
            EventBus.Publish<ResourceChangeEvent>(new ResourceChangeEvent(PlayerID, resource));

            if (total_farmed != 0)
            {
                GameObject tooltipPrefab = Resources.Load<GameObject>("Prefabs/Floating Text");
                tooltipPrefab.GetComponent<TextMeshPro>().text = "+" + total_farmed.ToString() + " wood!";
                GameObject tooltip = Instantiate(tooltipPrefab, transform.position, Quaternion.identity);
                Destroy(tooltip, 1f);
            }
        }

        foreach (var branch in deletion_list) {
            selected_branches.Remove(branch);
        }

        if (analytics)
        {
            AnalyticsEvent.Custom("Prune!", 
                new Dictionary<string, object>(){{"Time", Time.time}, {"PlayerIndex", PlayerID}, {"Resource", resource}}
            );
        }
    }

    public void OnRestart(InputAction.CallbackContext context)
    {
        if (!context.performed)
        {
            return;
        }
        PlayerMovement.nextPlayerID = 1;
        PlayerManager.first = true;
        SceneManager.LoadScene("Menu", LoadSceneMode.Single);
    }

    public void OnBombard(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            if (!bombIns)
            {
                // TODO: Kill whoever wrote the fucking pile of shit called InputSystem
                // This method is triggered for player 1 twice or even thrice for whatever reason
                // Therefore this workaround is put in place
                if (transform.position.x == 0.0f && transform.position.y == 0.0f)
                {
                    return;
                }

                if (free_bomb == 0 && resource < 1000)
                {
                    return;
                }
                if (free_bomb > 0) {
                    --free_bomb;
                    EventBus.Publish<FreeBombEvent>(new FreeBombEvent(PlayerID, false));
                } else {
                    resource -= 1000;
                    EventBus.Publish<ResourceChangeEvent>(new ResourceChangeEvent(PlayerID, resource));
                }
                bombIns = Instantiate(bomb, transform.position, Quaternion.identity);
                bombIns.GetComponent<BombController>().PlayerID = PlayerID;
            }
        } else if (context.phase == InputActionPhase.Canceled)
        {
            if (bombIns)
            {
                bombIns.GetComponent<BombController>().ThrowBomb();
                bombIns = null;
            }
        }
        if (analytics)
        {
            AnalyticsEvent.Custom("Bombard!", 
                new Dictionary<string, object>(){{"Time", Time.time}, {"PlayerIndex", PlayerID}, {"Resource", resource}}
            );
        }
    }

    public void OnAim(InputAction.CallbackContext context)
    {
        // if (bombIns)
        // {
        //     bombIns.GetComponent<BombController>().direction.x += 0.02f * context.ReadValue<Vector2>().x;
        // }
    }

    void Move() {
        //Temporary, will move to controllers
        Vector2 velocity = Vector2.zero;
        velocity.x = Input.GetAxis("Horizontal" + PlayerID.ToString());
        velocity.y = Input.GetAxis("Vertical" + PlayerID.ToString());
        rb.velocity = MoveSpeed * velocity.normalized;
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

    void FreeBombEventHandler(FreeBombEvent e) {
        if (e.PlayerID == PlayerID) {
            if (e.isGet) {
                ++free_bomb;
            }
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