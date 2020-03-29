using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResourceReaderBomb : MonoBehaviour
{
    public int PlayerID = 1;
    public float cost = 1000f;
    Subscription<ResourceChangeEvent> resourceStatusSubscription;
    Subscription<FreeBombEvent> fbs;
    Image panelImage;
    Image bombImage; 
    Image progressImage;
    Text[] texts;
    int free_bomb = 0;
    public int resource = 0;
    // Start is called before the first frame update
    void Start()
    {
        resourceStatusSubscription = EventBus.Subscribe<ResourceChangeEvent>(_OnResourceStatusUpdated);
        fbs = EventBus.Subscribe<FreeBombEvent>(FreeBombEventHandler);
        panelImage = GetComponent<Image>();
        bombImage = transform.Find("BombImage").GetComponent<Image>();
        progressImage = transform.Find("Progress").GetComponent<Image>();
        texts = GetComponentsInChildren<Text>();
    }

    void _OnResourceStatusUpdated(ResourceChangeEvent e)
    {
        if (PlayerID == e.PlayerID)
        {
            resource = e.resource;
        }

        if (free_bomb == 0) {
            cost = 1000;
        } else {
            cost = 0;
        }

        if (resource >= cost) {
            panelImage.color = new Color(1.0f, 1.0f, 1.0f, 0.4f);
            bombImage.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
            progressImage.fillAmount = 1.0f;
            foreach (Text text in texts){
                text.color = new Color(text.color.r, text.color.g, text.color.b, 1.0f);
                if (text.name == "Cost") {
                    text.text = cost.ToString();
                }
            }
        }
        else {
            panelImage.color = new Color(1.0f, 1.0f, 1.0f, 0.2f);
            bombImage.color = new Color(1.0f, 1.0f, 1.0f, 0.4f);
            progressImage.fillAmount = (cost == 0) ? 1 : resource / cost;
            foreach (Text text in texts){
                text.color = new Color(text.color.r, text.color.g, text.color.b, 0.4f);
                if (text.name == "Cost") {
                    text.text = cost.ToString();
                }
            }
        }
    }

    void FreeBombEventHandler(FreeBombEvent e) {
        if (e.PlayerID == PlayerID) {
            if (e.isGet) {
                ++free_bomb;
            } else {
                --free_bomb;
            }
        }
        _OnResourceStatusUpdated(new ResourceChangeEvent(PlayerID, resource));
    }

}
