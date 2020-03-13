using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResourceReader : MonoBehaviour
{
    public int PlayerID = 1;
    Subscription<ResourceChangeEvent> resourceStatusSubscription;
    Text woodQuantityText; 
    float resource = 0f;
    // Start is called before the first frame update
    void Start()
    {
        resourceStatusSubscription = EventBus.Subscribe<ResourceChangeEvent>(_OnResourceStatusUpdated);
        woodQuantityText = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        woodQuantityText.text = "X " + resource.ToString();
    }

    void _OnResourceStatusUpdated(ResourceChangeEvent e)
    {
        if (PlayerID == e.PlayerID)
        {
            resource = e.resource;
        }
    }
}
