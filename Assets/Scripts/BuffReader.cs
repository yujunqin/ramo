using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuffReader : MonoBehaviour
{
    public int PlayerID = 1;
    Subscription<BuffStatusEvent> buffStatusSubscription;
    Text buffStatusText;
    // Start is called before the first frame update
    void Start()
    {
        buffStatusSubscription = EventBus.Subscribe<BuffStatusEvent>(_OnBuffStatusUpdated);
        buffStatusText = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void _OnBuffStatusUpdated(BuffStatusEvent e)
    {
        if (PlayerID == e.playerIndex)
        {
            buffStatusText.text = e.buffText;
        }
    }
}
