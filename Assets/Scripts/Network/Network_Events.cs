using System;
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
    public class Event_HeartBeat : UnityEvent<Session_RoundData, User_Profile[]> { };
    public class Event_Game_Start : UnityEvent<int> { };
    public class Event_Round_Ready : UnityEvent<int> { };
    public class Event_Round_Start : UnityEvent { };
    public class Event_Item_Spawn : UnityEvent<Item_Data[]> { };
    public class Event_Item_Get : UnityEvent<int> { };
    public class Event_Round_End : UnityEvent { };
    public class Event_Game_End : UnityEvent<SESSION_END_REASON> { };
    public class Event_Player_Input : UnityEvent<User_Profile[]> { };
    public class Event_Player_Hit : UnityEvent<UInt16, UInt16> { };
    public class Event_Player_Stun : UnityEvent<UInt16, UInt16> { };
}