using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointController : MonoBehaviour
{
    bool isChecked;
    public int playerID;
    float checkedTime;
    // public AudioClip checkpointClip;

    Transform trans;
    // Start is called before the first frame update
    void Start()
    {
        isChecked = false;
        trans = GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isChecked)
        {
            if (Time.time < checkedTime + 3f)
            {
                float scale = 0.3f - 0.3f / (Time.time - checkedTime);
                trans.localScale = new Vector3(scale, scale, 1);
            }
            else
            {

            }
        }
    }

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.CompareTag("Branch") && !isChecked)
        {
            EventBus.Publish<CheckPointEvent>(new CheckPointEvent(Time.time, playerID));
            isChecked = true;
            checkedTime = Time.time;
            // gameObject.SetActive(false);
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
