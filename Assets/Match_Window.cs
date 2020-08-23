using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Match_Window : Small_Window
{
    public Text m_Title;
    public Text m_Desc;

    public void Start_Match_Security()
    {
        m_Title.text = "security";
        m_Desc.text = "matching";
        gameObject.SetActive(true);
    }
    public void Start_Match_Theft()
    {
        m_Title.text = "theft";
        m_Desc.text = "matching";
        gameObject.SetActive(true);
    } 
}
