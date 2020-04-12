using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialPool : MonoBehaviour
{
    public GameObject buffPrefab, shieldPrefab, checkpointPrefab;  
    Subscription<PlayerProgressEvent> sub;
    public int PlayerID;
    bool opponent_done = false;
    void Start()
    {
        sub = EventBus.Subscribe<PlayerProgressEvent>(ObjectiveGenerator);
    }

    void ObjectiveGenerator(PlayerProgressEvent e) {
        if (e.PlayerID == PlayerID) {
            if (e.progress == "first grow") {
                GameObject buff = Instantiate(buffPrefab, new Vector3(1f, 3f, 0) + transform.position, Quaternion.identity);
                buff.GetComponent<BuffController>().playerIndex = PlayerID;
            }
            if (e.progress == "first bomb") {
                GameObject buff = Instantiate(buffPrefab, new Vector3(-1f, 4f, 0) + transform.position, Quaternion.identity);
                buff.GetComponent<BuffController>().playerIndex = PlayerID;
                StartCoroutine(ConvertChest());
            }
            if (e.progress == "first chest") {
                GameObject shield = Instantiate(shieldPrefab, new Vector3(3f, 4f, 0) + transform.position, Quaternion.identity);
                shield.GetComponent<ShieldController>().playerID = PlayerID;
            }
            if (e.progress == "first shield") {
                GameObject checkpoint = Instantiate(checkpointPrefab, new Vector3(-3f, 5f, 0) + transform.position, Quaternion.identity);
                checkpoint.GetComponent<GoalPointController>().playerID = PlayerID;
            }
            if (e.progress == "first checkpoint" && opponent_done) {
                EventBus.Publish<TutorialEndEvent>(new TutorialEndEvent());
            }
        } else {
            if (e.progress == "first checkpoint") {
                opponent_done = true;
            }
        }
    }

    IEnumerator ConvertChest() {
        yield return null;
        EventBus.Publish<ChestConvertEvent>(new ChestConvertEvent(PlayerID));
    }


}
