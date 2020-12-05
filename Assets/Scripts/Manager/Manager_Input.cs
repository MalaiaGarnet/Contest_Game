using Network.Data;
using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;

public class Event_Input_Projection : UnityEvent<bool> { }
public class Manager_Input : SingleToneMonoBehaviour<Manager_Input>
{
    #region  필드와 프로퍼티

    const float PITCH_LIMIT = 80f;

    public Event_Input_Projection e_Input_Projection = new Event_Input_Projection();

    [Header("디버그로 확인")]
    public User_Input m_Player_Input;   // 클라이언트 최신 인풋
    public Vector3 m_Pre_Position;      // 예측 위치

    Ingame_UI ui;
    #endregion

    void Start()
    {
        ui = Ingame_UI.Instance;
    }

    void Update()
    {
        if (Manager_Ingame.Instance.m_Game_Started)
        {
            Update_MouseView();
        }
    }

    void Update_Profile()
    {
        if (Manager_Ingame.Instance.m_DebugMode)
            return;

        User_Profile[] profiles = new User_Profile[1];
        Manager_Ingame.Instance.m_Client_Profile.User_Input = m_Player_Input;
        profiles[0] = Manager_Ingame.Instance.m_Client_Profile;
        Manager_Network.Instance.e_PlayerInput.Invoke(profiles);
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
        if (!ui.Can_Move())
            return;
        m_Player_Input.Move_X = _context.ReadValue<Vector2>().x;
        m_Player_Input.Move_Y = _context.ReadValue<Vector2>().y;

        Update_Profile();
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
        if (!ui.Can_Move())
            return;
        // 위아래
        m_Player_Input.View_X = Mathf.Max(-PITCH_LIMIT, Mathf.Min(PITCH_LIMIT, m_Player_Input.View_X + _view.x));
        // 양옆
        m_Player_Input.View_Y = m_Player_Input.View_Y + _view.y;
    }

    public void onTool_1(CallbackContext _context)
    {
        // TODO 툴버튼 전부 잠시 막아두기
        return;
        if (!ui.Can_Move())
            return;
        m_Player_Input.Tool_1 = _context.ReadValueAsButton();
    }
    public void onTool_2(CallbackContext _context)
    {
        // TODO 툴버튼 전부 잠시 막아두기
        return;
        if (!ui.Can_Move())
            return;
        m_Player_Input.Tool_2 = _context.ReadValueAsButton();
    }
    public void onTool_3(CallbackContext _context)
    {
        // TODO 툴버튼 전부 잠시 막아두기
        return;
        if (!ui.Can_Move())
            return;
        m_Player_Input.Tool_3 = _context.ReadValueAsButton();
    }
    public void onTool_4(CallbackContext _context)
    {
        // TODO 툴버튼 전부 잠시 막아두기
        return;
        if (!ui.Can_Move())
            return;
        m_Player_Input.Tool_4 = _context.ReadValueAsButton();
    }
    /// <summary>
    /// 가드/로그 특수 기능
    /// </summary>
    /// <param name="_context"></param>
    public void onSkill(CallbackContext _context)
    {
        if (!ui.Can_Move())
            return;

        if (m_Player_Input.Role_Skill != _context.ReadValueAsButton())
        {
            m_Player_Input.Role_Skill = _context.ReadValueAsButton();
            if (Manager_Ingame.Instance.m_DebugMode)
                return;

            if (m_Player_Input.Role_Skill)
            {
                UInt64 protocol = (UInt64)PROTOCOL.MNG_INGAME | (UInt64)PROTOCOL_INGAME.SKILL | (UInt64)PROTOCOL_INGAME.SKILL_QUSTION;
                Packet_Sender.Send_Protocol(protocol);
            }
        }
    }

    /// <summary>
    /// 상호작용
    /// </summary>
    /// <param name="_context"></param>
    public void onInteract(CallbackContext _context)
    {
        if (!ui.Can_Move())
            return;
        m_Player_Input.Interact = _context.ReadValueAsButton();
    }
    /// <summary>
    /// 발사
    /// </summary>
    /// <param name="_context"></param>
    public void onFire(CallbackContext _context)
    {
        if (!ui.Can_Move())
            return;
        m_Player_Input.Fire = _context.ReadValueAsButton();

        // Update_Profile();
    }
    /// <summary>
    /// 후로젝숀
    /// </summary>
    /// <param name="_context"></param>
    public void onProjection(CallbackContext _context)
    {
        if (!ui.Can_Move())
            return;
        e_Input_Projection.Invoke(_context.ReadValueAsButton());

        Update_Profile();
    }
    /// <summary>
    /// 메뉴
    /// </summary>
    /// <param name="_context"></param>
    public void onMenu(CallbackContext _context)
    {
        if (Manager_Ingame.Instance.m_Game_Started)
            Ingame_UI.Instance.m_Menu.Toggle();
    }
    
    /// <summary>
    /// 스코어보드
    /// </summary>
    /// <param name="_context"></param>
    public void onScoreboard(CallbackContext _context)
    {
        if (Manager_Ingame.Instance.m_Game_Started)
            Ingame_UI.Instance.m_Scoreboard.SetActive(_context.ReadValueAsButton());
    }

}