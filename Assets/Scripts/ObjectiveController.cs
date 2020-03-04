using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectiveController : MonoBehaviour
{
    public GameController gameController;
    public float cycle = 2.0f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // var time = Time.time;
        // var t = Mathf.Repeat(Time.time / cycle, 1.0f);
        // var colorA = new Color(0.0f, 1.0f, 0.0f, 1.0f);
        // var colorB = new Color(0.0f, 1.0f, 0.0f, 0.5f);
        // var color = Color.Lerp(colorA, colorB, t);
        // GetComponent<SpriteRenderer>().color = color;
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        Debug.Log("Collision");
        gameController.Win();
    }

    void OnTriggerStay2D(Collider2D col)
    {
        Debug.Log("Collision");
        gameController.Win();
    }
}
