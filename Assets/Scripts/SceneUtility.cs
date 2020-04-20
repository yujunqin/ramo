using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneUtility : MonoBehaviour
{
    public static readonly int[] GameScenes = {1, 2}, TutorialScenes = {3};
    public static readonly int background = 4;
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
        EventBus.Publish<SceneTransitionEvent>(new SceneTransitionEvent());
        yield return new WaitForSeconds(TransitionController.duration / 2);
        var rootObjects = SceneManager.GetActiveScene().GetRootGameObjects();
        foreach (var obj in rootObjects)
        {
            if (!obj.CompareTag("Player") && !obj.name.Contains("Main Camera") && obj.name != "GameMaster" && !obj.name.Contains("Transition"))
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
        yield return SceneUtility.LoadSceneAsync(background);
        EventBus.Publish<TransitionEvent>(new TransitionEvent(round)); // color change and set position
        yield return new WaitForSeconds(TransitionController.duration / 2); // wait transition to end
        yield return new WaitForSeconds(3.5f - TransitionController.duration / 2); //color change end
        EventBus.Publish<SceneTransitionEvent>(new SceneTransitionEvent()); // Scene Trans
        yield return new WaitForSeconds(TransitionController.duration / 2);
        foreach (int i in GameScenes) {
            yield return SceneUtility.LoadSceneAsync(i);
        }
        EventBus.Publish<NewRoundEvent>(new NewRoundEvent(round));
        yield return new WaitForSeconds(TransitionController.duration / 2); // Scene Trans End
    }

    public static IEnumerator LoadTutorial() {
        EventBus.Publish<SceneTransitionEvent>(new SceneTransitionEvent()); // Scene Trans
        yield return new WaitForSeconds(TransitionController.duration / 2);
        var rootObjects = SceneManager.GetActiveScene().GetRootGameObjects();
        foreach (var obj in rootObjects)
        {
            if (!obj.CompareTag("Player") && !obj.name.Contains("Main Camera") && obj.name != "GameMaster" && !obj.name.Contains("Transition"))
            {
                Destroy(obj);
            }
        }
        foreach (int i in TutorialScenes) {
            yield return SceneUtility.LoadSceneAsync(i);
        }
        yield return new WaitForSeconds(TransitionController.duration / 2);
    }
}
