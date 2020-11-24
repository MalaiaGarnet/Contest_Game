using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Network.Data;
using UnityEngine.SceneManagement;
using System;

public class Event_RoundUpdate : UnityEvent<GameObject, Minimap> { }
public class Manager_Ingame : SingleToneMonoBehaviour<Manager_Ingame>
{
    public float m_Input_Update_Interval = 0.025f; // 인풋 보내는 속도

    [Header("세션 정보")]
    public Session_RoundData m_RoundData = new Session_RoundData();
    public User_Profile m_Client_Profile = new User_Profile();
    public List<User_Profile> m_Profiles = new List<User_Profile>();
    public List<Item_Data> m_Items = new List<Item_Data>();
    public float m_Heartbeat_Wait = 0f;
    public bool m_Game_Started = false;
    public int m_MapID = 0;
    public int m_Round = 0;

    [Header("프리팹")]
    public GameObject prefab_Guard;
    public GameObject prefab_Thief;
    List<GameObject> m_Round_Objects = new List<GameObject>(); // 라운드 끝나면 사라질 것들

    [Header("이벤트")]
    public Event_RoundUpdate e_RoundUpdate = new Event_RoundUpdate();
    List<IEnumerator> m_DelayedTaskList = new List<IEnumerator>();

    [Header("디버그 옵션")]
    public bool m_DebugMode = false;
    public Event_Player_Input e_FakeInput = new Event_Player_Input();
    public int m_FakeMap = 1, m_FakeRound = 1;

    IEnumerator Start()
    {
        if (m_DebugMode)
        {
            User_Profile up = new User_Profile();
            up.ID = "1";
            up.Session_ID = 0;
            up.Role_Index = 1;
            up.Current_Pos = new Vector3(2.0f, 2.0f, 0.0f);
            up.Tool_1 = 5001;
            up.HP = 2000;
            m_Profiles.Add(up);
            m_Client_Profile = up;

            up = new User_Profile();
            up.ID = "2";
            up.Session_ID = 1;
            up.Role_Index = 2;
            up.Tool_1 = 4001;
            up.HP = 2000;
            up.Current_Pos = new Vector3(-2.0f, 2.0f, 0.0f);
            m_Profiles.Add(up);

            Start_Game(1);
            yield return null;
        }

        while (Manager_Network.Instance == null)
            yield return new WaitForEndOfFrame();

        Manager_Network.Instance.e_HeartBeat.AddListener(new UnityAction<Session_RoundData, User_Profile[]>(Update_Datas));
        Manager_Network.Instance.e_GameStart.AddListener(new UnityAction<int>(Start_Game));
        Manager_Network.Instance.e_RoundReady.AddListener(new UnityAction<int>(Prepare_Round));
        Manager_Network.Instance.e_RoundStart.AddListener(new UnityAction(Start_Round));
        Manager_Network.Instance.e_RoundEnd.AddListener(new UnityAction(End_Round));
        Manager_Network.Instance.e_GameEnd.AddListener(new UnityAction(End_Game));
        Manager_Network.Instance.e_ItemSpawn.AddListener(new UnityAction<Item_Data[]>(Get_Items));
    }

    private void FixedUpdate()
    {
        if (m_DebugMode)
            return;

        if (m_Game_Started)
        {
            m_Heartbeat_Wait += Time.fixedDeltaTime;
            if (m_Heartbeat_Wait > 5.0) // 하트비트가 너무 안 오면 타이틀로
            {
                Quit_Game();
            }
        }
    }

    /// <summary>
    /// 하트비트 받을 때
    /// </summary>
    /// <param name="_datas"></param>
    public void Update_Datas(Session_RoundData _round, User_Profile[] _datas)
    {
        m_Heartbeat_Wait = 0;
        m_RoundData = _round;
        m_Profiles = new List<User_Profile>(_datas);
    }

    public void Add_Delayed_Coroutine(IEnumerator _func)
    {
        if (m_DelayedTaskList.Count > 0)
            m_DelayedTaskList.Add(_func);
        else
            StartCoroutine(_func);
    }
    public void Play_Next_Coroutine()
    {
        if (m_DelayedTaskList.Count == 0)
            return;
        m_DelayedTaskList.RemoveAt(0);
        if (m_DelayedTaskList.Count > 0)
            StartCoroutine(m_DelayedTaskList[0]);
    }

    /// <summary>
    /// 인게임 씬 불러오기
    /// </summary>
    public void Load_Ingame()
    {
        StartCoroutine(Load_Ingame_Process());
    }
    IEnumerator Load_Ingame_Process()
    {
        Ingame_UI ui = Ingame_UI.Instance;
        // 로딩창 부르기
        ui.m_Ingame_Scene_Loader.Show(true);
        yield return new WaitForSecondsRealtime(1.0f);

        // 장식용 1
        ui.m_Ingame_Scene_Loader.Add_Msg("loading process start");
        yield return new WaitForSecondsRealtime(0.5f);

        // 인게임 씬 로드
        SceneManager.LoadScene("Ingame");
        yield return new WaitForSecondsRealtime(2.0f);

        // hud 생성
        ui.m_Ingame_Scene_Loader.Add_Msg("create hud");
        ui.m_Header.SetActive(true);
        ui.m_Footer.SetActive(true);
        ui.m_LeftSide.SetActive(true);
        ui.m_RightSide.SetActive(true);
        GUI_Widget_Projection wp = ui.GetComponentInChildren<GUI_Widget_Projection>(true);
        if (wp)
            wp.Initialize();

        yield return new WaitForSecondsRealtime(0.5f);

        // 장식용 2
        ui.m_Ingame_Scene_Loader.Add_Msg("deploy requested");
        ui.m_Ingame_Scene_Loader.Add_Msg("waiting for a response from company");
        yield return new WaitForSecondsRealtime(0.5f);
        ui.m_Ingame_Scene_Loader.Add_Msg("ok");

        // 장식용 3
        ui.m_Ingame_Scene_Loader.Add_Msg("connecting to machine");
        ui.m_Ingame_Scene_Loader.Add_Msg("machine id " + UnityEngine.Random.Range(1, 999999).ToString("D6"));
        yield return new WaitForSecondsRealtime(0.5f);
        ui.m_Ingame_Scene_Loader.Add_Msg("ok");

        // 로딩 끝난 상태, 다른 이들 로딩 기다리기
        ui.m_Ingame_Scene_Loader.Add_Msg("waiting for team");
        Packet_Sender.Send_Protocol((UInt64)PROTOCOL.MNG_INGAME | (UInt64)PROTOCOL_INGAME.READY);
        yield return new WaitForSecondsRealtime(0.5f);

        yield return null;
    }

    /// <summary>
    /// 게임의 첫 시작
    /// </summary>
    /// <param name="_map_id"></param>
    public void Start_Game(int _map_id)
    { 
        m_MapID = _map_id;
        StartCoroutine(Start_Game_Process());
    }
    IEnumerator Start_Game_Process()
    {
        Ingame_UI ui = Ingame_UI.Instance;

        // 호출했던 씬 로더 문장 갱신
        ui.m_Ingame_Scene_Loader.Add_Msg("ok");
        yield return new WaitForSecondsRealtime(0.5f);
        ui.m_Ingame_Scene_Loader.Add_Msg(" ");

        if (m_DebugMode)
            Prepare_Round(m_FakeRound);

        // 인풋 시작
        StartCoroutine(Input_Send());
    }

    /// <summary>
    /// 게임 라운드 갱신
    /// </summary>
    /// <param name="_round"></param>
    public void Prepare_Round(int _round)
    {
        m_Round = _round;
        Add_Delayed_Coroutine(Prepare_Round_Process());
    }
    IEnumerator Prepare_Round_Process()
    {
        Ingame_UI ui = Ingame_UI.Instance;
        ui.m_Ingame_Scene_Loader.Show(true);
        ui.m_Ingame_Scene_Loader.Add_Msg("Loading Next Map...");
        yield return new WaitForSeconds(3.0f);

        // 기존에 존재하던 오브젝트 다 제거하기
        Manager_Network.Log("라운드 오브젝트 제거");
        Clear_Round_Objects();

        // 맵 읽기
        Manager_Network.Log("맵 로드");
        Load_Mapdata(m_MapID, m_Round);

        // 캐릭터 오브젝트 
        Manager_Network.Log("캐릭터 생성");
        Create_PlayerCharacters();

        // 아이템 오브젝트
        Create_Items();

        yield return new WaitForSeconds(2.0f);

        if (m_DebugMode)
            Start_Round();
        else
        {
            Manager_Network.Log("라운드 로딩 완료 프로토콜 송신");
            Packet_Sender.Send_Protocol((UInt64)PROTOCOL.MNG_INGAME | (UInt64)PROTOCOL_INGAME.SESSION | (UInt64)PROTOCOL_INGAME.SS_ROUND_READY);
        }
        Play_Next_Coroutine();

        yield return null;
    }

    public void Start_Round()
    {
        m_Game_Started = true;
        Add_Delayed_Coroutine(Start_Round_Process());
    }
    IEnumerator Start_Round_Process()
    {
        Ingame_UI ui = Ingame_UI.Instance;

        // 로딩창 지우기
        ui.m_Ingame_Scene_Loader.Show(false);
        ui.Lock_Cursor(true);

        yield return new WaitForSeconds(2.0f);

        // 라운드 갱신 GUI 표시
        m_Game_Started = true;
        ui.m_Ingame_Round_Indicator.Start_Round(m_Round);
        Play_Next_Coroutine();

        yield return null;
    }

    public void End_Round()
    {
        Ingame_UI ui = Ingame_UI.Instance;

        m_Game_Started = false;
        ui.Lock_Cursor(false);
        Add_Delayed_Coroutine(End_Round_Process());
        // TODO 이후 바로 prepare round 프로토콜 오므로 그에 대한 처리 할 것
    }
    IEnumerator End_Round_Process()
    {
        Ingame_UI ui = Ingame_UI.Instance;
        ui.m_Ingame_Round_Indicator.End_Round(m_Round);
        yield return new WaitForSeconds(3.0f);
        Play_Next_Coroutine();

        yield return null;
    }

    public void End_Game()
    {
        Ingame_UI.Instance.Lock_Cursor(false);
        Add_Delayed_Coroutine(End_Game_Process());
        // 터졌으면 터진대로 처리해줄 것
    }
    IEnumerator End_Game_Process()
    {
        Ingame_UI ui = Ingame_UI.Instance;
        ui.m_Ingame_Round_Indicator.End_Game();
        yield return new WaitForSeconds(3.0f);
        Quit_Game();
        Play_Next_Coroutine();

        yield return null;
    }

    // 강제로 나가기
    public void Quit_Game()
    {
        Ingame_UI.Instance.Lock_Cursor(false);
        Ingame_UI.Instance.Initialize();
        // Destroy(Ingame_UI.Instance.gameObject);
        m_Game_Started = false;
        if (Manager_Network.Instance != null)
            Manager_Network.Instance.Disconnect();
        SceneManager.LoadScene("Title");
    }

    IEnumerator Input_Send()
    {
        WaitForSecondsRealtime wfsr = new WaitForSecondsRealtime(m_Input_Update_Interval);
        while (true)
        {
            while (m_Game_Started)
            {
                if (m_DebugMode)
                {
                    for (int i = 0; i < m_Profiles.Count; i++)
                    {
                        User_Profile up = m_Profiles[i];
                        if (up.Session_ID == m_Client_Profile.Session_ID)
                            m_Profiles[i].User_Input = Manager_Input.Instance.m_Player_Input;
                    }
                    e_FakeInput.Invoke(m_Profiles.ToArray());
                    yield return wfsr;
                    continue;
                }
                // 입력값 보내기
                // Debug.Log("입력 = " + Manager_Input.Instance.m_Player_Input.Move_X + ", "
                //    + Manager_Input.Instance.m_Player_Input.Move_Y);
                Packet_Sender.Send_Input((UInt64)PROTOCOL.MNG_INGAME | (UInt64)PROTOCOL_INGAME.INPUT,
                    Manager_Input.Instance.m_Player_Input,
                    Manager_Input.Instance.m_Pre_Position);
                yield return wfsr;
            }
            yield return wfsr;
        }
        yield return null;
    }

    public void Load_Mapdata(int _map, int _round)
    {
        GameObject map = Resources.Load<GameObject>("Prefabs/Maps/Map_M" + _map + "_R" + _round);
        GameObject minimap = Resources.Load<GameObject>("Prefabs/Maps/Map_M" + _map + "_R" + _round + "_Minimap");

        GameObject temp_map = Instantiate(map);
        GameObject temp_minimap = Instantiate(minimap);

        Add_Round_Object(temp_map);
        Add_Round_Object(temp_minimap);

        e_RoundUpdate.Invoke(temp_map, temp_minimap.GetComponent<Minimap>());

        temp_minimap.SetActive(false);
    }

    public void Create_PlayerCharacters()
    {
        // 프로필에 맞춰 캐릭터 오브젝트 생성
        foreach (User_Profile profile in m_Profiles)
        {
            Debug.Log("캐릭터 생성 = " + profile.ID + " // 좌표 = " + profile.Current_Pos);

            profile.Round_Init();
            if (profile.ID.Equals(m_Client_Profile.ID))
                m_Client_Profile = profile;

            GameObject player_character = Instantiate(profile.Role_Index == 1 ? prefab_Guard : prefab_Thief);
            player_character.transform.position = profile.Current_Pos;
            CharacterController pc = player_character.GetComponent<CharacterController>();
            if (pc != null)
            {
                // 프로필 심기
                pc.m_MyProfile = profile;

                // 카메라 자신의 캐릭터 찾아가기
                if (profile.Session_ID == m_Client_Profile.Session_ID)
                {
                    pc.Fix_Camera();
                    Ingame_UI.Instance.Set_Player(pc);
                }
            }
            player_character.transform.position = pc.m_MyProfile.Current_Pos;
            Add_Round_Object(player_character);
        }
    }

    public void Get_Items(Item_Data[] _items)
    {
        m_Items = new List<Item_Data>(_items);
    }
    public void Create_Items()
    {
        foreach (Item_Data item in m_Items)
        {
            GameObject item_prefab = Resources.Load<GameObject>("Prefabs/Tools/Tool_" + item.OID);
            GameObject item_object = Instantiate(item_prefab);
            item_object.transform.position = item.Position;
            item_object.transform.rotation = Quaternion.Euler(item.Rotation);
            Add_Round_Object(item_object);
        }
    }

    public void Add_Round_Object(GameObject _obj)
    {
        m_Round_Objects.Add(_obj);
    }
    public void Clear_Round_Objects()
    {
        foreach (GameObject obj in m_Round_Objects)
            Destroy(obj);
        m_Round_Objects.Clear();
    }
}
