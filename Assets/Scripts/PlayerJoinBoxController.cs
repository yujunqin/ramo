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

    bool skipTutorial = false;

    private void Start()
    {
        sb = GetComponent<SpriteRenderer>();
        normalColor = sb.color;
        StartCoroutine(PulseSprite());
    }

    private void Update()
    {
        if (playersJoined >= 2)
        {
            playersJoined = 0;
            if (skipTutorial) {
                EventBus.Publish<GameStartEvent>(new GameStartEvent());
            } else {
                EventBus.Publish<TutorialEvent>(new TutorialEvent());
            }
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

    public void OnSkipTutorial(bool skip) {
        skipTutorial = skip;
    }

    private IEnumerator PulseSprite()
    {
        while (true)
        {
            for (float i = 1.0f; i < 1.2f; i += 0.01f)
            {
                transform.localScale = new Vector3(i, i, 0);
                yield return new WaitForSeconds(0.01f);
            }

            for (float i = 1.2f; i >= 1.0f; i -= 0.01f)
            {
                transform.localScale = new Vector3(i, i, 0);
                yield return new WaitForSeconds(0.01f);
            }
        }
    }
}
