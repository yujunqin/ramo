using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerJoinBoxController : MonoBehaviour
{
    static public int playersJoined = 0;
    public int PlayerID;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (playersJoined >= 2)
        {
            playersJoined = 0;
            var rootObjects = SceneManager.GetActiveScene().GetRootGameObjects();
            foreach (var obj in rootObjects)
            {
                if (!obj.CompareTag("Player") && obj.name != "Main Camera" && obj.name != "GameMaster")
                {
                    Destroy(obj);
                }
            }
            EventBus.Publish<GameStartEvent>(new GameStartEvent());
            //SceneManager.LoadScene(1, LoadSceneMode.Additive);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && other.GetComponent<PlayerMovement>().PlayerID == PlayerID){
            playersJoined += 1;
            Debug.Log("playersJoined: " + playersJoined.ToString());
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && other.GetComponent<PlayerMovement>().PlayerID == PlayerID){
            playersJoined -= 1;
            Debug.Log("playersJoined: " + playersJoined.ToString());
        }
    }

}
