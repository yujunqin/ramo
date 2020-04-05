using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxEffect : MonoBehaviour
{
    public float ParallaxRate = 0;
    public int CameraID = 0;
    public Camera cam = null;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (!cam) {
            if (Camera.allCamerasCount > CameraID)
                cam = Camera.allCameras[CameraID];
        }
        else {
            transform.localPosition = new Vector3(0f, -ParallaxRate*cam.transform.position.y, 0f);
        }
    }
}
