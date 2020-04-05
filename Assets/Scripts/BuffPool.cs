using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffPool : MonoBehaviour
{
    // TODO: merge CheckpointPool with this file

    public GameObject buffPrefab;  
    public int buffCount;
    float Ymax = 2f;
    float Ymin = 0.5f;
    float Xmax = 7f;
    float Xmin = 1f;

    float currentY = 2f;
    float currentX;
    GameObject currentCheckpointL;
    GameObject currentCheckpointR;
    // Subscription<CheckPointEvent> checkpointSub;
    
    // Start is called before the first frame update
    void Start()
    {
        Random.InitState((int)System.DateTime.Now.Ticks);
        for (int i = 0; i < buffCount; i++)
        {
            currentY += Random.Range(Ymin, Ymax);
            currentX = Random.Range(Xmin, Xmax);
            currentCheckpointL = (GameObject)Instantiate(buffPrefab, new Vector2(-currentX, currentY) + (Vector2) transform.position, Quaternion.identity);
            currentCheckpointL.GetComponent<BuffController>().playerIndex = 1;
            currentCheckpointR = (GameObject)Instantiate(buffPrefab, new Vector2(currentX, currentY) + (Vector2) transform.position, Quaternion.identity);
            currentCheckpointR.GetComponent<BuffController>().playerIndex = 2;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
