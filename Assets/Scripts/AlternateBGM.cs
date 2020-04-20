using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlternateBGM : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        if (Random.Range(0, 10) < 2)
        {
            UseAlternateBGM();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void UseAlternateBGM()
    {
        // by Morgan Elder
        GetComponent<AudioSource>().clip = Resources.Load<AudioClip>("Audio/raum title");
        GetComponent<AudioSource>().Play();
    }
}
