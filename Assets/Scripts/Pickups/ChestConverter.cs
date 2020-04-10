using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestConverter : MonoBehaviour
{
    // Start is called before the first frame update
    bool _isChest = false;
    Subscription<ChestConvertEvent> sub;
    void Start()
    {
        sub = EventBus.Subscribe<ChestConvertEvent>(ChestConvertEventHandler);
    }

    void ChestConvertEventHandler(ChestConvertEvent e) {
        if (!_isChest && e.PlayerID == GetComponent<BuffController>().playerIndex) {
            Convert();
        }
    }

    void Convert() {
        GetComponent<SpriteRenderer>().sprite = Resources.LoadAll<Sprite>("Images/treasure_chests_32x32")[5];
        transform.rotation = Quaternion.identity;
        transform.localScale = new Vector3(2f, 2f, 1f);
        GetComponent<BoxCollider>().size = new Vector3(0.25f, 0.25f, 1f);
        _isChest = true;
    }

    public bool isChest() {
        return _isChest;
    }
}

class FreeBombEvent {
    public int PlayerID;
    public bool isGet;
    public FreeBombEvent(int id, bool isget) {
        PlayerID = id;
        isGet = isget;
    }
}

class ChestConvertEvent {
    public int PlayerID;
    public ChestConvertEvent(int id) {
        PlayerID = id;
    }
}