using RootMotion.FinalIK;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 200912 주현킴
/// 캐릭터 애니메이션 관리 클래스
/// </summary>
public class CharacterAnimator : MonoBehaviour
{
    [Header("IK 핸들")]
    public GameObject IK_AimMode;   // 조준 모드시의 IK
    public GameObject IK_LookMode;  // 일반 모드시의 IK

    [Header("움직임 값")]
    public Vector2 m_StickValue;
    public Vector2 m_LastValue;

    CharacterController pc;
    Animator m_Anim;

    void Start()
    {
        pc = GetComponent<CharacterController>();
        m_Anim = GetComponentInChildren<Animator>();

        pc.e_ToolChanged.AddListener(When_Tool_Changed);
    }

    void FixedUpdate()
    {
        m_StickValue = new Vector2(pc.m_MyProfile.User_Input.Move_X, pc.m_MyProfile.User_Input.Move_Y);
        if (m_StickValue.y < 0.0f)
            m_StickValue.x = -m_StickValue.x;
        m_LastValue = Vector2.Lerp(m_LastValue, m_StickValue, 0.2f);

        m_Anim.SetFloat("Horizontal", m_LastValue.x);
        m_Anim.SetFloat("Vertical", m_LastValue.y);
    }

    void When_Tool_Changed(int _tool_index)
    {
        // 툴 오브젝트를 취득
        Tool tool = pc.m_Tools[_tool_index - 1];
        if (tool == null)
        {
            IK_AimMode.SetActive(false);
            m_Anim.SetBool("is_Aiming", false);
            IK_LookMode.SetActive(true);
            return;
        }

        // 툴이 조준 가능함?
        I_IK_Shotable shotable = null;
        if (tool is I_IK_Shotable)
        {
            shotable = tool as I_IK_Shotable;
            AimIK aim = IK_AimMode.GetComponentInChildren<AimIK>(true);
            aim.solver.transform = shotable.Get_Muzzle();
            LimbIK limb = IK_AimMode.GetComponentInChildren<LimbIK>(true);
            limb.solver.target = shotable.Get_Lefthand_Grip();
        }

        // 조준 여부에 따른 애니메이션 처리
        IK_AimMode.SetActive(shotable != null);
        m_Anim.SetBool("is_Aiming", shotable != null);
        IK_LookMode.SetActive(shotable == null);
    }
}
