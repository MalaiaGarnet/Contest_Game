using Network.Data;
using UnityEngine;
using UnityEngine.EventSystems;

class PlayerController : MonoBehaviour
{
    [Header("카메라")]
    public Camera           mainCamera;
    [Header("물리")]
    public Rigidbody        playerRigidbody;

    [Header("입출력")]
    private Manager_Input    m_PlayerInputs;
    public  User_Input       m_Output;

    [Header("네트워크/프로필")]
    private User_Profile[]    m_UserProfile;

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
        Manager_Input.Instance.PlayerRigidbody = playerRigidbody;
        Manager_Input.Instance.MainCamera = mainCamera;
        m_RayCastDir = new Vector3(0, 0, 0);
    }

    void ReciveUserMoveMent()
    {
        Manager_Input.Instance.recvInputEvent.Invoke(m_UserProfile);
        m_Output.Move_X = m_UserProfile[m_Index].User_Input.Move_X;
        m_Output.Move_Y = m_UserProfile[m_Index].User_Input.Move_Y;

        Manager_Input.Instance.InputDirection = new Vector3(m_Output.Move_X, 0, m_Output.Move_Y);
    }
    /// <summary>
    /// 충돌 처리를 합니다!
    /// 
    /// </summary>
    bool CheckCollision()
    {
        CapsuleCollider collider = gameObject.GetComponent<CapsuleCollider>();
        
        float zxDirRadian = transform.forward.z + Mathf.Atan2(transform.forward.z, transform.forward.x);
        float xzDirRadian = transform.forward.x + Mathf.Atan2(transform.forward.x, transform.forward.z);

        m_RayCastDir.x = Mathf.Sin(zxDirRadian);
        m_RayCastDir.y = 0.0f;
        m_RayCastDir.z = Mathf.Sin(zxDirRadian);

        float dist = collider.radius + moveSpeed * (Manager_Input.Instance.TimeStamp * 10);
        // 반지름 + 1.5(이동속도) * 0.1(타임스탬프)
        RaycastHit hit;

        Debug.LogFormat("콜라이더 : {0}, zx방향각 : {1}, xz방향각 : {2}\n 방향 : {3}, 거리 : {4}", collider, zxDirRadian, xzDirRadian, m_RayCastDir, dist);
        int layerMask = (1 << LayerMask.NameToLayer("Player"));
        layerMask = ~layerMask; // 리버스
        if (Physics.Raycast(m_NowPos + m_RayCastDir * collider.radius, m_RayCastDir, out hit, dist, layerMask))
        {
            if (hit.collider.CompareTag("Object")) // 만약 히트엔티티가 특정 오브젝트라면 // 히트콜라이더가 플레이어인이유
            {
                m_hitPos = hit.collider.transform.position; // 충돌 좌표를 저장
                Debug.Log("충돌된 좌표 : " + m_hitPos + " 충돌 오브젝트 타입 : " + hit.collider.ToString());
                return true;
            }
        }
        return false;
    }

    void UpdatePosition()
    {
        m_NowPos = gameObject.transform.position;
    }

    void FixedUpdate()
    {
        UpdatePosition();
        CheckCollision();
    }


}