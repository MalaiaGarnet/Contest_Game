using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Network.Data;
using UnityEngine.SceneManagement;

public class Manager_Ingame : SingleToneMonoBehaviour<Manager_Ingame>
{
    public List<User_Profile> m_Profiles = new List<User_Profile>();

    IEnumerator Start()
    {
        while (Manager_Network.Instance == null)
            yield return new WaitForEndOfFrame();

        Manager_Network.Instance.e_HeartBeat.AddListener(new UnityAction<User_Profile[]>(Update_Datas));
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
        Ingame_UI.Instance.Show_Ingame_Scene_Loader(true);
        yield return new WaitForSecondsRealtime(2.0f);
        SceneManager.LoadScene("Ingame");
        yield return new WaitForSecondsRealtime(2.0f);
        Ingame_UI.Instance.Show_Ingame_Scene_Loader(false);
        yield return null;
    }
}
