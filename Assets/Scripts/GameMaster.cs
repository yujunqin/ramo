using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameMaster : MonoBehaviour
{
    Subscription<HeightChangeEvent> sub;

    private void Start() {
        sub = EventBus.Subscribe<HeightChangeEvent>(Judge);
    }
    void Judge(HeightChangeEvent h) {
        if (h.height >= 100f) {
            EventBus.Publish<GameOverEvent>(new GameOverEvent(h.PlayerID));
        }
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.R)) {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}

class GameOverEvent{
    public int winner;
    public GameOverEvent(int win) {
        winner = win;
    }
}