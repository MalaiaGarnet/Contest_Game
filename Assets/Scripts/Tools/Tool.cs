using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// 툴 베이스 클래스
/// </summary>
public abstract class Tool : MonoBehaviour
{
    /// <summary>
    /// 이 툴을 사용했을 때 벌어지는 일들
    /// </summary>
    public abstract void Use();

}
