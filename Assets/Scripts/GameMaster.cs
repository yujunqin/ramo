using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameMaster : MonoBehaviour
{
    Subscription<HeightChangeEvent> sub;
    Subscription<BuffEvent> buffSub;
    Subscription<ShieldEvent> cpSub;
    Subscription<GameStartEvent> st;
    bool finished = false;
    bool[] first_buff, first_check;
    int round = 1, bluewin = 0;
    public static int total_round = 3;
    private void Start() {
        sub = EventBus.Subscribe<HeightChangeEvent>(Judge);
        buffSub = EventBus.Subscribe<BuffEvent>(BuffEventHandler);
        cpSub = EventBus.Subscribe<ShieldEvent>(CheckPointEventHandler);
        st = EventBus.Subscribe<GameStartEvent>(ResetGame);
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

            if (h.PlayerID == 1) { ++bluewin; }

            int yellow_win = round - bluewin;
            if (yellow_win * 2 > total_round || bluewin * 2 > total_round) {
                EventBus.Publish<GameOverEvent>(new GameOverEvent(h.PlayerID, bluewin, round - bluewin));
                return;
            } else {
                EventBus.Publish<RoundWinnerEvent>(new RoundWinnerEvent(h.PlayerID, round, bluewin, round - bluewin));
                Debug.Log("WIN");
            }
            ++round;
            StartCoroutine(WaitAndReset());
        }
    }

    void BuffEventHandler(BuffEvent e) {
        if (round == 1 && first_buff[e.playerIndex]) {
            EventBus.Publish<PlayerProgressEvent>(new PlayerProgressEvent("first buff", e.playerIndex));
            first_buff[e.playerIndex] = false;
        }
    }

    void CheckPointEventHandler(ShieldEvent e) {
        if (round == 1 && first_check[e.playerID]) {
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

    IEnumerator WaitAndReset() {
        yield return new WaitForSeconds(5f);
        yield return SceneUtility.UnloadAll();
        yield return SceneUtility.LoadAll(round);
        EventBus.Publish<ResourceChangeEvent>(new ResourceChangeEvent(1, 1000));
        EventBus.Publish<ResourceChangeEvent>(new ResourceChangeEvent(2, 1000));
        EventBus.Publish<SpeedChangeEvent>(new SpeedChangeEvent(1, 50));
        EventBus.Publish<SpeedChangeEvent>(new SpeedChangeEvent(2, 50));
        finished = false;
    }

    public void ResetGame(GameStartEvent e) {
        StartCoroutine(SceneUtility.LoadAll(round));
        EventBus.Publish<ResourceChangeEvent>(new ResourceChangeEvent(1, 1000));
        EventBus.Publish<ResourceChangeEvent>(new ResourceChangeEvent(2, 1000));
        EventBus.Publish<SpeedChangeEvent>(new SpeedChangeEvent(1, 50));
        EventBus.Publish<SpeedChangeEvent>(new SpeedChangeEvent(2, 50));
        finished = false;
    }
}

class GameOverEvent{
    public int winner, blue_score, yellow_score;
    public GameOverEvent(int win, int b, int y) {
        winner = win;
        blue_score = b;
        yellow_score = y;
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

class NewRoundEvent {
    public int round;
    public NewRoundEvent(int r) {
        round = r;
    }
}

class RoundWinnerEvent{
    public int winner, round, blue_score, yellow_score;
    public RoundWinnerEvent(int win, int r, int b, int y) {
        winner = win;
        round = r;
        blue_score = b;
        yellow_score = y;
    }
}

public class GameStartEvent{}