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
        if (pruneShown && firstGrowDone)
        {
            GetComponentInParent<PanelLerp>().Move(new Vector3(0f, -100f, 0f));
            gameObject.SetActive(false);
        }
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
                StartCoroutine(ShowPruneTip());
            }
            if (e.progress == "first grow") {
                firstGrowDone = true;
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
        yield return new WaitForSeconds(1);
        if (PlayerID == 1) {
            t.text = "Prune = Q";
        }
        else {
            t.text = "Prune = .>";
        }
        yield return new WaitForSeconds(1);
        pruneShown = true;
    }
}
