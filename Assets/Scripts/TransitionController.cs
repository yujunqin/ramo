using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransitionController : MonoBehaviour
{
    public AnimationCurve curve;
    public static float duration = 2f;
    public float max_scale = 20f;
    Vector3 init_scale;
    bool in_transition = false;
    Subscription<SceneTransitionEvent> sub;
    // Start is called before the first frame update
    void Start()
    {
        sub = EventBus.Subscribe<SceneTransitionEvent>(StartTransition);
        init_scale = transform.localScale.normalized;
        transform.localScale = Vector3.zero;
        Debug.Log(init_scale.ToString());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartTransition(SceneTransitionEvent e) {
        if (in_transition) return;
        StartCoroutine(Transit());
    }

    IEnumerator Transit() {
        in_transition = true;
        float initial_time = Time.time;

        float progress = (Time.time - initial_time) / duration;
        while(progress < 1.0f)
        {
            progress = (Time.time - initial_time) / duration;
            transform.localScale = init_scale * curve.Evaluate(progress) * max_scale;
            yield return null;
        }
        in_transition = false;
    }



}

public class SceneTransitionEvent{}
