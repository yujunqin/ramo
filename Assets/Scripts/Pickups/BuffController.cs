using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffController : MonoBehaviour
{
    public enum buffType
    {   
        speedUp,
        resourceUp,
        speedDown
    }

    public buffType type;
    public int playerIndex;
    Vector2 uiResourcePos, uiBombPos; // the ui position where the buff goes
    bool touched = false;
    // Start is called before the first frame update
    void Start()
    {
        if (playerIndex == 1) {
            uiResourcePos = GameObject.Find("Canvas/Panel1/Wood").transform.position;
            uiBombPos = GameObject.Find("Canvas/Panel1/BombPanel").transform.position;
        }
        else {
            uiResourcePos = GameObject.Find("Canvas/Panel2/Wood").transform.position;
            uiBombPos = GameObject.Find("Canvas/Panel2/BombPanel").transform.position;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    private void OnTriggerEnter(Collider collider)
    {
        Debug.Log("collision");
        Debug.Log(collider.tag);
        // respond when get touch to branch
        if(collider.gameObject.CompareTag("Branch") && !touched)
        {
            // publish current buff type event to player
            touched = true;
            EventBus.Publish<BuffEvent>(new BuffEvent(type, playerIndex, Time.time));
            if (GetComponent<ChestConverter>().isChest()) {
                EventBus.Publish<FreeBombEvent>(new FreeBombEvent(playerIndex, true));
                GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Images/wood");
                transform.localScale = new Vector3(1f, 1f, 1.0f);
                GameObject bomb = Instantiate(gameObject, transform.position, Quaternion.identity);
                bomb.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Images/bomb");
                bomb.transform.localScale = new Vector3(1f, 1f, 1.0f);
                StartCoroutine(BuffJuicyDisappear(bomb.transform, uiBombPos, 0.5f));
            }
            StartCoroutine(BuffJuicyDisappear(transform, uiResourcePos, 0.5f));
        }
    }

    IEnumerator BuffJuicyDisappear(Transform trans, Vector2 final_uipos, float duration_sec)
    {
        float initial_time = Time.time;

        float progress = (Time.time - initial_time) / duration_sec;
        Vector3 dest_scale = Vector3.zero;
        Vector3 dest_pos = Vector3.zero;
        while(progress < 1.0f)
        {
            progress = (Time.time - initial_time) / duration_sec;
            dest_pos = Camera.allCameras[playerIndex-1].ScreenToWorldPoint(new Vector3(final_uipos.x, final_uipos.y, 10.0f));
            Vector3 new_position = Vector3.Lerp(trans.position, dest_pos, 0.05f);
            Vector3 new_scale = Vector3.Lerp(trans.localScale, dest_scale, 0.05f);
            trans.position = new_position;
            trans.localScale = new_scale;

            yield return null;
        }
        trans.gameObject.SetActive(false);
    }
}

public class BuffEvent
{
    public BuffController.buffType type;
    public int playerIndex;
    public float effectiveTime;
    public float duration = 10f;
    public int resourceNum = 500;
    public int SpeedUP = 50;
    public BuffEvent(BuffController.buffType _type, int _playerIndex, float _effectiveTime)
    { 
        type = _type;
        playerIndex = _playerIndex;
        effectiveTime = _effectiveTime;
    }
}

public class BuffStatusEvent
{
    public string buffText = "";
    public int playerIndex;
    public BuffStatusEvent(string _buffText, int _playerIndex)
    { 
        playerIndex = _playerIndex;
        buffText = _buffText;
    }
}

