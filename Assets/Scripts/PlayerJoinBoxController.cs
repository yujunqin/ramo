using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerJoinBoxController : MonoBehaviour
{
    static public int playersJoined = 0;
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
                if (!obj.CompareTag("Player") && obj.name != "AudioListener")
                {
                    Destroy(obj);
                }
            }
            StartCoroutine(LoadSceneAsync(1));
            //SceneManager.LoadScene(1, LoadSceneMode.Additive);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        playersJoined += 1;
        Debug.Log("playersJoined: " + playersJoined.ToString());
    }

    void OnTriggerExit(Collider other)
    {
        playersJoined -= 1;
        Debug.Log("playersJoined: " + playersJoined.ToString());
    }

    IEnumerator LoadSceneAsync(int SceneID) {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(SceneID, LoadSceneMode.Additive);

        while (!asyncLoad.isDone) {
            yield return null;
        }
    }
}
