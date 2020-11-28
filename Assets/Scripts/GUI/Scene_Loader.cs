using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Scene_Loader : MonoBehaviour
{
    public RectTransform m_Circle;
    public Text m_Loading_Text;

    public void Show(bool _enable)
    {
        if (!gameObject.activeSelf && !_enable)
            return;
        if (gameObject.activeSelf && _enable)
            return;

        if (_enable == true)
        {
            m_Loading_Text.text = "";
            gameObject.SetActive(_enable);
        }
        StartCoroutine(Show_Ingame_Scene_Loader(_enable));
    }

    private void Update()
    {
        m_Circle.localRotation = Quaternion.Euler(m_Circle.localRotation.eulerAngles + new Vector3(0f, 0f, 16f * Time.deltaTime));
    }

    public IEnumerator Show_Ingame_Scene_Loader(bool _enable)
    {
        string anim_name = "GUI_Scene_Loder_Fade_" + (_enable ? "In" : "Out");

        GetComponent<Animation>().Play(anim_name);
        if (!_enable)
        {
            while (GetComponent<Animation>().isPlaying)
                yield return new WaitForEndOfFrame();
            gameObject.SetActive(_enable);
        }
        yield return null;
    }

    public void Add_Msg(string _msg)
    {
        m_Loading_Text.text += "\n" + _msg;
    }
}
