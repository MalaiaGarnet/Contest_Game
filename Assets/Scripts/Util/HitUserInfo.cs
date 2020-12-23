using System.Collections;
using UnityEngine;

public struct HitUserInfo
{
    public ushort AttackerID { get; set; }
    public ushort VictimID { get; set; }
    public Transform Attacker { get; set; }
    public Transform Victim { get; set;  }
    public Vector3 AttackerOrigin { get; set; }
    public Vector3 VictimOrigin { get; set; }
}

public class DisposeDeathPlayer : SingleToneMonoBehaviour<DisposeDeathPlayer>
{
    public HitUserInfo HitUsersInfos { get; set; }
}