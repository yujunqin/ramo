using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionResetter : MonoBehaviour
{
    Subscription<TransitionEvent> sub;
    // Start is called before the first frame update
    void Start()
    {
        sub = EventBus.Subscribe<TransitionEvent>(ResetPos);
    }

    void ResetPos(TransitionEvent e) {
        GameMaster.inst.StartCoroutine(resetPosition(gameObject));
    }

    IEnumerator resetPosition(GameObject go) {
        if (GetComponent<PlayerMovement>().PlayerID == 1) {
            transform.position = new Vector2(-4.5f, -2.5f);
        } else {
            transform.position = new Vector2(4.5f, -2.5f);
        }
        //go.SetActive(false);
        yield return new WaitForSeconds(3.5f);
        //go.SetActive(true);
    }

}
