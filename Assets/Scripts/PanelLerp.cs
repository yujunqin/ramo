using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelLerp : MonoBehaviour
{
    public Vector3 dest;
    Vector3 init_position;
    RectTransform rt;

    private void Start() {
        rt = GetComponent<RectTransform>();
        init_position = rt.localPosition;
        dest = init_position;    
    }

    private void Update() {
        rt.localPosition = Vector3.Lerp(rt.localPosition, dest, 0.05f);
    }

    public void BackToInit() {
        dest = init_position;
    }

    public void Move(Vector3 offset) {
        dest += offset;
    }
}
