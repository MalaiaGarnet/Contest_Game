using System;
using UnityEngine;
using UnityEngine.VFX;

/*
 *  샷건, 스캐터건 등의 발사되는 총알입니다.
 */
[Serializable]
public class Pellet
{
    [Tooltip("펠릿개수")]
    public byte pelletCount;
    [Tooltip("집탄율")]
    public float pelletAccurate;
    [Tooltip("펠릿 이펙트")]
    public VisualEffect pelletEffect;
    [Tooltip("펠릿 데미지")]
    public float pelletDamage;
    [Tooltip("펠릿 속도")]
    public float pelletSpeed;
    [Tooltip("펠릿 사거리")]
    public float pelletDist;
    [Tooltip("펠릿의 관통여부")]
    public bool IsPenetrate = false;
    [Tooltip("관통 가능 인원")]
    public short penetrateCount;
    [HideInInspector()]
    public Vector3 pelletDest;

    public Pellet()
    {

    }

    public Pellet(byte _Count, float _Accurate, float _Damage)
    {
        pelletCount = _Count;
        pelletAccurate = _Accurate;
        pelletDamage = _Damage;
    }
}
