using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldController : MonoBehaviour
{
    bool isChecked;
    public int playerID;
    public int shieldID;
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
            GetComponent<Collider>().enabled = false;
            // if (Time.time < checkedTime + 2f)
            // {
            //     float scale = 0.4f - (Time.time - checkedTime) / 5f;
            //     trans.localScale = new Vector3(scale, scale, 1f);
            // }
            // else
            // {
            //     m_SpriteRenderer.sprite = flower;
            //     if (Time.time < checkedTime + 4f)
            //     {
            //         float scale = 0.2f * (Time.time - checkedTime - 2f);
            //         trans.localScale = new Vector3(scale, scale, 1f);
            //     }
            // }
        }
    }

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.CompareTag("Branch") && !isChecked)
        {
            EventBus.Publish<ShieldEvent>(new ShieldEvent(Time.time, playerID, shieldID));
            GetComponent<ShakeEffect>().enabled = false;
            isChecked = true;
            checkedTime = Time.time;
            // gameObject.SetActive(false);
            AudioSource.PlayClipAtPoint(checkpointClip, Camera.main.transform.position);
            StartCoroutine(ShieldDisappear(0.5f));
        }
    }

    IEnumerator ShieldDisappear(float duration_sec)
    {
        transform.GetChild(0).gameObject.SetActive(true);
        float initial_time = Time.time;

        float progress = (Time.time - initial_time) / duration_sec;
        Vector3 dest_scale = Vector3.zero;
        while(progress < 1.0f)
        {
            progress = (Time.time - initial_time) / duration_sec;
            Vector3 new_scale = Vector3.Lerp(transform.localScale, dest_scale, 0.05f);
            transform.localScale = new_scale;
            yield return null;
        }
        m_SpriteRenderer.enabled = false;
    }
}

class ShieldEvent{
    public float checkedTime;
    public int playerID;
    public int shieldID;
    public ShieldEvent(float _checkedTime, int _playerID, int _shieldID)
    {
        checkedTime = _checkedTime;
        playerID = _playerID;
        shieldID = _shieldID;
    }
}
