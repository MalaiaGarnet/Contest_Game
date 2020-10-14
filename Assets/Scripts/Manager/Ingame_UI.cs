using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;


public class Event_UI_Initialize : UnityEvent { }

public class Ingame_UI : SingleToneMonoBehaviour<Ingame_UI>
{
    [Header("주시할 플레이어")]
    public CharacterController m_Player;

    [Header("곁다리")]
    public GameObject m_Header;
    public GameObject m_Footer;
    public GameObject m_LeftSide;
    public GameObject m_RightSide;

    [Header("센터 오브젝트들")]
    public Scene_Loader m_Ingame_Scene_Loader;
    public Round_Indicator m_Ingame_Round_Indicator;
    public GameObject m_Dead_Indicator;

    public Event_UI_Initialize e_Initialize = new Event_UI_Initialize();

    UnityAction<int> a_When_Damaged;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        a_When_Damaged = new UnityAction<int>(When_Damaged);
    }

    /// <summary>
    /// 플레이어 전환
    /// </summary>
    /// <param name="_cc"></param>
    public void Set_Player(CharacterController _cc)
    {
        // 이전 플레이어 단계에서 해줄 거
        if (m_Player != null)
        {
            m_Player.e_Damaged.RemoveListener(a_When_Damaged);
        }

        // 현재 플레이어 갱신 후 해줄 거
        m_Player = _cc;
        m_Player.e_Damaged.AddListener(a_When_Damaged);
        When_Damaged(0);
        e_Initialize.Invoke();
    }

    public void Show(bool _enable)
    {
        gameObject.SetActive(_enable);
    }

    public void Lock_Cursor(bool _enable)
    {
        Cursor.lockState = _enable ? CursorLockMode.Locked : CursorLockMode.None;
        Cursor.visible = !_enable;
    }

    public void When_Damaged(int _damage)
    {
        m_Dead_Indicator.SetActive(m_Player.m_MyProfile.HP <= 0);
    }
}
