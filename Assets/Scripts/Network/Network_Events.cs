using System;
using System.Diagnostics;
using System.Drawing;
using System.Text;
using UnityEngine;
using UnityEngine.Events;

namespace Network.Data
{
    // 디버그
    public class Event_Debug : UnityEvent { };

    // 글로벌
    public class Event_Disconnected : UnityEvent { };
    public class Protocol_Recv_Event : UnityEvent<PROTOCOL> { };

    // 로그인 부분
    public class Event_Login_Result : UnityEvent<bool> { };
    public class Event_Register_Result : UnityEvent<bool> { };
    public class Event_Match_Stopped : UnityEvent { };
    public class Event_Matched : UnityEvent<User_Profile[]> { };

    // 인게임
    public class Event_HeartBeat : UnityEvent<User_Profile[]> { };
    public class Event_Game_Start : UnityEvent { };
    public class Event_Player_Input: UnityEvent<User_Input> { };

}