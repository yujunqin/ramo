using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheerTextDisplay : MonoBehaviour
{
    Subscription<ChestConvertEvent> chSub;
    public int PlayerID;
    void Start()
    {
        chSub = EventBus.Subscribe<ChestConvertEvent>(Cheer);
    }


    void Cheer(ChestConvertEvent e) {
        if (PlayerID == e.PlayerID) {
            StartCoroutine(DisplayAndDisappear());
        }
    }

    IEnumerator DisplayAndDisappear() {
        GetComponentInParent<PanelLerp>().Move(new Vector3(0, -400f, 0));
        yield return new WaitForSeconds(3f);
        GetComponentInParent<PanelLerp>().BackToInit();
    }
}
