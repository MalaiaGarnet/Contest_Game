using UnityEngine;
using System;
[Serializable]
public class CustomRange : PropertyAttribute
{
    // 필드
    [SerializeField]
    private float m_MaxValue;
    [SerializeField]
    private float m_Value;

    // 프로퍼티
    public float MaxValue
    {
        get => m_MaxValue;
        set
        {
            m_MaxValue = value;
            m_Value = CustomMath.Clamp(m_Value, 0, m_MaxValue);
        }
    }

    public float Value
    {
        get => m_Value;
        set => CustomMath.Clamp(value, 0, m_MaxValue);
    }

    public CustomRange(float _Min, float _Max)
    {
        MaxValue = _Max;
        Value = _Min;
    }

}


[Serializable]
public struct CustomRangeS
{
    // 필드
    [SerializeField]
    private float m_MaxValue;
    [SerializeField]
    private float m_Value;

    // 프로퍼티
    public float MaxValue
    {
        get => m_MaxValue;
        set
        {
            m_MaxValue = value;
            m_Value = CustomMath.Clamp(m_Value, 0, m_MaxValue);
        }
    }

    public float Value
    {
        get => m_Value;
        set => CustomMath.Clamp(value, 0, m_MaxValue);
    }

}