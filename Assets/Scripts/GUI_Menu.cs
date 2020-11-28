using UnityEngine;

public class GUI_Menu : MonoBehaviour
{
    bool is_actavated = false;

    public void Toggle()
    {
        Activate(!is_actavated);
    }

    /// <summary>
    /// 메뉴 여닫기
    /// </summary>
    public void Activate(bool _enable)
    {
        is_actavated = _enable;
        Ingame_UI.Instance.Lock_Cursor(!_enable);
        gameObject.SetActive(_enable);
    }

    public void Button_Resume()
    {
        Activate(false);
    }
    public void Button_Option()
    {
        // TODO 옵션 창 열기
    }
    public void Button_Quit()
    {
        Manager_Ingame.Instance.Quit_Game();
    }
}
