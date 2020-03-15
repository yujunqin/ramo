using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointController : MonoBehaviour
{
    bool isChecked;
    public int playerID;
    // Start is called before the first frame update
    void Start()
    {
        isChecked = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.CompareTag("Branch") && !isChecked)
        {
            EventBus.Publish<CheckPointEvent>(new CheckPointEvent(Time.time, playerID));
            isChecked = true;
        }
    }
}

class CheckPointEvent{
    public float checkedTime;
    public int playerID;
    public CheckPointEvent(float _checkedTime, int _playerID)
    {
        checkedTime = _checkedTime;
        playerID = _playerID;
    }
}
