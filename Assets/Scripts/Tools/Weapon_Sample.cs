using Network.Data;
using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

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
    public GameObject prefab_Bullet;
    public GameObject effect_Bullet;
    public AudioSource sfx_Fire;

    Vector3 saved_pos;

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


    private void Update()
    {
        Debug.DrawRay(m_Gun_Muzzle.transform.position, Camera.main.transform.forward);
        saved_pos = m_Gun_Muzzle.transform.position;
        // Debug.Log(m_Gun_Muzzle.transform.position);
    }

    public override void onFire(bool _pressed)
    {
        if (!_pressed)
            return;

        int bullet_count = 8;
        float range = 0.03f;

        CharacterController cc = transform.GetComponentInParent<CharacterController>();

        Debug.Log(gameObject.name + " 발싸 - " + _pressed);
        Vector3 raw_dir = cc.m_CameraAxis.rotation.eulerAngles;

        raw_dir.x += Random.Range(-range, range);
        raw_dir.y += Random.Range(-range, range);
        Vector3 dir = cc.m_CameraAxis.forward;

        List<UInt16> session_ids = new List<UInt16>();
        List<Vector3> impact_pos = new List<Vector3>();

        sfx_Fire.PlayOneShot(sfx_Fire.clip);

        for (int i = 0; i < bullet_count; i++)
        {
            Vector3 temp_dir = dir + new Vector3(Random.Range(-range, range),
                Random.Range(-range, range), Random.Range(-range, range));
            Ray ray = new Ray(cc.m_CameraAxis.position + temp_dir * 2f, temp_dir);

            session_ids.Add(0);
            if (Physics.Raycast(ray, out RaycastHit hit, 100f))
            {
                CharacterController hit_user = hit.collider.gameObject.GetComponentInParent<CharacterController>();
                if (hit_user != null)
                {
                    Debug.Log("산탄 히트 - " + hit_user.m_MyProfile.Session_ID);
                    session_ids[i] = hit_user.m_MyProfile.Session_ID;
                }
                impact_pos.Add(hit.point);
            }
            else
                impact_pos.Add(ray.GetPoint(100f));

            // Debug.Log("Shot - " + m_Gun_Muzzle.transform.position);
            GameObject bullet = Instantiate(prefab_Bullet);
            LineRenderer lr = bullet.GetComponent<LineRenderer>();
            lr.SetPosition(0, saved_pos);
            lr.SetPosition(1, impact_pos[i]);

            GameObject effect = Instantiate(effect_Bullet);

            effect.transform.SetParent(cc.m_ToolAxis);
            effect.transform.SetPositionAndRotation(cc.m_ToolAxis.position, cc.m_ToolAxis.rotation);

            Destroy(effect, 1.0f);
            Destroy(bullet, 0.05f);
        }

        if (Manager_Ingame.Instance.m_Client_Profile.Session_ID == cc.m_MyProfile.Session_ID)
        {
            if (Manager_Network.Instance != null)
            {
                Packet_Sender.Send_Shot_Fire((UInt64)PROTOCOL.MNG_INGAME
                    | (UInt64)PROTOCOL_INGAME.SHOT | (UInt64)PROTOCOL_INGAME.SHOT_FIRE,
                    session_ids, impact_pos);
            }
        }
    }

    public override void onInteract(bool _pressed)
    {
    }
}
