using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// GUI 중 레이더 위젯
/// </summary>
public class GUI_Widget_Radar : GUI_Widget_Base
{
    public RectTransform m_Circle;
    public List<RectTransform> m_Markers = new List<RectTransform>();

    private void Update()
    {
        if (m_Player == null)
            return;

        float angle = m_Player.m_MyProfile.User_Input.View_Y;
        m_Circle.localRotation = Quaternion.Slerp(m_Circle.localRotation, Quaternion.Euler(0f, 0f, angle), 0.1f);
    }
}
