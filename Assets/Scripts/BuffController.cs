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
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        // respond when get touch to branch
        if(collider.gameObject.CompareTag("Branch"))
        {
            // publish current buff type event to player
            
            // TODO: get player index here
            EventBus.Publish<BuffEvent>(new BuffEvent(type, playerIndex, Time.time));
            this.gameObject.SetActive(false);
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

