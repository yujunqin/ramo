using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelLerp : MonoBehaviour
{
    public Vector3 dest;
    Vector3 init_position;

    private void Start() {
        init_position = transform.position;
        dest = init_position;    
    }

    private void Update() {
        transform.position = Vector3.Lerp(transform.position, dest, 0.05f);
    }

    public void BackToInit() {
        dest = init_position;
    }

    public void Move(Vector3 offset) {
        dest += offset;
    }
}
