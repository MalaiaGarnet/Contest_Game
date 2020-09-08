using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Network.Data;
using UnityEngine.SceneManagement;
using System;

public class Manager_Ingame : SingleToneMonoBehaviour<Manager_Ingame>
{
    public List<User_Profile> m_Profiles = new List<User_Profile>();

    public GameObject prefab_Guard;
    public GameObject prefab_Thief;

    public bool m_Game_Started = false;

    IEnumerator Start()
    {
        while (Manager_Network.Instance == null)
            yield return new WaitForEndOfFrame();

        Manager_Network.Instance.e_HeartBeat.AddListener(new UnityAction<User_Profile[]>(Update_Datas));
        Manager_Network.Instance.e_GameStart.AddListener(new UnityAction(Start_Game));
    }

    public void Update_Datas(User_Profile[] _datas)
    {
        m_Profiles = new List<User_Profile>(_datas);
    }

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

    public void Start_Game()
    {
        StartCoroutine(Start_Game_Process());
    }
    IEnumerator Start_Game_Process()
    { 
        Ingame_UI ui = Ingame_UI.Instance;
        m_Game_Started = true;

        ui.m_Ingame_Scene_Loader.Add_Msg("ok");
        yield return new WaitForSecondsRealtime(0.5f);
        ui.m_Ingame_Scene_Loader.Add_Msg(" ");

        foreach (User_Profile profile in m_Profiles)
        {
            GameObject player_character = Instantiate(profile.Role_Index == 1 ? prefab_Guard : prefab_Thief);
            player_character.transform.position = profile.Current_Pos;
            PlayerController pc = player_character.GetComponent<PlayerController>();
            if (pc != null)
            {
                // 프로필 심기
                pc.m_MyProfile = profile;
            }
            player_character.transform.position = pc.m_MyProfile.Current_Pos;
        }

        // TODO 카메라 자신의 캐릭터 찾아가기


        // 로딩창 지우기
        ui.m_Ingame_Scene_Loader.Show(false);

        // 인풋 시작
        StartCoroutine(Input_Send());
    }

    public float m_Input_Update_Interval = 0.100f; // 인풋 보내는 속도
    IEnumerator Input_Send()
    {
        WaitForSecondsRealtime wfsr = new WaitForSecondsRealtime(m_Input_Update_Interval);
        while(m_Game_Started)
        {
            // 입력값 보내기
            Debug.Log("입력 = " + Manager_Input.Instance.m_Player_Input.Move_X + ", "
                + Manager_Input.Instance.m_Player_Input.Move_Y);
            Packet_Sender.Send_Input((UInt64)PROTOCOL.MNG_INGAME | (UInt64)PROTOCOL_INGAME.INPUT,
                Manager_Input.Instance.m_Player_Input,
                Manager_Input.Instance.m_Pre_Position);
            yield return wfsr;
        }

        yield return null;
    }
}
