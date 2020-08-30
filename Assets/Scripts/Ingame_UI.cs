using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ingame_UI : SingleToneMonoBehaviour<Ingame_UI>
{
    [Header("곁다리")]
    public GameObject m_Header;
    public GameObject m_Footer;

    [Header("센터 오브젝트들")]
    public GameObject m_Ingame_Scene_Loader;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    public void Show(bool _enable)
    {
        gameObject.SetActive(_enable);
    }

    public void Show_Ingame_Scene_Loader(bool _enable)
    {
        m_Ingame_Scene_Loader.SetActive(_enable);
    }
}
