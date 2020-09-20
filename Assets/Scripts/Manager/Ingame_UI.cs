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

    [Header("센터 오브젝트들")]
    public Scene_Loader m_Ingame_Scene_Loader;

    public Event_UI_Initialize e_Initialize = new Event_UI_Initialize();

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    /// <summary>
    /// 플레이어 전환
    /// </summary>
    /// <param name="_cc"></param>
    public void Set_Player(CharacterController _cc)
    {
        m_Player = _cc;
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
}
