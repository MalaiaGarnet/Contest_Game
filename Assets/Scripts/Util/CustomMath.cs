using System;

/// <summary>
/// 기존 Math/MathF 클래스에서 지원하는 기능의 확장 및 커스텀 수학기능을 지원하도록 합니다.
/// </summary>
public static class CustomMath
{
    /// <summary>
    /// MathF.Clamp()의 제네릭 지원버젼입니다.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="_Result"></param>
    /// <param name="_Min"></param>
    /// <param name="_Max"></param>
    /// <returns></returns>
    public static T Clamp<T>(this T _Result, T _Min, T _Max) where T : IComparable<T>
    {

        if (_Result.CompareTo(_Min) < 0)
        {
            return _Min;
        }
        else if (_Result.CompareTo(_Max) > 0)
        {
            return _Max;
        }
        else
        {
            return _Result;
        }
    }
}

public class RangeLimit<T> where T : IComparable<T>
{
    public T Min { get; }
    public T Max { get; }
    public RangeLimit(T min, T max)
    {
        if (min.CompareTo(max) > 0)
            throw new InvalidOperationException("invalid range");
        Min = min;
        Max = max;
    }

    public void Validate(T param)
    {
        if (param.CompareTo(Min) < 0 || param.CompareTo(Max) > 0)
            throw new InvalidOperationException("invalid argument");
    }

    public T Clamp(T param) => param.CompareTo(Min) < 0 ? Min : param.CompareTo(Max) > 0 ? Max : param;
}