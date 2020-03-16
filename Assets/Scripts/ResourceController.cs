using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceController : MonoBehaviour
{
    public int PlayerID;
    int resource = 0;
    public int NaturalGrowth = 100;
    Subscription<BuffEvent> buffSubscription;
    Subscription<ResourceChangeEvent> resSub;

    // Start is called before the first frame update
    void Start()
    {
        PlayerID = GetComponent<PlayerMovement>().PlayerID;
        buffSubscription = EventBus.Subscribe<BuffEvent>(_OnBuffUpdated);
        resSub = EventBus.Subscribe<ResourceChangeEvent>(ResourceChangeHandler);
        EventBus.Publish<ResourceChangeEvent>(new ResourceChangeEvent(PlayerID, 1000));
        StartCoroutine(AutoGenerate());
    }

    // Update is called once per frame
    void Update()
    {
        //EventBus.Publish<ResourceStatusEvent>(new ResourceStatusEvent(resource, PlayerID));
    }

    void _OnBuffUpdated(BuffEvent e)
    {
        if (e.playerIndex == PlayerID)
        {
            switch(e.type)
            {
                case BuffController.buffType.resourceUp:
                    resource += e.resourceNum;
                    EventBus.Publish<ResourceChangeEvent>(new ResourceChangeEvent(PlayerID, resource));
                break;
                case BuffController.buffType.speedUp:
                    NaturalGrowth += e.SpeedUP;
                    EventBus.Publish<SpeedChangeEvent>(new SpeedChangeEvent(PlayerID, NaturalGrowth));
                break;
            }
        }
    }

    void ResourceChangeHandler(ResourceChangeEvent rc) {
        if (rc.PlayerID == PlayerID) {
            resource = rc.resource;
        }
    }

    IEnumerator AutoGenerate() {
        while (true) {
            yield return new WaitForSeconds(1f);
            resource += NaturalGrowth;
            EventBus.Publish<ResourceChangeEvent>(new ResourceChangeEvent(PlayerID, resource));
        }
    }
}

class ResourceChangeEvent {
    public int PlayerID;
    public int resource;
    public ResourceChangeEvent(int id, int res) {
        PlayerID = id;
        resource = res;
    }
}

class SpeedChangeEvent {
    public int PlayerID;
    public int speed;
    public SpeedChangeEvent(int id, int spd) {
        PlayerID = id;
        speed = spd;
    }
}

