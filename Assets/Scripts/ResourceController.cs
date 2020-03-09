using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceController : MonoBehaviour
{
    public int PlayerID = 1;
    float resource = 0f;
    Subscription<BuffEvent> buffSubscription;

    // Start is called before the first frame update
    void Start()
    {
        buffSubscription = EventBus.Subscribe<BuffEvent>(_OnBuffUpdated);
    }

    // Update is called once per frame
    void Update()
    {
        EventBus.Publish<ResourceStatusEvent>(new ResourceStatusEvent(resource, PlayerID));
    }

    void _OnBuffUpdated(BuffEvent e)
    {
        if (e.playerIndex == PlayerID)
        {
            switch(e.type)
            {
                case BuffController.buffType.resourceUp:
                    resource += e.resourceNum;
                break;
            }
        }
    }
}

public class ResourceStatusEvent
{
    public float resource = 0f;
    public int playerIndex = 1;

    public ResourceStatusEvent(float _resource, int _playerIndex)
    {
        resource = _resource;
        playerIndex = _playerIndex;
    }
    
}
