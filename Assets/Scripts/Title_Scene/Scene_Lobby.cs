using System.Collections;
using UnityEngine;

public class Scene_Lobby : SingleToneMonoBehaviour<Scene_Lobby>
{
    public Scene_Title s_Title;

    public Match_Window w_MatchWindow;
    public Small_Window w_ProfileWindow;

    public Title_RadioCircle m_Circle;
    public Animation m_Animation;


    bool m_Working = false;

    // Start is called before the first frame update
    void OnEnable()
    {
        m_Animation.clip = m_Animation.GetClip("GUI_Scene_Lobby_Fade_In");
        m_Animation.Play();
    }

    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Match_Security()
    {
        w_MatchWindow.Start_Match_Security();
    }
    public void Match_Theft()
    {
        w_MatchWindow.Start_Match_Theft();
    }
    public void Open_Profile()
    {
        w_ProfileWindow.gameObject.SetActive(true);
    }

    public void Logout()
    {
        if (m_Working)
            return;
        m_Working = true;
        StartCoroutine(Logout_Process());
    }
    IEnumerator Logout_Process()
    {
        m_Circle.Level = 3;
        Manager_Network.Instance.Logout();
        m_Animation.clip = m_Animation.GetClip("GUI_Scene_Lobby_Fade_Out");
        m_Animation.Play();

        yield return new WaitForSecondsRealtime(m_Animation.clip.length);

        m_Circle.Level = 0;
        m_Working = false;
        s_Title.gameObject.SetActive(true);
        gameObject.SetActive(false);
        yield return null;
    }
}
