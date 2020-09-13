using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 200912 주현킴
/// 캐릭터 애니메이션 관리 클래스
/// </summary>
public class CharacterAnimator : MonoBehaviour
{
    
    public Vector2 debug_stick_value;

    public Vector2 m_StickValue;
    public Vector2 m_LastValue;

    CharacterController pc;
    Animator m_Anim;

    void Start()
    {
        pc = GetComponent<CharacterController>();
        m_Anim = GetComponentInChildren<Animator>();
    }

    void FixedUpdate()
    {
        m_StickValue = new Vector2(pc.m_MyProfile.User_Input.Move_X, pc.m_MyProfile.User_Input.Move_Y);
        if (m_StickValue.y < 0.0f)
            m_StickValue.x = -m_StickValue.x;
        m_LastValue = Vector2.Lerp(m_LastValue, m_StickValue, 0.1f);

        m_Anim.SetFloat("Horizontal", m_LastValue.x);
        m_Anim.SetFloat("Vertical", m_LastValue.y);
        //m_Anim.SetFloat("Horizontal", debug_stick_value.x);
        //m_Anim.SetFloat("Vertical", debug_stick_value.y);
    }
}
