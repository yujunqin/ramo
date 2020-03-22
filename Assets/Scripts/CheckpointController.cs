using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointController : MonoBehaviour
{
    bool isChecked;
    public int playerID;
    public Sprite flower;
    float checkedTime;
    public AudioClip checkpointClip;

    Transform trans;
    SpriteRenderer m_SpriteRenderer;
    // Start is called before the first frame update
    void Start()
    {
        isChecked = false;

        trans = GetComponent<Transform>();
        m_SpriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isChecked)
        {
            if (Time.time < checkedTime + 2f)
            {
                float scale = 0.4f - (Time.time - checkedTime) / 5f;
                trans.localScale = new Vector3(scale, scale, 1f);
            }
            else
            {
                m_SpriteRenderer.sprite = flower;
                if (Time.time < checkedTime + 4f)
                {
                    float scale = 0.2f * (Time.time - checkedTime - 2f);
                    trans.localScale = new Vector3(scale, scale, 1f);
                }
                
            }
        }
    }

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.CompareTag("Branch") && !isChecked)
        {
            EventBus.Publish<CheckPointEvent>(new CheckPointEvent(Time.time, playerID));
            isChecked = true;
            checkedTime = Time.time;
            // gameObject.SetActive(false);
            AudioSource.PlayClipAtPoint(checkpointClip, Camera.main.transform.position);
        }
    }
}

class CheckPointEvent{
    public float checkedTime;
    public int playerID;
    public CheckPointEvent(float _checkedTime, int _playerID)
    {
        checkedTime = _checkedTime;
        playerID = _playerID;
    }
}
