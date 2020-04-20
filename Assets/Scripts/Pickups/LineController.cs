using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineController : MonoBehaviour
{
    public int targetIndex;
    public int playerID;
    Subscription<GoalPointEvent> goalPointSub;
    
    // Start is called before the first frame update
    void Start()
    {
        // foreach (var sr in GetComponentsInChildren<LineRenderer>()) {
        //     sr.enabled = false;
        // }
        var emission = GetComponent<ParticleSystem>().emission;
        if (targetIndex > 0) {
            emission.enabled = false;
        } else {
            emission.enabled = true;
        }
        goalPointSub = EventBus.Subscribe<GoalPointEvent>(GoalPointHandler);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void GoalPointHandler(GoalPointEvent e)
    {
        if (e.playerID == playerID && e.index == targetIndex)
        {
            // foreach (var sr in GetComponentsInChildren<ParticleSystem>()) {
            //     sr.enabled = true;
            // }
            var emission = GetComponent<ParticleSystem>().emission;
            emission.enabled = true;
            
        }
    }
}
