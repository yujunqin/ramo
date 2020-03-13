using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static bool PrunePressed(int PlayerID) {
        return (PlayerID == 1 && Input.GetKey(KeyCode.Q)) || (PlayerID == 2 && Input.GetKey(KeyCode.Slash));
    }

    public static bool GrowPressed(int PlayerID) {
        return (PlayerID == 1 && Input.GetKey(KeyCode.E)) || (PlayerID == 2 && Input.GetKey(KeyCode.Period));
    }
}
