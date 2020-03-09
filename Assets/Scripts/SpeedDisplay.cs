using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpeedDisplay : MonoBehaviour
{
    Subscription<SpeedChangeEvent> sub;
    public int PlayerID;
    Text t;
    void Start() {
        sub = EventBus.Subscribe<SpeedChangeEvent>(SpeedChangeHandler);
        t = GetComponent<Text>();
    }

    void SpeedChangeHandler(SpeedChangeEvent sc) {
        if (sc.PlayerID == PlayerID) {
            t.text = "+" + sc.speed.ToString() + "/s";
        }
    }
}
