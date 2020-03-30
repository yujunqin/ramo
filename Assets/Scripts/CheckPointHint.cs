using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CheckPointHint : MonoBehaviour
{
    Subscription<PlayerProgressEvent> sub;
    
    Subscription<NewRoundEvent> nr;
    Text t;
    int first_buff_cnt = 0, first_cp_cnt = 0;
    void Start()
    {
        sub = EventBus.Subscribe<PlayerProgressEvent>(PlayerProgressEventHandler);
        nr = EventBus.Subscribe<NewRoundEvent>(NewRound);
        t = GetComponent<Text>();
    }

    void PlayerProgressEventHandler(PlayerProgressEvent e){
        if (e.progress == "first buff") {
            ++first_buff_cnt;
        }
        if (e.progress == "first checkpoint") {
            ++first_cp_cnt;
        }
        if (!t.enabled && first_buff_cnt == 2) {
            t.enabled = true;
            StartCoroutine(WaitAndHide());
        }  
    }
    void NewRound(NewRoundEvent e) {
        if (e.round > 1) {
            gameObject.SetActive(false);
        }
    }

    IEnumerator WaitAndHide() {
        yield return new WaitForSeconds(10f);
        gameObject.SetActive(false);
    }
}
