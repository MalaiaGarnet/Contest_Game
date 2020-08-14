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

    // 인게임
    public class Event_Matched : UnityEvent<UInt64> { };
    public class Event_Game_Start : UnityEvent<UInt64> { };
    public class Event_Knockdown : UnityEvent<UInt64> { };
    public class Event_Game_End : UnityEvent<UInt64> { };

}