using Network.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Threading;
using UnityEngine;

/// <summary>
/// 네트워크 관리자
/// </summary>
public class Manager_Network : MonoBehaviour
{
    public static Manager_Network Instance;

    public bool m_Connected { get; private set; } // 서버 연결 상태

    public TcpClient m_Socket = null; // TCP소켓
    public KJH_Crypto_2 m_Encryptor = null; // 암호화

    // 멤버
    string m_IP = "127.0.0.1";
    string m_Port = "9000";
    Manager_Packet m_Packet;

    // 이벤트

    // global
    public Event_Disconnected e_Disconnected = new Event_Disconnected(); // 연결 끊어짐
    public Protocol_Recv_Event e_ProtocolRecv = new Protocol_Recv_Event(); // 프로토콜 겟또다제 

    // login
    public Event_Login_Result e_LoginResult = new Event_Login_Result(); // 로그인 시도 시 결과값
    public Event_Register_Result e_RegisterResult = new Event_Register_Result(); // 회원가입 시도 시 결과값
    public Event_Match_Stopped e_Match_Stopped = new Event_Match_Stopped();
    public Event_Matched e_Matched = new Event_Matched();

    // ingame
    public Event_HeartBeat e_HeartBeat = new Event_HeartBeat();
    public Event_Round_Ready e_RoundReady = new Event_Round_Ready();
    public Event_Round_Start e_RoundStart = new Event_Round_Start();
    public Event_Round_End e_RoundEnd = new Event_Round_End();
    public Event_Game_Start e_GameStart = new Event_Game_Start();
    public Event_Game_End e_GameEnd = new Event_Game_End();
    public Event_Player_Input e_PlayerInput = new Event_Player_Input();
    public Event_Player_Hit e_PlayerHit = new Event_Player_Hit();
    public Event_Player_Stun e_PlayerStun = new Event_Player_Stun();
    public Event_Item_Spawn e_ItemSpawn = new Event_Item_Spawn();
    public Event_Item_Get e_ItemGet = new Event_Item_Get();

    public static bool Debug_Toggle = false; // 디버그 로거 표현 여부
    public static void Log(string _msg) // 로그 쓰기
    {
        if (Debug_Toggle)
        {
            string name = System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name;
            Debug.Log("[" + name + "] " + _msg);
        }
    }

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        m_Packet = new Manager_Packet(this);
    }

    private void Update()
    {
        if (m_Connected) // 연결이 된 경우
            m_Packet?.Update();
    }

    public void Change_IP(string _ip) { m_IP = _ip; }
    public void Connect_To_Server()
    {
        if (m_Socket != null)
        {
            m_Socket.GetStream().Close();
            m_Socket.Close();
        }
        m_Socket = new TcpClient();

        try
        {
            // 연결 시도, 실패시 SocketException
            m_Socket.Connect(m_IP, int.Parse(m_Port));
            m_Connected = true;

            // 패킷 핸들러 오브젝트 초기화
            m_Packet.Init();

            // 연결 됐으면 ACK 보내기
            Packet_Sender.Send_Heartbeat();
        }
        catch (SocketException e)
        {
            Log(e.Message);
            e_Disconnected.Invoke();
            m_Socket = null;
        }
    }
    public void Disconnect()
    {
        if (m_Socket != null)
        {
            m_Packet.End_Thread();
            Log("close...");
            m_Socket.Close();
            Log("dispose...");
            m_Socket.Dispose();
        }
        m_Socket = null;
        m_Connected = false;
    }


    /// <summary>
    /// 로그인 시도
    /// </summary>
    /// <returns>실패 시 false</returns>
    public bool Login(string _id, string _pw)
    {
        // 서버 연결
        if (!m_Connected)
            Connect_To_Server();

        // 로그인 패킷 전송
        UInt64 protocol = (UInt64)PROTOCOL.MNG_LOGIN | (UInt64)PROTOCOL_LOGIN.LOGIN;
        Userdata data = new Userdata(_id, _pw, "");
        Packet_Sender.Send_Userdata(protocol, data);

        Manager_Ingame.Instance.m_Client_Profile.ID = _id;

        return true;
    }
    /// <summary>
    /// 회원가입 시도
    /// </summary>
    /// <returns>실패 시 false</returns>
    public bool Register(string _id, string _pw, string _nickname)
    {
        // 서버 연결
        if (!m_Connected)
            Connect_To_Server();

        StartCoroutine(Register_Process(_id, _pw, _nickname));
        return true;
    }
    IEnumerator Register_Process(string _id, string _pw, string _nickname)
    {
        yield return new WaitForSecondsRealtime(2.0f);

        /* 그거
        while (m_Encryptor == null)
            yield return new WaitForEndOfFrame();
        */

        // 회원가입 패킷 전송
        UInt64 protocol = (UInt64)PROTOCOL.MNG_LOGIN | (UInt64)PROTOCOL_LOGIN.REGISTER;
        Userdata data = new Userdata(_id, _pw, _nickname);
        Packet_Sender.Send_Userdata(protocol, data);

        yield return null;
    }
    /// <summary>
    /// 로그아웃 및 연결 끊기
    /// </summary>
    public void Logout()
    {
        if (!m_Connected)
            return;

        // TODO 디스커넥트 패킷 전송
        // UInt64 protocol = (UInt64)PROTOCOL.DISCONNECT;
        // Sender.Send_Protocol(protocol);

        Disconnect();
    }


}

/// <summary>
/// 패킷 관리자, 패킷을 보내거나 받거나 한다
/// 패킷은 Packer에서 먼저 패킹 작업이 이루어져야한다
/// 패킷을 받을 때에도 Packer에서 가공해서 인게임에서 써야 한다
/// </summary>
public class Manager_Packet
{
    public static Manager_Packet Instance;

    public Queue<Task> m_SendQueue; // 서버에게 보내는 패킷 모음, 선입 선출
    public Queue<Task> m_RecvQueue; // 서버에게서 받는 패킷 모음, 선입 선출

    Manager_Network m_NetworkManager; // 모체
    Task_Handler m_Task_Handler;
    Thread t_Receiver;

    public Manager_Packet(Manager_Network _network)
    {
        Instance = this;
        m_NetworkManager = _network;
        m_Task_Handler = new Task_Handler();
        m_SendQueue = new Queue<Task>();
        m_RecvQueue = new Queue<Task>();
    }

    public void Init()
    {
        if (t_Receiver != null)
            t_Receiver.Abort();

        m_SendQueue = new Queue<Task>();
        m_RecvQueue = new Queue<Task>();
        t_Receiver = new Thread(Thread_Recv);
        t_Receiver.Start();
    }

    public void End_Thread()
    {
        if (t_Receiver != null)
            t_Receiver.Abort();
        t_Receiver = null;
    }

    public void Update()
    {
        SendAll();
        RecvAll();
    }

    // Recv 전용 스레드
    void Thread_Recv()
    {
        m_RecvQueue.Clear();
        while (true)
        {
            Task task = new Task(); // 새로운 버퍼를 만들고
            PacketRecv(ref task.buffer, ref task.datasize); // 받고
            task.datasize = (task.datasize / 8 + 1) * 8;
            m_RecvQueue.Enqueue(task); // 받으면 받기 큐에 그것을 투입
        }
    }

    public byte[] SetBuffer(UInt64 _protocol, byte[] _buffer, ref int _size)
    {
        byte[] data = new byte[1024];
        int place = 0;

        Buffer.BlockCopy(BitConverter.GetBytes(_protocol), 0, data, place, sizeof(UInt64));
        place += sizeof(UInt64);

        Buffer.BlockCopy(_buffer, 0, data, place, _buffer.Length);
        place += _buffer.Length;

        _size = place;

        return data;
    }

    #region SEND

    // 프로토콜 포장
    public void PackProtocol(ref UInt64 _protocol, UInt64 __protocol)
    {
        _protocol = _protocol | __protocol;
    }

    public void PacketSend(Task _task)
    {
        NetworkStream ns;

        ns = m_NetworkManager.m_Socket.GetStream();

        ns.Write(_task.buffer, 0, _task.datasize);
    }

    // 보내기 큐에 패킷 투입
    public void SendEnqueue(Task _task)
    {
        m_SendQueue.Enqueue(_task);
    }

    // 보내기 큐를 비울 때까지 계속해서 보내기
    public void SendAll()
    {
        try
        {
            while (m_SendQueue.Count > 0)
            {
                Task task = m_SendQueue.Dequeue();
                PacketSend(task);

                Manager_Network.Log("Sended - " + task.buffer[0] + "/" + task.buffer[1] + "/" + task.buffer[2] + "/" + task.buffer[3]);
            }
        }
        catch (Exception)
        {
            Manager_Network.Log("send error.");
            m_NetworkManager.e_Disconnected.Invoke();
        }
    }

    #endregion

    #region RECV

    // 서버에게서 패킷 받기
    public void PacketRecv(ref byte[] _buf, ref int _size)
    {
        byte[] size = new byte[4];

        NetworkStream ns = m_NetworkManager.m_Socket.GetStream();

        int recv = ns.Read(size, 0, 4);
        _size = BitConverter.ToInt16(size, 0);
        Manager_Network.Log("target size = " + _size);

        recv = ns.Read(_buf, 0, _size);
    }

    // 받기 큐를 비울 때까지 계속해서 받기
    void RecvAll()
    {
        try
        {
            while (m_RecvQueue.Count > 0)
            {
                Task task = new Task();

                task = m_RecvQueue.Dequeue();

                Manager_Network.Log("Received...");
                m_Task_Handler.Perform_Task(m_NetworkManager, task); // 받은 패킷과 프로토콜을 전달, 인게임 요소들에 반영
            }
        }
        catch (Exception)
        {
            m_NetworkManager.e_Disconnected.Invoke();
        }
    }

    #endregion





}
