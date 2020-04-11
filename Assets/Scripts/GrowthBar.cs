using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrowthBar : MonoBehaviour
{
    public int playerID;
    Subscription<HeightChangeEvent> height_sub;
    // Start is called before the first frame update
    void Start()
    {
        height_sub = EventBus.Subscribe<HeightChangeEvent>(heightUpdate);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void heightUpdate(HeightChangeEvent h)
    {
        if (h.PlayerID != playerID)
        {
            return;
        }
        else
        {
            float progress = h.height / 300f;
        }
    }
}
