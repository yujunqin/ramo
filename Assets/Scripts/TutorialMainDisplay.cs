using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialMainDisplay : MonoBehaviour
{
    Subscription<PlayerProgressEvent> sub;
    public int PlayerID;
    void Start()
    {
        sub = EventBus.Subscribe<PlayerProgressEvent>(ProgressHandler);
    }

    void ProgressHandler(PlayerProgressEvent e) {
        if (e.PlayerID == PlayerID) {
            if (e.progress == "first buff") {
                string key = (PlayerID == 1) ? "Q" : ".";
                StartCoroutine(LeaveAndBack("Press " + key + " to prune a branch."));
            }
            if (e.progress == "first prune") {
                string key = (PlayerID == 1) ? "SPACE" : "R SHIFT";
                StartCoroutine(LeaveAndBack("Hold " + key + " to create a bomb. Left/Right to aim. Release to launch."));
            }
            if (e.progress == "first bomb") {
                StartCoroutine(LeaveAndBack("Get free bombs from chests!"));
            }
            if (e.progress == "first chest") {
                StartCoroutine(LeaveAndBack("Grow to a shield for protection!"));
            }
            if (e.progress == "first shield") {
                StartCoroutine(LeaveAndBack("Get a star! You will need 3 stars to win the game!"));
            }
            if (e.progress == "first checkpoint") {
                StartCoroutine(LeaveAndBack("Excellent! Now you will compete to grow into the sky!"));
            }
        }
    }

    IEnumerator LeaveAndBack(string s) {
        GetComponentInParent<PanelLerp>().Move(new Vector3(0, 500f, 0));
        yield return new WaitForSeconds(1f);
        GetComponent<Text>().text = s;
        GetComponentInParent<PanelLerp>().BackToInit();

    }

}
