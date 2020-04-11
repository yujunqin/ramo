using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Analytics;

public class GameMaster : MonoBehaviour
{
    Subscription<HeightChangeEvent> sub, chestSub;
    Subscription<BuffEvent> buffSub;
    Subscription<ShieldEvent> cpSub;
    Subscription<GameStartEvent> st;
    Subscription<TutorialEvent> tutorialSub;
    Subscription<ResourceChangeEvent> resSub;
    Subscription<GoalPointEvent> goalSub;
    bool finished = false;
    bool[] first_buff, first_check, chest_converted;
    int round = 1, bluewin = 0;
    public static int total_round = 3;
    public bool analytics = true;
    float last_analyzed_time = 0f;
    float[] height, resources;
    int[] goal_point;
    bool isTutorial;
    public Camera cam1, cam2;
    private void Start() {
        sub = EventBus.Subscribe<HeightChangeEvent>(Judge);
        chestSub = EventBus.Subscribe<HeightChangeEvent>(ConvertChest);
        buffSub = EventBus.Subscribe<BuffEvent>(BuffEventHandler);
        cpSub = EventBus.Subscribe<ShieldEvent>(CheckPointEventHandler);
        resSub = EventBus.Subscribe<ResourceChangeEvent>(ResChangeHandler);
        st = EventBus.Subscribe<GameStartEvent>(StartGame);
        tutorialSub = EventBus.Subscribe<TutorialEvent>(StartTutorial);
        goalSub = EventBus.Subscribe<GoalPointEvent>(GoalPointHandler);
        first_buff = new bool[3];
        first_check = new bool[3];
        chest_converted = new bool[3];
        height = new float[3];
        resources = new float[3];
        goal_point = new int[3];
        for (int i = 1; i <= 2; ++i) {
            first_check[i] = true;
            first_buff[i] = true;
            chest_converted[i] = false;
            height[i] = 0;
            resources[i] = 0f;
            goal_point[i] = 0;
        }

        if (analytics)
        {
            AnalyticsEvent.LevelStart(SceneManager.GetActiveScene().buildIndex, 
                new Dictionary<string, object>(){{"Date", System.DateTime.Now.ToString("MM/dd")}}
            );
        }
    }
    void Judge(HeightChangeEvent h) {
        if (analytics && Time.time >= last_analyzed_time + 3f)
        {
            AnalyticsEvent.Custom("HeightUpdate", 
                new Dictionary<string, object>{ {"Time", Time.time}, {"PlayerIndex", h.PlayerID}, {"Height", h.height} }
            );
            last_analyzed_time = Time.time;
        }
       

        if (!finished && h.height >= 300f && goal_point[h.PlayerID] >= 3) {
            finished = true;
            height[1] = 0;
            height[2] = 0;
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

        if (isTutorial && h.height >= 10f && !finished) {
            finished = true;
            StartCoroutine(WaitAndReset());
            //EventBus.Publish<GameStartEvent>(new GameStartEvent());
        }
    }

    void ConvertChest(HeightChangeEvent h) {
        if (finished) return;
        height[h.PlayerID] = h.height;
        if (height[1] >= 50f || height[2] >= 50f) {
            if (!chest_converted[2] && resources[2] < 1000 && height[1] - height[2] > 20f) {
                chest_converted[2] = true;
                EventBus.Publish<ChestConvertEvent>(new ChestConvertEvent(2));
            }
            if (!chest_converted[1] && resources[1] < 1000 && height[2] - height[1] > 20f) {
                chest_converted[1] = true;
                EventBus.Publish<ChestConvertEvent>(new ChestConvertEvent(1));
            }
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
        yield return SceneUtility.UnloadAll(isTutorial);
        yield return SceneUtility.LoadGame(round);
        EventBus.Publish<ResourceChangeEvent>(new ResourceChangeEvent(1, 1000));
        EventBus.Publish<ResourceChangeEvent>(new ResourceChangeEvent(2, 1000));
        EventBus.Publish<SpeedChangeEvent>(new SpeedChangeEvent(1, 50));
        EventBus.Publish<SpeedChangeEvent>(new SpeedChangeEvent(2, 50));
        finished = false;
        chest_converted[1] = chest_converted[2] = false;
        goal_point[1] = goal_point[2] = 0;
        isTutorial = false;
    }

    public void StartGame(GameStartEvent e) {
        SplitScreen();
        StartCoroutine(SceneUtility.LoadGame(round));
        EventBus.Publish<ResourceChangeEvent>(new ResourceChangeEvent(1, 1000));
        EventBus.Publish<ResourceChangeEvent>(new ResourceChangeEvent(2, 1000));
        EventBus.Publish<SpeedChangeEvent>(new SpeedChangeEvent(1, 50));
        EventBus.Publish<SpeedChangeEvent>(new SpeedChangeEvent(2, 50));
        finished = false;
        chest_converted[1] = chest_converted[2] = false;
        goal_point[1] = goal_point[2] = 0;
        isTutorial = false;
    }

    public void StartTutorial(TutorialEvent e) {
        SplitScreen();
        StartCoroutine(SceneUtility.LoadTutorial());
        EventBus.Publish<ResourceChangeEvent>(new ResourceChangeEvent(1, 1000));
        EventBus.Publish<ResourceChangeEvent>(new ResourceChangeEvent(2, 1000));
        EventBus.Publish<SpeedChangeEvent>(new SpeedChangeEvent(1, 50));
        EventBus.Publish<SpeedChangeEvent>(new SpeedChangeEvent(2, 50));
        finished = false;
        chest_converted[1] = chest_converted[2] = false;
        goal_point[1] = goal_point[2] = 0;
        isTutorial = true;
    }

    void ResChangeHandler(ResourceChangeEvent e) {
        resources[e.PlayerID] = e.resource;
    }

    void GoalPointHandler(GoalPointEvent e) {
        if (analytics)
        {
            AnalyticsEvent.Custom("GoalPoint", 
                new Dictionary<string, object>{ {"Time", Time.time}, {"PlayerIndex", e.playerID}, {"GoalPointIndex", e.index} }
            );
        }
        ++goal_point[e.playerID];
    }

    void SplitScreen()
    {
        cam1.transform.position = new Vector3(-4.45f, 0f, -10f);
        cam2.transform.position = new Vector3(4.45f, 0f, -10f);
        cam1.rect = new Rect(0f,0f,0.5f,1f);
        cam2.rect = new Rect(0.5f,0f,0.5f,1f);
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject player in players){
            Debug.Log(player.name);
            if (player.name == "Player(Clone)") {
                if (cam1.GetComponent<CameraFollowPlayer>().player == null)
                    cam1.GetComponent<CameraFollowPlayer>().player = player;
                else if (cam2.GetComponent<CameraFollowPlayer>().player == null)
                    cam2.GetComponent<CameraFollowPlayer>().player = player;
            }
            
        }
    }

    public void OnToggleChanged()
    {
        analytics = !analytics;
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

public class TutorialEvent{}