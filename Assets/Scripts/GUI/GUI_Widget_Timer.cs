using Network.Data;
using UnityEngine.Events;
using UnityEngine.UI;

public class GUI_Widget_Timer : GUI_Widget_Base
{
    public Text m_Timer;

    private void Start()
    {
        if (Manager_Network.Instance != null)
            Manager_Network.Instance.e_HeartBeat.AddListener(new UnityAction<Session_RoundData, User_Profile[]>(Update_Timer));
    }

    public void Update_Timer(Session_RoundData _round, User_Profile[] profiles)
    {
        ulong minute = _round.Time_Left / 1000 / 60;
        ulong second = _round.Time_Left / 1000 - minute * 60;
        m_Timer.text = "time left\n" + minute.ToString() + ":" + second.ToString();
    }
}
