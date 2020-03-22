using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeightScanner : MonoBehaviour
{
    Rigidbody rb;
    public int PlayerID;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        Scan();
        ReportHeight();
    }

    void Scan() {
        Vector3 left = transform.position;
        left.x -= transform.localScale.x / 2f;
        RaycastHit[] hits = Physics.RaycastAll(left, new Vector3(transform.localScale.x, 0, 0), transform.localScale.x, 1 << 8);
        if (hits.Length > 0) {
            rb.AddForce(new Vector3(0, 5f, 0));
        } else {
            left.y -= 0.05f;
            RaycastHit[] lower_hits = Physics.RaycastAll(left, new Vector3(transform.localScale.x, 0, 0), transform.localScale.x, 1 << 8);
            if (lower_hits.Length > 0) {
                rb.velocity = Vector3.zero;
            } else {
                rb.AddForce(new Vector3(0, -5f, 0));
            }
        }
    }

    void ReportHeight() {
        EventBus.Publish<HeightChangeEvent>(new HeightChangeEvent(PlayerID, TransformToHeight(transform)));
    }
    
    float TransformToHeight(Transform t) {
        float h = Mathf.Floor(13.253f * t.position.y + 54.217f);
        return (h > 0) ? h : 0;
    }
}

class HeightChangeEvent {
    public float height;
    public int PlayerID;
    public HeightChangeEvent(int id, float h) {
        height = h;
        PlayerID = id;
    }
}