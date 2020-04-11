using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneUtility : MonoBehaviour
{
    public static readonly int[] GameScenes = {1, 2}, TutorialScenes = {3};
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

    public static IEnumerator UnloadAll(bool isTutorial) {
        var rootObjects = SceneManager.GetActiveScene().GetRootGameObjects();
        foreach (var obj in rootObjects)
        {
            if (!obj.CompareTag("Player") && !obj.name.Contains("Main Camera") && obj.name != "GameMaster")
            {
                Destroy(obj);
            }
        }
        if (isTutorial) {
            foreach (int i in TutorialScenes) {
                yield return SceneUtility.UnloadSceneAsync(i);
            }
        } else {
            foreach (int i in GameScenes) {
                yield return SceneUtility.UnloadSceneAsync(i);
            }
        }
    }

    public static IEnumerator LoadGame(int round) {
        foreach (int i in GameScenes) {
            yield return SceneUtility.LoadSceneAsync(i);
        }
        EventBus.Publish<NewRoundEvent>(new NewRoundEvent(round));
    }

    public static IEnumerator LoadTutorial() {
        foreach (int i in TutorialScenes) {
            yield return SceneUtility.LoadSceneAsync(i);
        }
    }
}
