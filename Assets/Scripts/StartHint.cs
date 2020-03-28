using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartHint : MonoBehaviour
{
    public int PlayerID;
    Subscription<PlayerProgressEvent> sub;
    Subscription<NewRoundEvent> nr;
    Text t;
    void Start()
    {
        sub = EventBus.Subscribe<PlayerProgressEvent>(PlayerProgressEventHandler);
        nr = EventBus.Subscribe<NewRoundEvent>(NewRound);
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

    void NewRound(NewRoundEvent e) {
        if (e.round > 1) {
            gameObject.SetActive(false);
        }
    }
}
