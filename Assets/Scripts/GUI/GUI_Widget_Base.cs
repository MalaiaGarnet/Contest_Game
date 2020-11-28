using System.Collections;
using UnityEngine;

/// <summary>
/// GUI 위젯 베이스 객체
/// </summary>
public class GUI_Widget_Base : MonoBehaviour
{
    protected CharacterController m_Player;
    protected Ingame_UI m_UI;

    IEnumerator Start()
    {
        while (Ingame_UI.Instance == null)
            yield return new WaitForEndOfFrame();
        m_UI = Ingame_UI.Instance;
        m_UI.e_Initialize.AddListener(When_UI_Initialized);

        yield return null;
    }

    protected virtual void When_UI_Initialized()
    {
        m_Player = m_UI.m_Player;
    }
}