using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneUtility : MonoBehaviour
{
    public static IEnumerator LoadSceneAsync(int SceneID) {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(SceneID, LoadSceneMode.Additive);

        while (!asyncLoad.isDone) {
            yield return null;
        }
    }

    public static IEnumerator UnloadSceneAsync(int SceneID) {
        AsyncOperation asyncUnload = SceneManager.UnloadSceneAsync(SceneID);

        while (!asyncUnload.isDone) {
            yield return null;
        }
    }

    public static IEnumerator UnloadAll() {
        var rootObjects = SceneManager.GetActiveScene().GetRootGameObjects();
        foreach (var obj in rootObjects)
        {
            if (!obj.CompareTag("Player") && obj.name != "Main Camera" && obj.name != "GameMaster")
            {
                Destroy(obj);
            }
        }
        for (int i = 1; i < SceneManager.sceneCountInBuildSettings; ++i) {
            yield return SceneUtility.UnloadSceneAsync(i);
        }
    }

    public static IEnumerator LoadAll(int round) {
        for (int i = 1; i < SceneManager.sceneCountInBuildSettings; ++i) {
            yield return SceneUtility.LoadSceneAsync(i);
        }
        EventBus.Publish<NewRoundEvent>(new NewRoundEvent(round));
    }
}
