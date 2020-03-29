using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheerTextDisplay : MonoBehaviour
{
    Vector3 dest;
    Subscription<ChestConvertEvent> chSub;
    public int PlayerID;
    void Start()
    {
        dest = transform.position;
        chSub = EventBus.Subscribe<ChestConvertEvent>(Cheer);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.Lerp(transform.position, dest, 0.05f);
    }

    void Out() {
        dest.y += 4f;
    }

    void In() {
        dest.y -= 4f;
    }

    void Cheer(ChestConvertEvent e) {
        if (PlayerID == e.PlayerID) {
            StartCoroutine(DisplayAndDisappear());
        }
    }

    IEnumerator DisplayAndDisappear() {
        In();
        yield return new WaitForSeconds(3f);
        Out();
    }
}
