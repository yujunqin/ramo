using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartHint : MonoBehaviour
{
    public int PlayerID;
    private Subscription<PlayerProgressEvent> sub;
    private Subscription<NewRoundEvent> nr;
    private Text t;
    private bool pruneShown;
    private bool firstGrowDone;

    private void Start()
    {
        sub = EventBus.Subscribe<PlayerProgressEvent>(PlayerProgressEventHandler);
        nr = EventBus.Subscribe<NewRoundEvent>(NewRound);
        t = GetComponent<Text>();
    }

    private void Update()
    {
    }

    void PlayerProgressEventHandler(PlayerProgressEvent e){
        if (!isActiveAndEnabled) {
            return;
        }
        if (e.PlayerID == PlayerID) {
            if (e.progress == "reach root") {
                if (PlayerID == 1) {
                    t.text = "Press E!";
                } else {
                    t.text = "Press /!";
                }
            }
            if (e.progress == "first grow") {
                 GetComponentInParent<PanelLerp>().Move(new Vector3(0, -200f, 0));
            }
        }
    }

    void NewRound(NewRoundEvent e) {
        if (e.round > 1) {
            gameObject.SetActive(false);
            transform.parent.gameObject.SetActive(false);
        }
    }

    IEnumerator ShowPruneTip()
    {
        if (PlayerID == 1) {
            t.text = "Prune = Q";
        }
        else {
            t.text = "Prune = .>";
        }
        yield return new WaitForSeconds(3f);
        GetComponentInParent<PanelLerp>().Move(new Vector3(0, -100f, 0));
    }
}
