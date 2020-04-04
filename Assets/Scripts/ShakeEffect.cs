using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShakeEffect : MonoBehaviour
{
    Vector3 init_scale, vel;
    public float k = 0.3f, damp = 0.97f;
    private void Start() {
        init_scale = transform.localScale;
        vel = Vector3.zero;
    }

    private void Update() {
        Vector3 acc = -k * (transform.localScale - init_scale);
        vel += acc;
        vel *= damp;
        transform.localScale += vel;
    }

    public void Shake() {
        transform.localScale = 2f * transform.localScale;
    }


}
