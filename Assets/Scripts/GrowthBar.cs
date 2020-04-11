using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class GrowthBar : MonoBehaviour
{
    public float fillSpeed = 0.5f;
    public int playerID;
    Subscription<HeightChangeEvent> height_sub;
    Slider slider;
    ParticleSystem particleSys;
    float targetProgress = 0;
    // Start is called before the first frame update
    void Start()
    {
        height_sub = EventBus.Subscribe<HeightChangeEvent>(heightUpdate);
        slider = gameObject.GetComponent<Slider>();
        particleSys = gameObject.transform.GetChild(1).transform.GetChild(0).transform.GetChild(0).GetComponent<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        if (slider.value < targetProgress)
        {
            slider.value += fillSpeed * Time.deltaTime;
            if (!particleSys.isPlaying)
            {
                particleSys.Play();
            }
        }
        else
        {
            particleSys.Stop();
        }
    }

    void heightUpdate(HeightChangeEvent h)
    {
        if (h.PlayerID != playerID)
        {
            return;
        }
        else
        {
            targetProgress = h.height / 300f;
        }
    }
}
