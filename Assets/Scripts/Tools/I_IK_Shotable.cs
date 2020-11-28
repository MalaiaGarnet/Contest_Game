using UnityEngine;

/// <summary>
/// 조준 모션이 있음을 알려주는 인터페이스
/// 근접 무기는 이 인터페이스가 필요 없다
/// </summary>
interface I_IK_Shotable
{
    /// <summary>
    /// 조준하는 애니메이션 클립 취득
    /// </summary>
    AnimationClip Get_Aim_Anim();

    /// <summary>
    /// 정면을 겨눠야하는 트랜스폼 취득<br/>
    /// (총구 or 조준경)
    /// </summary>
    Transform Get_Muzzle();

    /// <summary>
    /// 왼손이 가야하는 트랜스폼 취득<br/>
    /// 포지션, 회전값에 맞춰 왼손이 움직임
    /// </summary>
    Transform Get_Lefthand_Grip();
}
