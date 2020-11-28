using Network.Data;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Match_Window : Small_Window
{
    public Text m_Title;
    public Text m_Desc;

    public bool m_Timer_Enable;
    public int m_Timer;

    void Start()
    {
        Manager_Network.Instance.e_Matched.AddListener(new UnityAction<User_Profile[]>(Finded));
        Manager_Network.Instance.e_Match_Stopped.AddListener(new UnityAction(Stopped_Match));
    }

    public void Start_Match_Security()
    {
        m_CloseButton.interactable = true;
        m_Title.text = "security";
        m_Desc.text = "matching";
        gameObject.SetActive(true);
        m_Timer = 0;
        m_Timer_Enable = true;
        StartCoroutine(Timer_Process(1));
    }
    public void Start_Match_Theft()
    {
        m_CloseButton.interactable = true;
        m_Title.text = "theft";
        m_Desc.text = "matching";
        gameObject.SetActive(true);
        m_Timer = 0;
        m_Timer_Enable = true;
        StartCoroutine(Timer_Process(2));
    }

    public void Stop_Match()
    {
        Packet_Sender.Send_Protocol((UInt64)PROTOCOL.MNG_LOGIN | (UInt64)PROTOCOL_LOGIN.MATCH | (UInt64)PROTOCOL_LOGIN.STOP);
        StartCoroutine(Stop_Process());
    }

    public void Stopped_Match()
    {
        StartCoroutine(Stop_Process());
        Close();
    }
    IEnumerator Stop_Process()
    {
        m_Timer_Enable = false;
        m_CloseButton.interactable = false;
        m_Desc.text = "stopped.";
        yield return new WaitForSecondsRealtime(1.0f);

        Close();
        yield return null;
    }

    public void Finded(User_Profile[] _datas)
    {
        m_CloseButton.interactable = false;

        m_Timer = 0;
        m_Timer_Enable = false;
        m_Desc.text = "finded.";

        Manager_Ingame.Instance.Update_Datas(new Session_RoundData(), _datas);
        Manager_Ingame.Instance.StartCoroutine(Finded_Process());
    }

    IEnumerator Timer_Process(UInt16 _Role)
    {
        while (m_Timer_Enable)
        {
            if (m_Timer == 2)
                Packet_Sender.Send_Match_Start(_Role);

            m_Timer += 1;
            int minute = m_Timer / 60;
            int second = m_Timer - minute * 60;
            m_Desc.text = "matching\n" + minute.ToString("00") + ":" + second.ToString("00");
            yield return new WaitForSecondsRealtime(1.0f);
        }

        yield return null;
    }

    IEnumerator Finded_Process()
    {
        yield return new WaitForSecondsRealtime(0.5f);
        Close();
        Manager_Ingame.Instance.Load_Ingame();
        yield return null;
    }
}
