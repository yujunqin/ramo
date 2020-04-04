using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowPlayer : MonoBehaviour
{
    public GameObject player = null;

    // Start is called before the first frame update
    void Start()
    {
         
    }

    // Update is called once per frame
    void Update()
    {
        if (player) {
            if (player.transform.position.y > 0) {
                transform.position = new Vector3(transform.position.x, player.transform.position.y, transform.position.z);
            }
            else 
                transform.position = new Vector3(transform.position.x, 0.0f, transform.position.z);
        }
    }
}
