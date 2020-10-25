using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class Widget_IP_Reminder : MonoBehaviour
{
    public InputField IP_Input;

    // Start is called before the first frame update
    void Start()
    {
        if (PlayerPrefs.HasKey("Saved_IP"))
            IP_Input.SetTextWithoutNotify(PlayerPrefs.GetString("Saved_IP"));
    }

    public void onTextChange(string _text)
    {
        PlayerPrefs.SetString("Saved_IP", _text);
    }
}
