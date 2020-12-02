using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Network.Data;

public class GUI_Element_Profile : MonoBehaviour
{
    public Text m_Nick, m_Score;
    public GameObject m_No_Signal;
    public GameObject m_Dead;

    public void Update_Profile(User_Profile _profile)
    {
        m_Nick.text = _profile.ID;
        m_Score.text = "" + _profile.Score;

        if (m_No_Signal)
            m_No_Signal.SetActive(false);
        if (m_Dead)
            m_Dead.SetActive(_profile.HP <= 0);
    }
}
