using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameMaster : MonoBehaviour
{
    Subscription<HeightChangeEvent> sub;
    Subscription<BuffEvent> buffSub;
    Subscription<CheckPointEvent> cpSub;
    bool finished = false;
    bool[] first_buff, first_check;

    private void Start() {
        sub = EventBus.Subscribe<HeightChangeEvent>(Judge);
        buffSub = EventBus.Subscribe<BuffEvent>(BuffEventHandler);
        cpSub = EventBus.Subscribe<CheckPointEvent>(CheckPointEventHandler);
        first_buff = new bool[3];
        first_check = new bool[3];
        for (int i = 1; i <= 2; ++i) {
            first_check[i] = true;
            first_buff[i] = true;
        }
    }
    void Judge(HeightChangeEvent h) {
        if (!finished && h.height >= 100f) {
            finished = true;
            EventBus.Publish<GameOverEvent>(new GameOverEvent(h.PlayerID));
        }
    }

    void BuffEventHandler(BuffEvent e) {
        if (first_buff[e.playerIndex]) {
            EventBus.Publish<PlayerProgressEvent>(new PlayerProgressEvent("first buff", e.playerIndex));
            first_buff[e.playerIndex] = false;
        }
    }

    void CheckPointEventHandler(CheckPointEvent e) {
        if (first_check[e.playerID]) {
            EventBus.Publish<PlayerProgressEvent>(new PlayerProgressEvent("first checkpoint", e.playerID));
            first_check[e.playerID] = false;
        }
    }

    private void Update() {
        // if (Input.GetKeyDown(KeyCode.R)) {
        //     // SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        //     SceneManager.LoadScene("Menu", LoadSceneMode.Single);
        // }
    }
}

class GameOverEvent{
    public int winner;
    public GameOverEvent(int win) {
        winner = win;
    }
}

class PlayerProgressEvent{
    public int PlayerID;
    public string progress;
    //reach root
    //first grow
    //first buff
    //first checkpoint
    public PlayerProgressEvent(string progressText, int id) {
        progress = progressText;
        PlayerID = id;
    }
}