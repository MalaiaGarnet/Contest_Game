using Network.Data;
using System;
using System.Collections;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

/// <summary>버튼 트리거 이벤트(버튼명, 누름/뗌 여부)</summary>
public class Event_Button_Triggered : UnityEvent<string, bool> { }
/// <summary>툴 변경 이벤트(변경한 툴 인덱스)</summary>
public class Event_Tool_Changed : UnityEvent<int> { }

/// <summary>
/// 200913 주현킴
/// 캐릭터 컨트롤러 베이스 클래스
/// </summary>
public class CharacterController : MonoBehaviour
{
    [Header("입출력")]
    protected Manager_Input InputManager;   // 입력 관리자
    private  User_Input     m_Output;       // 서버로부터 받은 입력값

    [Header("네트워크/프로필")]
    public User_Profile                  m_MyProfile;   // 캐릭터 프로필
    private User_Profile                 m_Profile_Before; // 이전 입력값
    private UnityAction<User_Profile[]>  m_PlayerInputEvts; // 입력 수신 이벤트
    private UnityAction<User_Profile[]>  m_PlayerUpdatePosEvts; // 위치 갱신 이벤트

    [Header("이벤트")]
    public Event_Button_Triggered e_Triggered = new Event_Button_Triggered();
    public Event_Tool_Changed e_ToolChanged = new Event_Tool_Changed();

    [Header("캐릭터 스탯")]
    private short           m_Index;
    public  int             battery = 10000;
    public  float           moveSpeed;

    [Header("캐릭터 물리메테리얼")]
    public PhysicMaterial   noFriction;     //프릭션 X
    public PhysicMaterial   fullFriction; // 프릭션 O

    [Header("캐릭터 무기")]
    public Transform m_ToolAxis;
    public Tool[] m_Tools = new Tool[4]; // 툴 리스트

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

        // 프로필에 해당하는 툴 등록
        m_Tools[0] = MakeTool(m_MyProfile.Tool_1);
        m_Tools[1] = MakeTool(m_MyProfile.Tool_2);
        m_Tools[2] = MakeTool(m_MyProfile.Tool_3);
        m_Tools[3] = MakeTool(m_MyProfile.Tool_4);
        ChangeTool(1);
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
                Update_Profile(_Profiles[index]);
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

                Update_Profile(_Profiles[index]);
                transform.position = m_MyProfile.Current_Pos;
            }
        }
    }

    /// <summary>
    /// 입력 업데이트<br/>
    /// 트리거의 경우 눌렀을 때, 뗐을 때 이벤트 발동
    /// </summary>
    /// <param name="_input">새로 들어온 입력값</param>
    void Update_Profile(User_Profile _new_profile)
    {
        // 발싸!
        if (m_Output.Fire != _new_profile.User_Input.Fire)
            e_Triggered.Invoke("Fire", _new_profile.User_Input.Fire);
        // 킹호작용!
        if (m_Output.Interact != _new_profile.User_Input.Interact)
            e_Triggered.Invoke("Interact", _new_profile.User_Input.Interact);

        bool debug_tool_change = false;
        if (Manager_Ingame.Instance.m_DebugMode)
        {
            if (_new_profile.User_Input.Tool_1 && _new_profile.Current_Tool != 1)
            { _new_profile.Current_Tool = 1; debug_tool_change = true; }
            else if (_new_profile.User_Input.Tool_2 && _new_profile.Current_Tool != 2)
            { _new_profile.Current_Tool = 2; debug_tool_change = true; }
            else if (_new_profile.User_Input.Tool_3 && _new_profile.Current_Tool != 3)
            { _new_profile.Current_Tool = 3; debug_tool_change = true; }
            else if (_new_profile.User_Input.Tool_4 && _new_profile.Current_Tool != 4)
            { _new_profile.Current_Tool = 4; debug_tool_change = true; }
        }
        if (m_MyProfile.Current_Tool != _new_profile.Current_Tool || debug_tool_change)
        {
            ChangeTool(_new_profile.Current_Tool);
            e_ToolChanged.Invoke(_new_profile.Current_Tool);
        }

        m_MyProfile = _new_profile;
        m_Output = _new_profile.User_Input;
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
        Vector3 movement = dir * moveSpeed * 100f * Time.fixedDeltaTime;
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
            // Debug.Log("충돌된 좌표 : " + hitpos + " 충돌 오브젝트 타입 : " + hit.collider.ToString());
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

    /// <summary>
    /// 툴 매니저로부터 툴 프리팹을 얻어와 생성하고, 그것을 손에 쥐게한다
    /// 생성된 툴은 기본적으로 비활성화되어있다
    /// </summary>
    /// <param name="_oid"></param>
    /// <returns></returns>
    Tool MakeTool(ushort _oid)
    {
        if (_oid == 0)
            return null;

        GameObject prefab = Manager_Tool.Instance.Get_Tool_Prefab(_oid);
        if (prefab == null)
            return null;

        GameObject tool = Instantiate(prefab, m_ToolAxis);
        tool.transform.localPosition = Vector3.zero;
        tool.transform.localRotation = Quaternion.identity;
        tool.SetActive(false);

        return tool.GetComponent<Tool>();
    }

    void ChangeTool(int _index)
    {
        for (int i = 0; i < m_Tools.Length; i++)
        {
            if (m_Tools[i] != null)
            {
                bool enable = i + 1 == _index;
                m_Tools[i].gameObject.SetActive(enable);
                if (enable)
                    m_Tools[i].Register(e_Triggered);
                else
                    m_Tools[i].Unregister();
            }
        }
    }
}