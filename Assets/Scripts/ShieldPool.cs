using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldPool : MonoBehaviour
{
    public GameObject checkPointPrefab;  
    float Ymax = 5f;
    float Ymin = 3f;
    float Xmax = 7f;
    float Xmin = 3f;

    float currentY = 3f;
    float currentX;
    GameObject currentCheckpointL;
    GameObject currentCheckpointR;
    Subscription<CheckPointEvent> checkpointSub;
    
    // Start is called before the first frame update
    void Start()
    {
        Random.InitState((int)System.DateTime.Now.Ticks);
        currentX = Random.Range(Xmin, Xmax);
        currentCheckpointL = (GameObject)Instantiate(checkPointPrefab, new Vector2(-currentX, currentY) + (Vector2) transform.position, Quaternion.identity);
        currentCheckpointL.GetComponent<ShieldController>().playerID = 1;
        currentCheckpointR = (GameObject)Instantiate(checkPointPrefab, new Vector2(currentX, currentY) + (Vector2) transform.position, Quaternion.identity);
        currentCheckpointR.GetComponent<ShieldController>().playerID = 2;
        checkpointSub = EventBus.Subscribe<CheckPointEvent>(CheckPointHandler);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void CheckPointHandler(CheckPointEvent e)
    {
        currentY += Random.Range(Ymin, Ymax);
        currentX = Random.Range(Xmin, Xmax);
        currentCheckpointL = (GameObject)Instantiate(checkPointPrefab, new Vector2(-currentX, currentY) + (Vector2) transform.position, Quaternion.identity);
        currentCheckpointL.GetComponent<ShieldController>().playerID = 1;
        currentCheckpointR = (GameObject)Instantiate(checkPointPrefab, new Vector2(currentX, currentY) + (Vector2) transform.position, Quaternion.identity);
        currentCheckpointR.GetComponent<ShieldController>().playerID = 2;
    }
}
