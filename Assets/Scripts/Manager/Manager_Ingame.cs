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
        ui.StartCoroutine(ui.Show_Ingame_Scene_Loader(true));
        yield return new WaitForSecondsRealtime(1.0f);
        ui.m_Header.SetActive(true);
        ui.m_Footer.SetActive(true);

        // 인게인 씬 로드
        SceneManager.LoadScene("Ingame");
        yield return new WaitForSecondsRealtime(2.0f);

        // 로딩 끝난 상태, 다른 이들 로딩 기다리기
        Sender.Send_Protocol((UInt64)PROTOCOL.MNG_INGAME | (UInt64)PROTOCOL_INGAME.READY);

        yield return null;
    }

    public void Start_Game()
    {
        Ingame_UI ui = Ingame_UI.Instance;
        
        // 자신의 캐릭터로 이동

        // 로딩창 지우기
        ui.StartCoroutine(ui.Show_Ingame_Scene_Loader(false));
    }
}
