using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOverDisplay : MonoBehaviour
{
    Subscription<GameOverEvent> sub;
    Subscription<RoundWinnerEvent> rw;
    Subscription<NewRoundEvent> nr;
    Text t;
    static int blue_win = 0, yellow_win = 0;
    // Start is called before the first frame update
    void Awake()
    {
        sub = EventBus.Subscribe<GameOverEvent>(GameOver);
        rw = EventBus.Subscribe<RoundWinnerEvent>(RoundEnd);
        nr = EventBus.Subscribe<NewRoundEvent>(NewRound);
        t = GetComponent<Text>();
    }

    void GameOver(GameOverEvent e) {
        GetComponentInParent<PanelLerp>().BackToInit();
        t.enabled = true;
        blue_win = e.blue_score;
        yellow_win = e.yellow_score;
        if (e.winner == 1) {
            t.text = "\nBlue Bird Wins!\n Press Y button / P key to restart.";
            // t.color = Color.blue;
        } else {
            t.text = "\nYellow Bird Wins!\n Press Y button / P key to restart.";
            // t.color = Color.yellow;
        }
        t.text = blue_win.ToString() + ":" + yellow_win.ToString() + t.text;
    }

    void RoundEnd(RoundWinnerEvent e) {
        GetComponentInParent<PanelLerp>().BackToInit();
        t.enabled = true;
        string winner_name;
        if (e.winner == 1) {
            winner_name = "Blue Bird";
            // t.color = Color.blue;
            blue_win = e.blue_score;
        } else {
            winner_name = "Yellow Bird";
            // t.color = Color.yellow;
            yellow_win = e.yellow_score;
        }
        t.text = "Round " + e.round + " Winner: " + winner_name;
    }

    void NewRound(NewRoundEvent nr) {
        t.enabled = true;
        t.text = "Round " + nr.round.ToString() + "/" + GameMaster.total_round.ToString() + "\n" +
                 blue_win.ToString() + ":" + yellow_win.ToString() + "\nGO!!!";
        if (nr.round == GameMaster.total_round) {
            t.text += "\nFinal Round!!";
        }
        // t.color = Color.black;
        StartCoroutine(WaitAndTurnOff(3f));
    }

    IEnumerator WaitAndTurnOff(float time) {
        yield return new WaitForSeconds(time);
        t.enabled = false;
        GetComponentInParent<PanelLerp>().Move(new Vector3(0, -1000f, 0));
    }

}
