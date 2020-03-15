using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointPool : MonoBehaviour
{
    public GameObject checkPointPrefab;  
    float Ymax = 5f;
    float Ymin = 3f;
    float Xmax = 7f;
    float Xmin = 3f;

    float currentHeight = 3f;
    GameObject currentCheckpoint;
    Subscription<CheckPointEvent> checkpointSub;
    
    // Start is called before the first frame update
    void Start()
    {
        Random.InitState(494);
        currentCheckpoint = (GameObject)Instantiate(checkPointPrefab, new Vector2(Random.Range(-Xmax, Xmax), currentHeight), Quaternion.identity);
        checkpointSub = EventBus.Subscribe<CheckPointEvent>(CheckPointHandler);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void CheckPointHandler(CheckPointEvent e)
    {
        currentHeight += Random.Range(Ymin, Ymax);
        currentCheckpoint = (GameObject)Instantiate(checkPointPrefab, new Vector2(Random.Range(-Xmax, Xmax), currentHeight), Quaternion.identity);
    }
}
