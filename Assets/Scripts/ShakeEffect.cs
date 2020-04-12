using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShakeEffect : MonoBehaviour
{
    Vector3 init_scale, vel;
    public float k = 0.3f, damp = 0.97f, amplitude = 1f;
    public bool shakeFromSpawn = false;
    private void Start() {
        init_scale = transform.localScale;
        vel = Vector3.zero;
        if (shakeFromSpawn) {
            Shake();
        }
    }

    private void Update() {
        Vector3 acc = -k * (transform.localScale - init_scale);
        vel += acc;
        vel *= damp;
        transform.localScale += vel;
    }

    public void Shake() {
        transform.localScale = (amplitude + 1f) * init_scale;
    }


}
