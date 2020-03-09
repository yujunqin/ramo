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
    public int playerIndex = 1;
    bool touched = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // respond when get touch to branch
        if(other.gameObject.CompareTag("Branch") && !touched)
        {
            // publish current buff type event to player
            touched = true;
            EventBus.Publish<BuffEvent>(new BuffEvent(type, playerIndex, Time.time));
            gameObject.SetActive(false);
        }
    }
}

public class BuffEvent
{
    public BuffController.buffType type;
    public int playerIndex;
    public float effectiveTime;
    public float duration = 10f;
    public float resourceNum = 10f;
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

