using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 이것은 샘플 총이다
/// 모든 툴 관련 오브젝트는 Tool 클래스를 상속받을 수 있도록 한다
/// 중간에 무기 클래스 같은 거 껴넣어도 됨
/// </summary>
public class Weapon_Sample : Tool, I_IK_Shotable
{
    [Header("총기 세팅")]
    public AnimationClip m_Aim_Anim;
    public GameObject m_Gun_LeftGrip;
    public GameObject m_Gun_Muzzle;

    [Header("원하는 거 구현하기")]
    public int m_ID; // 지워도 됨


    public AnimationClip Get_Aim_Anim()
    {
        return m_Aim_Anim;
    }

    public Transform Get_Lefthand_Grip()
    {
        return m_Gun_LeftGrip.transform;
    }

    public Transform Get_Muzzle()
    {
        return m_Gun_Muzzle.transform;
    }

    public override void onFire(bool _pressed)
    {
        Debug.Log(gameObject.name + " 발싸 - " + _pressed);
    }

    public override void onInteract(bool _pressed)
    {
        Debug.Log(gameObject.name + " 사용 - " + _pressed);
    }
}
