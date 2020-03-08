using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeightReader : MonoBehaviour
{
    public int PlayerID;
    Subscription<HeightChangeEvent> sub;
    Text t;
    // Start is called before the first frame update
    void Start()
    {
        sub = EventBus.Subscribe<HeightChangeEvent>(ReadHeight);
        t = GetComponent<Text>();
    }

    void ReadHeight(HeightChangeEvent h) {
        if (h.PlayerID == PlayerID) {
            t.text = h.height.ToString() + " m";
        }
    }
}
