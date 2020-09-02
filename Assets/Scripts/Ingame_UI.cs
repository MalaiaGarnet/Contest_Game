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
    public RectTransform m_Circle;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    public void Show(bool _enable)
    {
        gameObject.SetActive(_enable);
    }

    public IEnumerator Show_Ingame_Scene_Loader(bool _enable)
    {
        string anim_name = "GUI_Scene_Loder_Fade_" + (_enable ? "In" : "Out");

        if(_enable)
            m_Ingame_Scene_Loader.SetActive(_enable);

        m_Ingame_Scene_Loader.GetComponent<Animation>().Play(anim_name);

        if (!_enable)
        {
            while (m_Ingame_Scene_Loader.GetComponent<Animation>().isPlaying)
                yield return new WaitForEndOfFrame();
            m_Ingame_Scene_Loader.SetActive(_enable);
        }
        yield return null;
    }
}
