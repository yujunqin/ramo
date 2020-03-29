using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldPool : MonoBehaviour
{
    public GameObject shieldPrefab;  
    float Ymax = 5f;
    float Ymin = 3f;
    float Xmax = 7f;
    float Xmin = 3f;

    float currentY = 3f;
    float currentX;
    GameObject currentShieldL;
    GameObject currentShieldR;
    Subscription<ShieldEvent> shieldSub;
    
    // Start is called before the first frame update
    void Start()
    {
        Random.InitState((int)System.DateTime.Now.Ticks);
        currentX = Random.Range(Xmin, Xmax);
        currentShieldL = (GameObject)Instantiate(shieldPrefab, new Vector2(-currentX, currentY) + (Vector2) transform.position, Quaternion.identity);
        currentShieldL.GetComponent<ShieldController>().playerID = 1;
        currentShieldR = (GameObject)Instantiate(shieldPrefab, new Vector2(currentX, currentY) + (Vector2) transform.position, Quaternion.identity);
        currentShieldR.GetComponent<ShieldController>().playerID = 2;
        shieldSub = EventBus.Subscribe<ShieldEvent>(ShieldHandler);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void ShieldHandler(ShieldEvent e)
    {
        currentY += Random.Range(Ymin, Ymax);
        currentX = Random.Range(Xmin, Xmax);
        currentShieldL = (GameObject)Instantiate(shieldPrefab, new Vector2(-currentX, currentY) + (Vector2) transform.position, Quaternion.identity);
        currentShieldL.GetComponent<ShieldController>().playerID = 1;
        currentShieldR = (GameObject)Instantiate(shieldPrefab, new Vector2(currentX, currentY) + (Vector2) transform.position, Quaternion.identity);
        currentShieldR.GetComponent<ShieldController>().playerID = 2;
    }
}
