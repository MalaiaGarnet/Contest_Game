using System;
using UnityEngine;

[Serializable]
public class Laser
{
    [Header("레이저 설정")]
    [Tooltip("레이저 갯수")]
    public short laserCount;
    [Tooltip("레이저 사출가로넓이")]
    public float laserWidth;
    [Tooltip("레이저 데미지")]
    public float LaserDamage;
    [Tooltip("펠릿 속도")]
    public float LaserSpeed;
    [Tooltip("펠릿 사거리")]
    public float LaserDist;
    [Tooltip("펠릿의 관통여부")]
    public bool IsPenetrate = false;
    [Tooltip("관통 가능 인원")]
    public short penetrateCount;
    public Laser()
    {

    }
}


