using System;
using System.Runtime.InteropServices;
using UnityEngine;

[Serializable]
[StructLayout(LayoutKind.Explicit)]
public class WeaponCategory
{
    [Tooltip("무기칸")]
    [FieldOffset(0)]
    public WeaponBlock eWeaponBlock = WeaponBlock.NOTHING;

    [Tooltip("값확인")]
    [FieldOffset(4)]
    public int iSubType = 0;

    [Tooltip("무기판정종류")]
    [FieldOffset(4)]
    public WeaponDetectionType eWeaponDetectionType = 0;

    public WeaponCategory() { }
    public WeaponCategory(WeaponBlock _WeaponBlock, WeaponDetectionType _Detect)
    {
        eWeaponBlock = _WeaponBlock;
        eWeaponDetectionType = _Detect;
    }

    public bool Compare(WeaponBlock _WeaponBlock)
    {
        if ((eWeaponBlock & _WeaponBlock) == _WeaponBlock)
        {
            return true;
        }
        return false;
    }

    public WeaponDetectionType GetMelee()
    {
        if (eWeaponBlock == WeaponBlock.MELEE)
        {
            return eWeaponDetectionType;
        }
        return WeaponDetectionType.NULL_VALUE;
    }
}

/*
 * 무기 칸 종류
 */
public enum WeaponBlock
{
    NOTHING = 0x00,
    GUN = 0x01,
    TRAP = 0x02,
    MELEE = 0x04,

    EVERYTHING = 0xffff
}