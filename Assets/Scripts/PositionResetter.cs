using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionResetter : MonoBehaviour
{
    Subscription<NewRoundEvent> sub;
    // Start is called before the first frame update
    void Start()
    {
        sub = EventBus.Subscribe<NewRoundEvent>(ResetPos);
    }

    void ResetPos(NewRoundEvent e) {
        if (GetComponent<PlayerMovement>().PlayerID == 1) {
            transform.position = new Vector2(-4.5f, -2.5f);
        } else {
            transform.position = new Vector2(4.5f, -2.5f);
        }
    }

}
