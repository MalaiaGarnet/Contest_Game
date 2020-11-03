using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GUI_Widget_Battery : GUI_Widget_Base
{
    public Text m_Text;
    int battery = 0;

    void Update()
    {
        if (m_Player == null)
            return;

        battery = m_Player.m_MyProfile.Battery;
        m_Text.text = "";
        for (int i = 0; i < 10000; i += 500)
            m_Text.text += i <= battery ? "■" : "□";
    }
}
