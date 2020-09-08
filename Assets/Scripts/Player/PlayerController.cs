using Network.Data;
using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

class PlayerController : MonoBehaviour
{
    [Header("입출력")]
    protected Manager_Input playerInputs;
    private  User_Input       m_Output;

    [Header("네트워크/프로필")]
    public User_Profile                  m_MyProfile;
    private UnityAction<User_Profile[]>  m_PlayerInputEvts;

    [Header("캐릭터 스탯")]
    private short           m_Index;
    public  int             battery = 10000;
    public  float           moveSpeed;
    //[Header("캐릭터 무기")]

    [Header("캐릭터 좌표")]
    private Vector3         m_NowPos;
    private Vector3         m_hitPos;
    private Vector3         m_RayCastDir;


    void Start()
    {
        InitializePlayer();
    }

    void InitializePlayer()
    {
        playerInputs = Manager_Input.Instance;
        playerInputs.Initialized();
        m_NowPos = Vector3.zero;
        m_hitPos = Vector3.zero;
        m_RayCastDir = Vector3.zero;

        if (m_PlayerInputEvts == null)
        {
            m_PlayerInputEvts = new UnityAction<User_Profile[]>(ReciveUserMoveMent);
            Manager_Network.Instance.e_PlayerInput.AddListener(ReciveUserMoveMent);
        }
        else if(m_PlayerInputEvts != null)
        {
            if(Manager_Network.Instance.e_PlayerInput.GetPersistentEventCount() != 0)
                Manager_Network.Instance.e_PlayerInput.RemoveAllListeners();
        }
    }

    public void CallPlayerMoveMent()
    {
        playerInputs.CreateMovingAction();
    }

    void ReciveUserMoveMent(User_Profile[] _Profiles)
    {
        Debug.Log("무브먼트 발현");
        for (int index = 0; index < _Profiles.Length; index++)
        {
            Debug.Log(m_MyProfile.Session_ID + " == " + _Profiles[index].Session_ID);
            if (m_MyProfile.Session_ID == _Profiles[index].Session_ID)
            {
                m_MyProfile = _Profiles[index];
                m_Output.Move_X = m_MyProfile.User_Input.Move_X;
                m_Output.Move_Y = m_MyProfile.User_Input.Move_Y;
                m_NowPos = m_MyProfile.Current_Pos;
                UpdatePosition();
                CheckCollision();
            }
        }
        // playerInputs.InputDirection = new Vector3(m_Output.Move_X, 0, m_Output.Move_Y);
    }
    /// <summary>
    /// 충돌 처리를 합니다!
    /// 
    /// </summary>
    bool CheckCollision()
    {
        // 무입력 처리
        if(m_Output.Move_X.Equals(0.0f) &&
            m_Output.Move_Y.Equals(0.0f))
            return false;

        CapsuleCollider collider = gameObject.GetComponent<CapsuleCollider>();

        // 시점 방향값 => 카메라의 rotation.eulerangle.y * mathf.deg2rad 로 라디안 구해서 해보셈
        // float view_rad = Mathf.Atan2(transform.forward.z, transform.forward.x);
        // 스틱 방향값
        float stick_rad = Mathf.Atan2(m_Output.Move_Y, m_Output.Move_X);
        // 총합 방향값
        float dir_rad = /*view_rad + */stick_rad;

        m_RayCastDir.x = Mathf.Cos(dir_rad);
        m_RayCastDir.y = 0f;
        m_RayCastDir.z = Mathf.Sin(dir_rad);

        // 반지름 + 1.5(이동속도) * 0.1(타임스탬프)
        float dist = collider.radius + moveSpeed * playerInputs.TimeStamp;

        MoveThePlayer(m_RayCastDir, 1.5f);

        RaycastHit hit;
        Ray ray = new Ray(m_NowPos + new Vector3(0f, 0.2f, 0f) + m_RayCastDir * collider.radius, m_RayCastDir);
        // Ray ray = new Ray(m_NowPos + new Vector3(0f, 0.2f, 0f), m_RayCastDir);
        Debug.DrawRay(ray.origin, ray.direction, Color.red, 1.0f);
        Debug.Log("시작 좌표 = " + ray.origin);
        Debug.Log("레이 각도 = " + ray.direction);

        // Debug.LogFormat("콜라이더 : {0}, zx방향각 : {1}, xz방향각 : {2}\n 방향 : {3}, 거리 : {4}", collider, zxDirRadian, xzDirRadian, m_RayCastDir, dist);
        int layerMask = (1 << LayerMask.NameToLayer("Player"));
        layerMask = ~layerMask; // 리버스

        if (Physics.Raycast(ray, out hit, dist, layerMask))
        {
            if (hit.collider.CompareTag("Object")) // 만약 히트엔티티가 특정 오브젝트라면 // 히트콜라이더가 플레이어인이유
            {
                m_hitPos = hit.collider.transform.position; // 충돌 좌표를 저장
                Debug.Log("충돌된 좌표 : " + m_hitPos + " 충돌 오브젝트 타입 : " + hit.collider.ToString());
                m_MyProfile.Current_Pos = m_hitPos;
                return true;
            }
        }
        return false;
    }

    void UpdatePosition()
    {
        m_NowPos = gameObject.transform.position;
        m_MyProfile.Current_Pos = m_NowPos;
    }

    void FixedUpdate()
    {
        // UpdatePosition();
        // CheckCollision();
    }

    void TurnThePlayer()
    {
    }

    void MoveThePlayer(Vector3 _dir, float _speed_per_sec)
    {
        Vector3 movement = _dir.normalized * _speed_per_sec * Manager_Ingame.Instance.m_Input_Update_Interval;
        GetComponent<Rigidbody>().MovePosition(transform.position + movement);
    }

}