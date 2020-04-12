using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class GrowthBar : MonoBehaviour
{
    float targetHeight = 300f;
    public float fillSpeed = 0.5f;
    public int playerID;
    public int percentageValue;
    Subscription<HeightChangeEvent> height_sub;
    Slider slider;
    Text valueText;
    ParticleSystem particleSys;
    float targetProgress = 0;
    // Start is called before the first frame update
    void Start()
    {
        height_sub = EventBus.Subscribe<HeightChangeEvent>(heightUpdate);
        slider = gameObject.GetComponent<Slider>();
        particleSys = gameObject.transform.GetChild(1).transform.GetChild(0).transform.GetChild(0).GetComponent<ParticleSystem>();
        valueText = gameObject.transform.GetChild(2).GetComponent<Text>();
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
        else if (slider.value > targetProgress)
        {
            slider.value -= fillSpeed * Time.deltaTime;
            if (!particleSys.isPlaying)
            {
                particleSys.Play();
            }
        }
        else
        {
            particleSys.Stop();
        }
        percentageValue = Mathf.RoundToInt(slider.value * 100);
        valueText.text = percentageValue.ToString() + "%";
    }

    void heightUpdate(HeightChangeEvent h)
    {
        if (h.PlayerID != playerID)
        {
            return;
        }
        else
        {
            targetProgress = h.height / targetHeight;
        }
    }
}
