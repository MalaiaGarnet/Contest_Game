using System.Collections.Generic;
using UnityEngine;
using System;

[RequireComponent(typeof(Weapon))]
public class Weapon_Laser : Tool, I_IK_Shotable
{
    [Header("에임 애니메이션, 총구, 총알발사위치")]
    public AnimationClip animAim;
    public Transform gunleftGrip;
    public Transform gunMuzzle;

    [Header("비쥬얼 및 사운드 이펙트")]
    [Tooltip("총구 화염")]
    public GameObject effect_Fire;
    [Tooltip("발사음")]
    public AudioSource sfx_Fire;

    public Laser laserInfo;
    public LaserTrail laserTraii;
    public ushort weapon_uid = 4002;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

    }

    public override void onFire(bool _pressed)
    {
        if (!_pressed) 
            return;

        // 공격자 구하기
        CharacterController attacker = GetComponentInParent<CharacterController>();
        // 보고있는 방향을 구해주고
        Vector3 eyeDir = attacker.m_CameraAxis.forward;
        Ray ray = new Ray(attacker.m_CameraAxis.position + eyeDir * 2f, eyeDir);
        List<UInt16> victim_IDs = new List<UInt16>();
        List<Vector3> impact_Pos = new List<Vector3>();
        RaycastHit hit;

        for(short i = 1; i < laserTraii.Laser.laserCount; i++)
        {
            GameObject laserObj = Instantiate(laserTraii.gameObject, gunMuzzle.position, attacker.m_ToolAxis.rotation);
            if(Physics.Raycast(ray, out hit, laserTraii.Laser.LaserDist))
            {

            }
        }

    }

    public override void onInteract(bool _pressed)
    {

    }



    public AnimationClip Get_Aim_Anim()
    {
        return animAim;
    }

    public Transform Get_Lefthand_Grip()
    {
        return gunleftGrip;
    }

    public Transform Get_Muzzle()
    {
        return gunMuzzle;
    }
}
