using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BombCamDisplay : MonoBehaviour
{
    Subscription<BombEvent> sub;
    int bomb_cnt = 0;
    //Color visible = new Color(0.75f, 0.75f, 0.75f, 0.5f), invisible = new Color(0.75f, 0.75f, 0.75f, 0f);
    void Start()
    {
        sub = EventBus.Subscribe<BombEvent>(BombEventHandler);
    }

    void BombEventHandler(BombEvent e) {
        if (e.isExplode) {
            --bomb_cnt;
            if (bomb_cnt == 0) {
                GetComponent<RawImage>().enabled = false;
            }
        } else {
            ++bomb_cnt;
            GetComponent<RawImage>().enabled = true;
        }
    }

}
