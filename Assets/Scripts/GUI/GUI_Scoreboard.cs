using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Network.Data;

public class GUI_Scoreboard : MonoBehaviour
{
    public GUI_Element_Profile m_Guard_Profile;
    public GUI_Element_Profile[] m_Rogue_Profiles;

    void Start()
    {
    }

    public void Register_Event()
    {
        Manager_Network.Instance.e_HeartBeat.AddListener(Update_Scoreboard);
    }

    public void Update_Scoreboard(Session_RoundData _round, User_Profile[] _Profiles)
    {
        for (int i = 0, j = 0; i < _Profiles.Length; i++)
        {
            if (_Profiles[i].Role_Index == 1)
                m_Guard_Profile.Update_Profile(_Profiles[i]);
            else
            {
                m_Rogue_Profiles[j].Update_Profile(_Profiles[i]);
                ++j;
            }
        }
    }
}
