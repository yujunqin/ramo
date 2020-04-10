using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldPool : MonoBehaviour
{
    public GameObject shieldPrefab;  
    float Ymax = 4f;
    float Ymin = 2f;
    float Xmax = 8f;
    float Xmin = 1f;

    float currentY = 3f;
    float currentX;
    GameObject currentShieldL;
    GameObject currentShieldR;
    Subscription<ShieldEvent> shieldSub;
    
    // Start is called before the first frame update
    void Start()
    {
        Random.InitState((int)System.DateTime.Now.Ticks);
        // currentX = Random.Range(Xmin, Xmax);
        currentX = Mathf.Lerp(Xmin, Xmax, NextShieldPosition());
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
        // if (currentY > 16f) {
        //     return;
        // }
        // currentX = Random.Range(Xmin, Xmax);
        currentX = Mathf.Lerp(Xmin, Xmax, NextShieldPosition());
        currentShieldL = (GameObject)Instantiate(shieldPrefab, new Vector2(-currentX, currentY) + (Vector2) transform.position, Quaternion.identity);
        currentShieldL.GetComponent<ShieldController>().playerID = 1;
        currentShieldR = (GameObject)Instantiate(shieldPrefab, new Vector2(currentX, currentY) + (Vector2) transform.position, Quaternion.identity);
        currentShieldR.GetComponent<ShieldController>().playerID = 2;
    }

    float NextShieldPosition()
    {
        float limit = 3f;
        float t = GaussianRNG.NextGaussian(0f, 1f, -1f * limit, limit);
        if (t > 0f)
        {
            return t / limit / 2f;
        } else {
            return (t + limit) / limit / 2f + 0.5f;
        }
    }
}
