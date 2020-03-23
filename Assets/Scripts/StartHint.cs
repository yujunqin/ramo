using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartHint : MonoBehaviour
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
            if (e.progress == "reach root") {
                if (PlayerID == 1) {
                    t.text = "Press E!";
                } else {
                    t.text = "Press /!";
                }
            }
            if (e.progress == "first grow") {
                gameObject.SetActive(false);
            }
        }
    }
}
