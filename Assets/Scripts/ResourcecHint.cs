using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResourcecHint : MonoBehaviour
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
        if (!isActiveAndEnabled) {
            return;
        }
        if (e.PlayerID == PlayerID) {
            if (e.progress == "first grow") {
                t.text = "Grow to a log for extra resources!";
                GetComponentInParent<PanelLerp>().Move(new Vector3(0f, 500f, 0f));
            }
            if (e.progress == "first buff") {
                GetComponentInParent<PanelLerp>().BackToInit();
            }
        }
    }
    void NewRound(NewRoundEvent e) {
        if (e.round > 1) {
            gameObject.SetActive(false);
            transform.parent.gameObject.SetActive(false);
        }
    }
}
