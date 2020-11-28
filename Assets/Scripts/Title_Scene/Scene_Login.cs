using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Scene_Login : MonoBehaviour
{
    public Scene_Title s_Title;
    public Scene_Lobby s_Lobby;
    public Title_RadioCircle m_Circle;

    public Text m_Loading_Text;
    public Image m_Loading_Window;
    public Image m_Loading_Bar;

    string m_ID, m_PW;
    float m_Loading_Progress;
    bool m_Activated;

    IEnumerator Start()
    {
        while (Manager_Network.Instance == null)
            yield return new WaitForEndOfFrame();

        Manager_Network.Instance.e_LoginResult.AddListener(new UnityAction<bool>(When_Get_Login_Result));

        yield return null;
    }

    private void Update()
    {
        float alpha = m_Activated ? 1.0f : 0.0f;
        m_Loading_Window.color = Color.Lerp(m_Loading_Window.color, new Color(0.2f, 0.2f, 0.2f, alpha), 0.05f);
        m_Loading_Bar.color = Color.Lerp(m_Loading_Bar.color, new Color(0.2f, 0.2f, 0.2f, alpha), 0.05f);
        Vector3 vec = Vector3.Lerp(m_Loading_Bar.rectTransform.localScale, new Vector3(m_Loading_Progress, 1f, 1f), 0.04f);
        m_Loading_Bar.rectTransform.localScale = vec;
    }

    IEnumerator Flickering_Text(string _text, bool _loading)
    {
        m_Loading_Text.text = _text;
        yield return new WaitForSecondsRealtime(0.03f);
        m_Loading_Text.text = "";
        yield return new WaitForSecondsRealtime(0.03f);
        m_Loading_Text.text = _text;
        yield return new WaitForSecondsRealtime(0.03f);
        m_Loading_Text.text = "";
        yield return new WaitForSecondsRealtime(0.03f);
        m_Loading_Text.text = _text;

        if (_loading)
        {
            yield return new WaitForSecondsRealtime(0.4f);
            m_Loading_Text.text = _text + ".";
            yield return new WaitForSecondsRealtime(0.4f);
            m_Loading_Text.text = _text + "..";
            yield return new WaitForSecondsRealtime(0.4f);
            m_Loading_Text.text = _text + "...";
        }
        yield return null;
    }
    IEnumerator Flickering_Text_Remove()
    {
        string text = m_Loading_Text.text;
        m_Loading_Text.text = "";
        yield return new WaitForSecondsRealtime(0.03f);
        m_Loading_Text.text = text;
        yield return new WaitForSecondsRealtime(0.03f);
        m_Loading_Text.text = "";
        yield return new WaitForSecondsRealtime(0.03f);
        m_Loading_Text.text = text;
        yield return new WaitForSecondsRealtime(0.03f);
        m_Loading_Text.text = "";

        yield return null;
    }

    /// <summary>
    /// 실질적인 로그인 시도
    /// </summary>
    /// <param name="_id"></param>
    /// <param name="_pw"></param>
    public void Try_Login(string _id, string _pw)
    {
        m_Circle.Level = 3;
        m_Activated = true;
        m_Loading_Progress = 0.0f;
        m_Loading_Bar.rectTransform.localScale = new Vector3(0f, 1f, 1f);

        m_ID = _id;
        m_PW = _pw;
        m_Loading_Text.text = "";
        m_Loading_Text.color = new Color(0.2f, 0.2f, 0.2f, 1f);

        StartCoroutine(Login_Process());
    }
    IEnumerator Login_Process()
    {
        yield return new WaitForSecondsRealtime(1.0f);
        StartCoroutine(Flickering_Text("connect to server", true));
        Manager_Network.Instance.Connect_To_Server();
        m_Loading_Progress = 0.3f;
        yield return new WaitForSecondsRealtime(2.0f);


        if (!Manager_Network.Instance.m_Connected)
        {
            m_Loading_Text.color = new Color(0.5f, 0f, 0f, 1f);
            StartCoroutine(Flickering_Text("aborted\nclosed server", false));
            m_Activated = false;
            yield return new WaitForSecondsRealtime(2.0f);
            m_Circle.Level = 0;
            StartCoroutine(Flickering_Text_Remove());
            yield return new WaitForSecondsRealtime(1.0f);

            s_Title.gameObject.SetActive(true);
            gameObject.SetActive(false);
            yield return null;
        }

        m_Loading_Progress = 0.6f;
        Manager_Network.Instance.Login(m_ID, m_PW);

        yield return null;
    }

    /// <summary>
    /// 서버로부터 로그인 결과를 받았을 때 발동
    /// </summary>
    /// <param name="_result">성공하면 true</param>
    void When_Get_Login_Result(bool _result)
    {
        StartCoroutine(Login_Result_Process(_result));
    }
    IEnumerator Login_Result_Process(bool _result)
    {
        yield return new WaitForSecondsRealtime(2.0f);
        if (_result)
        {
            m_Circle.Level = 6;
            StartCoroutine(Flickering_Text("connected", false));
            m_Loading_Progress = 1.0f;
            yield return new WaitForSecondsRealtime(2.0f);
            m_Activated = false;
            StartCoroutine(Flickering_Text_Remove());
            yield return new WaitForSecondsRealtime(1.0f);

            s_Lobby.gameObject.SetActive(true);
            gameObject.SetActive(false);
        }
        else
        {
            m_Loading_Text.color = new Color(0.5f, 0f, 0f, 1f);
            StartCoroutine(Flickering_Text("aborted\nillegal data", false));
            m_Activated = false;
            yield return new WaitForSecondsRealtime(2.0f);
            StartCoroutine(Flickering_Text_Remove());
            m_Circle.Level = 0;
            yield return new WaitForSecondsRealtime(1.0f);

            s_Title.gameObject.SetActive(true);
            gameObject.SetActive(false);
        }
        yield return null;
    }
}
