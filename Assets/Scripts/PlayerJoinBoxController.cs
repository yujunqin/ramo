using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerJoinBoxController : MonoBehaviour
{
    static public int playersJoined = 0;
    public int PlayerID;

    private SpriteRenderer sb;
    public Color joinColor;
    private Color normalColor;

    private void Start()
    {
        sb = GetComponent<SpriteRenderer>();
        normalColor = sb.color;
    }

    private void Update()
    {
        if (playersJoined >= 2)
        {
            playersJoined = 0;
            var rootObjects = SceneManager.GetActiveScene().GetRootGameObjects();
            foreach (var obj in rootObjects)
            {
                if (!obj.CompareTag("Player") && !obj.name.Contains("Main Camera") && obj.name != "GameMaster")
                {
                    Destroy(obj);
                }
            }
            EventBus.Publish<GameStartEvent>(new GameStartEvent());
            //SceneManager.LoadScene(1, LoadSceneMode.Additive);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && other.GetComponent<PlayerMovement>().PlayerID == PlayerID){
            playersJoined += 1;
            Debug.Log("playersJoined: " + playersJoined.ToString());

            AudioClip clip = Resources.Load<AudioClip>("Sound Effects/Grow");
            AudioSource.PlayClipAtPoint(clip, Camera.main.transform.position, 0.4f);
            sb.color = joinColor;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && other.GetComponent<PlayerMovement>().PlayerID == PlayerID){
            playersJoined -= 1;
            Debug.Log("playersJoined: " + playersJoined.ToString());

            AudioClip clip = Resources.Load<AudioClip>("Sound Effects/GrowError");
            AudioSource.PlayClipAtPoint(clip, Camera.main.transform.position, 0.4f);
            sb.color = normalColor;
        }
    }
}
