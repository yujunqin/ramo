using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RuleInputFieldController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Append(string ch)
    {
        GetComponent<InputField>().text += ch;
    }

    public void Delete()
    {
        GetComponent<InputField>().text = GetComponent<InputField>().text.Remove(GetComponent<InputField>().text.Length - 1);
    }
}
