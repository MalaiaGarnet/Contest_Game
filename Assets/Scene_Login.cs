using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System;

public class Scene_Login : MonoBehaviour
{
    public Scene_Title s_Title;
    public Scene_Lobby s_Lobby;

    public RectTransform[] m_Circles;
    public Text m_Loading_Text;

    bool m_Activated = false;
    string m_ID, m_PW;

    IEnumerator Start()
    {
        while (Manager_Network.Instance == null)
            yield return new WaitForEndOfFrame();

        Manager_Network.Instance.e_LoginResult.AddListener(new UnityAction<bool>(When_Get_Login_Result));

        yield return null;
    }

    private void Update()
    {
        for (int i = 0; i < m_Circles.Length; i++)
        {
            Image circle_image = m_Circles[i].gameObject.GetComponent<Image>();
            Color color = circle_image.color;
            circle_image.color = Color.Lerp(color, new Color(0.2f, 0.2f, 0.2f, m_Activated ? 0.5f : 0.0f), m_Activated ? 0.002f : 0.005f);

            float turning_angle = 0.01f * (i % 2 == 0 ? 1f : -1f);
            Vector3 rot = m_Circles[i].localRotation.eulerAngles;
            m_Circles[i].localRotation = Quaternion.Euler(rot + new Vector3(0f, 0f, turning_angle));
        }
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
        m_Activated = true;
        for (int i = 0; i < m_Circles.Length; i++)
            m_Circles[i].gameObject.GetComponent<Image>().color = new Color(0.2f, 0.2f, 0.2f, 0f);
        
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
        yield return new WaitForSecondsRealtime(2.0f);

        Manager_Network.Instance.Connect_To_Server();

        if (!Manager_Network.Instance.m_Connected)
        {
            m_Loading_Text.color = new Color(0.5f, 0f, 0f, 1f);
            StartCoroutine(Flickering_Text("aborted\nclosed server", false));
            yield return new WaitForSecondsRealtime(2.0f);
            m_Activated = false;
            StartCoroutine(Flickering_Text_Remove());
            yield return new WaitForSecondsRealtime(1.0f);

            s_Title.gameObject.SetActive(true);
            gameObject.SetActive(false);
            yield return null;
        }
        StartCoroutine(Flickering_Text("connected", false));
        yield return new WaitForSecondsRealtime(1.0f);

        StartCoroutine(Flickering_Text("trying login", true));
        yield return new WaitForSecondsRealtime(1.0f);

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
            StartCoroutine(Flickering_Text("login successed", false));
            yield return new WaitForSecondsRealtime(1.0f);
            StartCoroutine(Flickering_Text_Remove());
            m_Activated = false;
            yield return new WaitForSecondsRealtime(1.0f);

            s_Lobby.gameObject.SetActive(true);
            gameObject.SetActive(false);
        }
        else
        {
            m_Loading_Text.color = new Color(0.5f, 0f, 0f, 1f);
            StartCoroutine(Flickering_Text("login denied", false));
            yield return new WaitForSecondsRealtime(2.0f);
            StartCoroutine(Flickering_Text_Remove());
            m_Activated = false;
            yield return new WaitForSecondsRealtime(1.0f);

            s_Title.gameObject.SetActive(true);
            gameObject.SetActive(false);
        }
        yield return null;
    }
}
