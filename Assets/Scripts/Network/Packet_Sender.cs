using Network.Data;
using System;
using System.Collections.Generic;
using UnityEngine;

public class Packet_Sender
{
    /// <summary>
    /// 나 살아있다고 알리기
    /// </summary>
    public static void Send_Heartbeat()
    {
        Task task = new Task();
        UInt64 protocol = (UInt64)PROTOCOL.GLOBAL | (UInt64)PROTOCOL_GLOBAL.HEART_BEAT;
        task.buffer = Packet_Packer.PackPacket(ref task.datasize, protocol, "");
        task.Encrypt(Manager_Network.Instance.m_Encryptor);

        Manager_Network.Log("buffer size = " + task.datasize);
        Manager_Packet.Instance.SendEnqueue(task);
    }

    /// <summary>
    /// 프로토콜 보내기
    /// </summary>
    public static void Send_Protocol(UInt64 _protocol)
    {
        Task task = new Task();
        task.buffer = Packet_Packer.PackPacket(ref task.datasize, _protocol);

        task.Encrypt(Manager_Network.Instance.m_Encryptor);
        Manager_Packet.Instance.SendEnqueue(task);
    }

    /// <summary>
    /// 로그인 & 회원가입시의 유저데이터 보내기
    /// </summary>
    public static void Send_Userdata(UInt64 _protocol, Userdata _data)
    {
        Task task = new Task();
        task.buffer = Packet_Packer.PackPacket(ref task.datasize, _protocol, _data);

        task.Encrypt(Manager_Network.Instance.m_Encryptor);
        Manager_Packet.Instance.SendEnqueue(task);
    }

    /// <summary>
    /// 매칭 프로토콜 보내기
    /// </summary>
    public static void Send_Match_Start(UInt16 _Type)
    {
        Task task = new Task();
        UInt64 protocol = (UInt64)PROTOCOL.MNG_LOGIN | (UInt64)PROTOCOL_LOGIN.MATCH | (UInt64)PROTOCOL_LOGIN.START;
        task.buffer = Packet_Packer.PackPacket(ref task.datasize, protocol, _Type);
        task.Encrypt(Manager_Network.Instance.m_Encryptor);

        Manager_Packet.Instance.SendEnqueue(task);
    }

    /// <summary>
    /// 인게임 씬 준비 완료했다고 알리기
    /// </summary>
    public static void Send_Ready()
    {
        Task task = new Task();
        UInt64 protocol = (UInt64)PROTOCOL.MNG_INGAME | (UInt64)PROTOCOL_INGAME.READY;
        task.buffer = Packet_Packer.PackPacket(ref task.datasize, protocol, "");
        task.Encrypt(Manager_Network.Instance.m_Encryptor);

        Manager_Packet.Instance.SendEnqueue(task);
    }

    /// <summary>
    /// 입력 및 추측 데이터 보내기
    /// </summary>
    public static void Send_Input(UInt64 _protocol, User_Input _input, Vector3 _pre_pos)
    {
        Task task = new Task();
        task.buffer = Packet_Packer.PackPacket(ref task.datasize, _protocol, _input, _pre_pos);
        task.Encrypt(Manager_Network.Instance.m_Encryptor);

        Manager_Network.Log("첫 여덟 바이트 = " + BitConverter.ToString(task.buffer));

        Manager_Packet.Instance.SendEnqueue(task);
    }

    /// <summary>
    /// 사격 데이터 보내기
    /// </summary>
    public static void Send_Shot_Fire(UInt64 _protocol, List<UInt16> _ids, List<Vector3> _impact_pos)
    {
        Task task = new Task();
        task.buffer = Packet_Packer.PackPacket(ref task.datasize, _protocol, _ids, _impact_pos);
        task.Encrypt(Manager_Network.Instance.m_Encryptor);

        Manager_Packet.Instance.SendEnqueue(task);
    }

    public static void Send_Item_Get(int _instance_id)
    {
        Task task = new Task();
        UInt64 protocol = (UInt64)PROTOCOL.MNG_INGAME | (UInt64)PROTOCOL_INGAME.ITEM | (UInt64)PROTOCOL_INGAME.ITEM_GET;
        task.buffer = Packet_Packer.PackPacket(ref task.datasize, protocol, _instance_id);
        task.Encrypt(Manager_Network.Instance.m_Encryptor);

        Manager_Packet.Instance.SendEnqueue(task);
    }
}