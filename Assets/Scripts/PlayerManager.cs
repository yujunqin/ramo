using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerManager : MonoBehaviour
{
    // Start is called before the first frame update
    static public bool first = true;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void JoinKeyboardPlayer()
    {
        if (first)
        {
            GetComponent<PlayerInputManager>().JoinPlayer(-1, -1, "Keyboard Left", Keyboard.current);
            first = false;
            return;
        }
        GetComponent<PlayerInputManager>().JoinPlayer(-1, -1, "Keyboard Right", Keyboard.current);
    }
}
