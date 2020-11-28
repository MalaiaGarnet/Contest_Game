using UnityEngine.UI;

public class GUI_Widget_HP : GUI_Widget_Base
{
    public Text m_Text;
    int hp = 0;

    void Update()
    {
        if (m_Player == null)
            return;

        if (hp != m_Player.m_MyProfile.HP)
        {
            hp = m_Player.m_MyProfile.HP;
            m_Text.text = "" + hp;
        }
    }
}
