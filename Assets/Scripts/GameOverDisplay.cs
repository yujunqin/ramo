using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOverDisplay : MonoBehaviour
{
    Subscription<GameOverEvent> sub;
    Text t;
    // Start is called before the first frame update
    void Start()
    {
        sub = EventBus.Subscribe<GameOverEvent>(GameOver);
        t = GetComponent<Text>();
    }

    void GameOver(GameOverEvent e) {
        t.enabled = true;
        if (e.winner == 1) {
            t.text = "Blue Wins! Press R to restart.";
        } else {
            t.text = "Red Wins! Press R to restart.";
            t.color = new Color(1f, 0, 0);
        }
    }

}
