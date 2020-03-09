using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResourceController : MonoBehaviour
{
    public Text woodQuantityText;
    public float resourceNum = 0f;
    public int PlayerID = 1;
    public float resource = 0f;
    Subscription<BuffEvent> buffSubscription;

    // Start is called before the first frame update
    void Start()
    {
        buffSubscription = EventBus.Subscribe<BuffEvent>(_OnBuffUpdated);
    }

    // Update is called once per frame
    void Update()
    {
        woodQuantityText.text = "X " + resource.ToString();
    }

    void _OnBuffUpdated(BuffEvent e)
    {
        if (e.playerIndex == PlayerID)
        {
            switch(e.type)
            {
                case BuffController.buffType.resourceUp:
                    resourceNum = e.resourceNum;
                    resource += e.resourceNum;
                break;
            }
        }
    }
}
