using Network.Data;
using System;
using System.Collections;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

/// <summary>
/// 200913 주현킴
/// 캐릭터 컨트롤러 베이스 클래스
/// </summary>
class CharacterController : MonoBehaviour
{
    [Header("입출력")]
    protected Manager_Input InputManager;   // 입력 관리자
    private  User_Input     m_Output;       // 서버로부터 받은 입력값

    [Header("네트워크/프로필")]
    public User_Profile                  m_MyProfile;   // 캐릭터 프로필
    private UnityAction<User_Profile[]>  m_PlayerInputEvts; // 입력 수신 이벤트
    private UnityAction<User_Profile[]>  m_PlayerUpdatePosEvts; // 위치 갱신 이벤트

    [Header("캐릭터 스탯")]
    private short           m_Index;
    public  int             battery = 10000;
    public  float           moveSpeed;

    [Header("캐릭터 물리메테리얼")]
    public PhysicMaterial   noFriction;     //프릭션 X
    public PhysicMaterial   fullFriction; // 프릭션 O

    [Header("캐릭터 무기")]

    [Header("캐릭터 좌표")]
    public Transform m_CameraAxis;
    public const float ASCENDING_LIMIT = 0.25f;

    IEnumerator Start()
    {
        // 인풋 매니저 싱글톤 대기
        while (Manager_Input.Instance == null)
            yield return new WaitForEndOfFrame();
        InputManager = Manager_Input.Instance;

        // 입력 이벤트 등록
        m_PlayerInputEvts = new UnityAction<User_Profile[]>(When_Player_Input);
        m_PlayerUpdatePosEvts = new UnityAction<User_Profile[]>(When_Player_UpdatePosition);
        if (Manager_Network.Instance == null)
        {
            // 네트워크 매니저 없음 -> 로컬 디버그 모드
            Manager_Ingame.Instance.e_FakeInput.AddListener(m_PlayerInputEvts);
        }
        else
        {
            // 네트워크 매니저 있음 -> 서버 거쳐서 인풋 받기
            Manager_Network.Instance.e_PlayerInput.AddListener(m_PlayerInputEvts);
            Manager_Network.Instance.e_HeartBeat.AddListener(m_PlayerUpdatePosEvts);
        }
    }

    void OnDestroy()
    {
        // 등록된 이벤트 떼기
        if (Manager_Network.Instance == null)
        {
            if (Manager_Ingame.Instance != null) // 아직 인게임 인스턴스가 파괴되지않았을때만
            {
                Manager_Ingame.Instance.e_FakeInput.RemoveListener(m_PlayerInputEvts);
            }
        }
        else
        {
            Manager_Network.Instance.e_PlayerInput.RemoveListener(m_PlayerInputEvts);
            Manager_Network.Instance.e_HeartBeat.RemoveListener(m_PlayerUpdatePosEvts);
        }
    }

    private void Update()
    {
        // 시선 처리
        if (m_MyProfile.Session_ID == Manager_Ingame.Instance.m_Client_Profile.Session_ID)
        {
            m_CameraAxis.localRotation = Quaternion.Euler(
                new Vector3(InputManager.m_Player_Input.View_X, InputManager.m_Player_Input.View_Y, 0f));
        }
        else
        {
            m_CameraAxis.localRotation = Quaternion.Euler(new Vector3(m_Output.View_X, m_Output.View_Y, 0f));
        }
    }

    void FixedUpdate()
    {
        // 인풋에 따른 이동 처리
        Move();

        // 충돌 예지
        if (Manager_Ingame.Instance.m_Client_Profile.Session_ID == m_MyProfile.Session_ID)
            InputManager.m_Pre_Position = Predict_Collide();
    }

    /// <summary>
    /// 플레이어 입력 이벤트
    /// </summary>
    /// <param name="_Profiles"></param>
    void When_Player_Input(User_Profile[] _Profiles)
    {
        for (int index = 0; index < _Profiles.Length; index++)
        {
            if (m_MyProfile.Session_ID == _Profiles[index].Session_ID)
            {
                m_MyProfile = _Profiles[index];
                m_Output.Move_X = m_MyProfile.User_Input.Move_X;
                m_Output.Move_Y = m_MyProfile.User_Input.Move_Y;
                m_Output.View_X = m_MyProfile.User_Input.View_X;
                m_Output.View_Y = m_MyProfile.User_Input.View_Y;
                // m_NowPos = m_MyProfile.Current_Pos;
            }
        }
    }

    /// <summary>
    /// 플레이어 위치 갱신 이벤트
    /// </summary>
    /// <param name="_Profiles"></param>
    void When_Player_UpdatePosition(User_Profile[] _Profiles)
    {
        for (int index = 0; index < _Profiles.Length; index++)
        {
            if (m_MyProfile.Session_ID == _Profiles[index].Session_ID)
            {
                if (m_MyProfile.Session_ID == Manager_Ingame.Instance.m_Client_Profile.Session_ID)
                    continue;

                m_MyProfile = _Profiles[index];
                m_Output.Move_X = m_MyProfile.User_Input.Move_X;
                m_Output.Move_Y = m_MyProfile.User_Input.Move_Y;
                m_Output.View_X = m_MyProfile.User_Input.View_X;
                m_Output.View_Y = m_MyProfile.User_Input.View_Y;
                transform.position = m_MyProfile.Current_Pos;
            }
        }
    }

    /// <summary>
    /// 해당 오브젝트에게로 카메라 고정 지시
    /// </summary>
    public void Fix_Camera()
    {
        Camera.main.transform.SetParent(m_CameraAxis);
        Camera.main.transform.localPosition = Vector3.zero;
        Camera.main.transform.localRotation = Quaternion.identity;
    }

    Vector3 Calculate_Direction()
    {
        // 시점 방향값
        float view_rad = (-m_CameraAxis.rotation.eulerAngles.y) * Mathf.Deg2Rad;
        // 스틱 방향값
        float stick_rad = Mathf.Atan2(m_Output.Move_Y, m_Output.Move_X);
        // 총합 방향값
        float dir_rad = view_rad + stick_rad;

        // 방향 벡터
        return new Vector3(Mathf.Cos(dir_rad), 0f, Mathf.Sin(dir_rad));
    }

    void Move()
    {
        // 만약에 이동하지 않을때
        if(m_Output.Move_X == 0 && m_Output.Move_Y == 0)
        {
            GetComponentInChildren<CapsuleCollider>().sharedMaterial = fullFriction;
        }
        else // 이동중일때
        {
            GetComponentInChildren<CapsuleCollider>().sharedMaterial = noFriction;
        }

        Vector2 dist = new Vector2(m_Output.Move_X, m_Output.Move_Y);
        if (dist.magnitude <= 0.1f)
            return;
        Vector3 dir = Calculate_Direction();
        Vector3 movement = dir * moveSpeed * Time.fixedDeltaTime;
        Vector3 final_pos = transform.position + movement;

        // 충돌 체크 시작 (아랫 방향)
        //Ray ray = new Ray(final_pos + new Vector3(0f, ASCENDING_LIMIT, 0f), new Vector3(0f, -1f, 0f));      
        Ray ray = new Ray(transform.position, dir.normalized);      

        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, ASCENDING_LIMIT * 2f))
            final_pos = hit.point;

        //GetComponent<Rigidbody>().MovePosition(final_pos);
        Vector3 vector3 = new Vector3(movement.x, GetComponent<Rigidbody>().velocity.y, movement.z);
        GetComponent<Rigidbody>().velocity = vector3;
    }

    /// <summary>
    /// 충돌 예측
    /// </summary>
    Vector3 Predict_Collide()
    {
        // 무입력시 체크 X
        if(m_Output.Move_X.Equals(0.0f) && m_Output.Move_Y.Equals(0.0f))
            return transform.position;

        // 캐릭터 콜라이더 취득
        //CapsuleCollider collider = gameObject.GetComponent<CapsuleCollider>();
        // TODO : 모델을 따라가는 콜라이더를 취득.
        CapsuleCollider collider = gameObject.GetComponentInChildren<CapsuleCollider>();

        // 방향 계산
        Vector3 dir = Calculate_Direction();
        // 예측해야하는 거리 계산
        float dist = collider.radius + moveSpeed * Manager_Ingame.Instance.m_Input_Update_Interval;

        // 충돌 체크 시작
        // Physics.CapsuleCast  ==> 원래 해야하는 충돌체크
        Ray ray = new Ray(transform.position + new Vector3(0f, ASCENDING_LIMIT, 0f) + dir * collider.radius, dir);
        RaycastHit hit;
        Vector3 final_pos;
        int mask = LayerMask.NameToLayer("Player");

        if (Physics.Raycast(ray, out hit, dist, ~mask))
        {
            Vector3 hitpos = hit.point; // 충돌 좌표를 저장
            Debug.Log("충돌된 좌표 : " + hitpos + " 충돌 오브젝트 타입 : " + hit.collider.ToString());
            m_MyProfile.Current_Pos = hitpos;
            final_pos = hitpos;
        }
        else
            final_pos = ray.GetPoint(dist); // 부딪힌 거 없으면 적당히 나아가기만 하기
        final_pos -= dir * collider.radius;

        // 충돌 체크 시작 (아랫 방향)
        ray = new Ray(final_pos + new Vector3(0f, ASCENDING_LIMIT, 0f), new Vector3(0f, -1f, 0f));
        if (Physics.Raycast(ray, out hit, ASCENDING_LIMIT * 2f, ~mask))
        {
            final_pos = hit.point;
        }
        return final_pos;

    }
}