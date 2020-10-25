using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Network.Data;

using static UnityEngine.InputSystem.InputAction;
using UnityEngine.InputSystem;

public class Event_Input_Projection : UnityEvent<bool> { }
public class Manager_Input : SingleToneMonoBehaviour<Manager_Input>
{
    #region  필드와 프로퍼티

    const float PITCH_LIMIT = 80f;

    public Event_Input_Projection e_Input_Projection = new Event_Input_Projection();

    [Header("디버그로 확인")]
    public User_Input m_Player_Input;   // 클라이언트 최신 인풋
    public Vector3 m_Pre_Position;      // 예측 위치


    #endregion

    void Start()
    {
    }

    void Update()
    {
        Update_MouseView();
    }

    void Update_MouseView()
    {
        Vector2 view_vec = Mouse.current.delta.ReadValue();
        view_vec = new Vector2(-view_vec.y, view_vec.x);
        view_vec *= 0.1f;
        ChangeView(view_vec);
    }

    /// <summary>
    /// 움직임 갱신
    /// </summary>
    /// <param name="_context"></param>
    public void onMove(CallbackContext _context)
    {
        m_Player_Input.Move_X = _context.ReadValue<Vector2>().x;
        m_Player_Input.Move_Y = _context.ReadValue<Vector2>().y;
    }
    /// <summary>
    /// 시점 갱신
    /// </summary>
    /// <param name="_context"></param>
    public void onView(CallbackContext _context)
    {
        Vector2 view = new Vector2(_context.ReadValue<Vector2>().y, _context.ReadValue<Vector2>().x);
        ChangeView(view);
    }

    /// <summary>
    /// 뷰 좌표 갱신
    /// </summary>
    /// <param name="_view"></param>
    public void ChangeView(Vector2 _view)
    {
        // 위아래
        m_Player_Input.View_X = Mathf.Max(-PITCH_LIMIT, Mathf.Min(PITCH_LIMIT, m_Player_Input.View_X + _view.x));
        // 양옆
        m_Player_Input.View_Y = m_Player_Input.View_Y + _view.y;
    }

    public void onTool_1(CallbackContext _context)
    { m_Player_Input.Tool_1 = _context.ReadValueAsButton(); }
    public void onTool_2(CallbackContext _context)
    { m_Player_Input.Tool_2 = _context.ReadValueAsButton(); }
    public void onTool_3(CallbackContext _context)
    { m_Player_Input.Tool_3 = _context.ReadValueAsButton(); }
    public void onTool_4(CallbackContext _context)
    { m_Player_Input.Tool_4 = _context.ReadValueAsButton(); }

    /// <summary>
    /// 상호작용
    /// </summary>
    /// <param name="_context"></param>
    public void onInteract(CallbackContext _context)
    {
        m_Player_Input.Interact = _context.ReadValueAsButton();
    }
    /// <summary>
    /// 발사
    /// </summary>
    /// <param name="_context"></param>
    public void onFire(CallbackContext _context)
    {
        m_Player_Input.Fire = _context.ReadValueAsButton();
    }
    /// <summary>
    /// 후로젝숀
    /// </summary>
    /// <param name="_context"></param>
    public void onProjection(CallbackContext _context)
    {
        e_Input_Projection.Invoke(_context.ReadValueAsButton());
    }
}