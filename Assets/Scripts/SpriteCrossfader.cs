using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteCrossfader : MonoBehaviour
{
    public List<Color> palette;
    Subscription<NewRoundEvent> subscription;

    // Start is called before the first frame update
    void Start()
    {
        subscription = EventBus.Subscribe<NewRoundEvent>(NewRound);
        GetComponent<SpriteRenderer>().color = palette[0];
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void NewRound(NewRoundEvent nr) {
        StartCoroutine(Crossfade(nr.round, 3.0f));
    }

    IEnumerator Crossfade(int round, float total) {
        if (round == 1)
        {
            yield return null;
        }
        else
        {
            for (float t = 0.0f; t < 1.0f; t += Time.deltaTime / total)
            {
                GetComponent<SpriteRenderer>().color = Color.Lerp(palette[round - 2], palette[round - 1], t);
                Debug.Log("crossfading: " + round.ToString() + " " + t.ToString());
                yield return null;
            }
        }
    }
}
