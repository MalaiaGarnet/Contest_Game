using Network.Data;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.VFX;

public class Weapon : Tool, I_IK_Shotable
{
    public bool isDrawRay;      //레이를 그릴것인지
    ushort m_WeaponID;          //무기번호
    [Tooltip("무기 분류 타입")]
    public WeaponCategory WeaponCategory = new WeaponCategory();
    public string wponName;     // 무기명
    public float  shotInternal; // 샷 간격
    public Pellet pellet;       // 펠릿(히트스캔류)
    public int    clip;         // 장탄수
    public int    ammo;         // 발사수
    public float  shotDistance; // 총기 사거리
    public LayerMask traceFilter; // 필터링

    public AnimationClip m_Aim_Ani;
    [Header("트랜스폼")]
    [Tooltip("총을 안든 손")]
    public Transform ownerNoGunInArm;
    [Tooltip("총을 든 손")]
    public Transform ownerArm;
    [Tooltip("총구")]
    public Transform ownerMuzzle;

    private Vector3      m_FOVScreenPos;
    
    public Vector3[] HitVector
    {
        get;
        protected set;
    }
    void Awake()
    {
        m_FOVScreenPos = new Vector3((Camera.main.pixelWidth / 2),  (Camera.main.pixelHeight / 2), 0.0f);
        // 히트스캔일때
        if (WeaponCategory.eWeaponDetectionType == WeaponDetectionType.HITSCAN)
        {
            //pellet = new Pellet(pellet.pelletCount, pellet.pelletAccurate, pellet.pelletDamage);
            pellet.pelletPoint = new Vector2[pellet.pelletCount]; // 펠릿의 갯수만큼 
        }
       
    }


    // Use this for initialization
    void Start()
    {
        
    }
    // Update is called once per frame
    void Update()
    {
        Shooting();
    }

    // 여기 조져야됨
    public void Shooting()
    {
        if(clip <= 0)
        {
            Reload();
        }
   
        List<Ray> rays = new List<Ray>();
        for(int count = 0; count < pellet.pelletCount; count++)
        {
            rays.Add(Camera.main.ViewportPointToRay(new Vector3(m_FOVScreenPos.x + Random.Range(-pellet.pelletAccurate, pellet.pelletAccurate),
                m_FOVScreenPos.y + Random.Range(-pellet.pelletAccurate, pellet.pelletAccurate), m_FOVScreenPos.z)));
        }

        Vector3 muzzleForward = ownerMuzzle.transform.TransformDirection(Vector3.forward) * shotDistance;

        RaycastHit hitInfo;

        foreach (Ray ray in rays)
        {
            if (Physics.Raycast(ray.origin, muzzleForward, out hitInfo, shotDistance))
            {
                CharacterController victim = hitInfo.collider.gameObject.GetComponent<CharacterController>();

                if (isDrawRay) // 디버그용
                {
                    UnityEngine.Debug.DrawLine(ownerMuzzle.position, muzzleForward, Color.blue);
                    if (hitInfo.collider.CompareTag("Player"))
                        UnityEngine.Debug.Log(victim.transform.position);
                    else
                        UnityEngine.Debug.Log(hitInfo.collider);
                }

               if(Manager_Ingame.Instance.m_Client_Profile.Session_ID == victim.m_MyProfile.Session_ID)
               {
                    //OnTakeDamage(); 이곳에 네트워크 관련 작용
                    
               }

            }
        }

        clip--;
    }

    public void Reload()
    {
        clip = 1000;
    }

    public void SetAttacker()
    {
        
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
            Shooting();
    }

    public override void onInteract(bool _pressed)
    {
    }
}