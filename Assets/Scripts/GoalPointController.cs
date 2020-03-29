using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalPointController : MonoBehaviour
{
    public int index;
    public int playerID;
    // public AudioClip ;
    bool isChecked;
    bool isTouchable;
    Subscription<GoalPointEvent> goalPointSub;
    // Start is called before the first frame update
    void Start()
    {
        isChecked = false;
        isTouchable = false;
        if (index == 1)
        {
            isTouchable = true;
        }

        goalPointSub = EventBus.Subscribe<GoalPointEvent>(GoalPointHandler);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void GoalPointHandler(GoalPointEvent e)
    {
        if (e.playerID == playerID && e.index == index - 1 && !isTouchable)
        {
            isTouchable = true;
        }
    }

    private void OnTriggerEnter(Collider other) 
    {
        if (!isTouchable)
        {
            return;
        }

        if (other.gameObject.CompareTag("Branch") && !isChecked)
        {
            if (other.gameObject.GetComponent<BranchController>().PlayerID == playerID)
            {
                EventBus.Publish<GoalPointEvent>(new GoalPointEvent(index, playerID));
                // lightup goalpoint
                GetComponent<SpriteRenderer>().color = Color.white;
                // AudioSource.PlayClipAtPoint(checkpointClip, Camera.main.transform.position);
            }
        }
    }
}

public class GoalPointEvent
{
    public int index;
    public int playerID;
    public GoalPointEvent(int _index, int _playerID)
    {
        index = _index;
        playerID = _playerID;
    }
}
