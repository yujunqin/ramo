using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResourcecHint : MonoBehaviour
{
    public int PlayerID;
    Subscription<PlayerProgressEvent> sub;
    Text t;
    void Start()
    {
        sub = EventBus.Subscribe<PlayerProgressEvent>(PlayerProgressEventHandler);
        t = GetComponent<Text>();
    }

    void PlayerProgressEventHandler(PlayerProgressEvent e){
        if (e.PlayerID == PlayerID) {
            if (e.progress == "first grow") {
                t.text = "Grow to a log for extra resources!";
            }
            if (e.progress == "first buff") {
                gameObject.SetActive(false);
            }
        }
    }
}
