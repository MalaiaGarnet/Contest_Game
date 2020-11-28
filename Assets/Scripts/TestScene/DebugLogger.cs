using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DebugLogger : SingleToneMonoBehaviour<DebugLogger>
{
    public GameObject m_Contents;
    public Text m_DebugText;
    public Queue<string> texts = new Queue<string>();

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    private void Update()
    {
        /*
        if (Input.GetKeyDown(KeyCode.F1))
            m_Contents.SetActive(!m_Contents.activeSelf);
        if (Input.GetKeyDown(KeyCode.F2))
        {
            texts = new Queue<string>();
            m_DebugText.text = "";
        }
        */
    }

    public void AddText(string _text)
    {
        texts.Enqueue(_text);
        if (texts.Count > 30)
            texts.Dequeue();

        Queue<string> temp_texts = new Queue<string>(texts);

        m_DebugText.text = "";
        while (temp_texts.Count > 0)
            m_DebugText.text += temp_texts.Dequeue() + "\n";

    }
}
