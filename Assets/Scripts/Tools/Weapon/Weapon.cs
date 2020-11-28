using Network.Data;
using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Weapon : Tool, I_IK_Shotable
{
    public bool isDrawRay;      //레이를 그릴것인지
    ushort m_WeaponID;          //무기번호
    [Tooltip("무기 분류 타입")]
    public WeaponCategory WeaponCategory = new WeaponCategory();
    public string wponName;     // 무기명
    public float shotInternal; // 샷 간격
    public Pellet pellet;       // 펠릿(히트스캔류)
    public int clip;         // 장탄수
    public int ammo;         // 발사수
    public float shotDistance; // 총기 사거리
    public LayerMask traceFilter; // 필터링

    public AnimationClip m_Aim_Ani;
    [Header("트랜스폼")]
    [Tooltip("총을 안든 손")]
    public Transform ownerNoGunInArm;
    [Tooltip("총을 든 손")]
    public Transform ownerArm;
    [Tooltip("총구")]
    public Transform ownerMuzzle;

    private Vector3 m_FOVScreenPos;
    private Transform m_CamAxis;

    public Vector3[] HitVector
    {
        get;
        protected set;
    }
    void Awake()
    {
        m_FOVScreenPos = new Vector3((Camera.main.pixelWidth / 2), (Camera.main.pixelHeight / 2), 0.0f);
        // 히트스캔일때
        if (WeaponCategory.eWeaponDetectionType == WeaponDetectionType.HITSCAN)
        {
            //pellet = new Pellet(pellet.pelletCount, pellet.pelletAccurate, pellet.pelletDamage);
        }

    }


    // Use this for initialization
    void Start()
    {
        m_CamAxis = ownerArm.GetComponentInParent<CharacterController>().m_CameraAxis;
    }
    // Update is called once per frame
    void Update()
    {
        Shooting();
    }

    // 여기 조져야됨
    public void Shooting()
    {
        List<Ray> rays = new List<Ray>();
        for (int count = 0; count < pellet.pelletCount; count++)
        {
            rays.Add(m_CamAxis.GetComponentInParent<Camera>().ScreenPointToRay(new Vector3(m_FOVScreenPos.x + Random.Range(-pellet.pelletAccurate, pellet.pelletAccurate),
                m_FOVScreenPos.y + Random.Range(-pellet.pelletAccurate, pellet.pelletAccurate), m_FOVScreenPos.z)));
        }

        Vector3 muzzleForward = ownerMuzzle.transform.TransformDirection(Vector3.forward) * shotDistance;

        List<UInt16> list_SessionID = new List<UInt16>();
        List<Vector3> list_vHitPos = new List<Vector3>();

        RaycastHit hitInfo;

        CharacterController victim = this.gameObject.GetComponentInParent<CharacterController>();
        int index = 0;
        foreach (Ray ray in rays)
        {
            index++;
            list_SessionID.Add(0);

            if (Physics.Raycast(ray.origin, muzzleForward, out hitInfo, shotDistance))
            {
                victim = hitInfo.collider.gameObject.GetComponent<CharacterController>();

                if (isDrawRay) // 디버그용
                {
                    Debug.DrawLine(ownerMuzzle.position, muzzleForward, Color.blue);
                    if (hitInfo.collider.CompareTag("Player"))
                        Debug.Log(victim.transform.position);
                    else
                        Debug.Log(hitInfo.collider);
                }

                if (victim != null)
                {
                    list_SessionID[index] = victim.m_MyProfile.Session_ID;
                }



            }
        }

        if (Manager_Ingame.Instance.m_Client_Profile.Session_ID == victim.m_MyProfile.Session_ID)
        {
            if (Manager_Network.Instance != null)
            {
                Packet_Sender.Send_Shot_Fire((UInt64)PROTOCOL.MNG_INGAME
                    | (UInt64)PROTOCOL_INGAME.SHOT | (UInt64)PROTOCOL_INGAME.SHOT_FIRE,
                    list_SessionID, list_vHitPos);
            }
        }
    }

    public ushort GetWeaponID()
    {
        return m_WeaponID;
    }

    public AnimationClip Get_Aim_Anim()
    {
        return m_Aim_Ani;
    }

    public Transform Get_Muzzle()
    {
        return ownerMuzzle;
    }

    public Transform Get_Lefthand_Grip()
    {
        return ownerNoGunInArm;
    }

    public override void onFire(bool _pressed)
    {
        if (_pressed)
            return;

        Shooting();
    }

    public override void onInteract(bool _pressed)
    {
    }
}