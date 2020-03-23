using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CheckPointHint : MonoBehaviour
{
    Subscription<PlayerProgressEvent> sub;
    Text t;
    int first_buff_cnt = 0, first_cp_cnt = 0;
    void Start()
    {
        sub = EventBus.Subscribe<PlayerProgressEvent>(PlayerProgressEventHandler);
        t = GetComponent<Text>();
    }

    void PlayerProgressEventHandler(PlayerProgressEvent e){
        if (e.progress == "first buff") {
            ++first_buff_cnt;
        }
        if (e.progress == "first checkpoint") {
            ++first_cp_cnt;
        }
        if (first_buff_cnt == 2) {
            t.enabled = true;
        }
        if (first_cp_cnt == 2) {
            gameObject.SetActive(false);
        }
    }
}
