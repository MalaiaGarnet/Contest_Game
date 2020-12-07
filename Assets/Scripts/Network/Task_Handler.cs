using Network.Data;
using System;
using UnityEngine;

/// <summary>
/// 일 처리기
/// 패킷을 받으면 자동적으로 이곳을 통해서 패킷에 대한 처리를 수행함
/// </summary>
public class Task_Handler
{

    // 버퍼에서 프로토콜 얻어내기
    public void GetProtocol(byte[] _data, ref UInt64 _protocol)
    {
        int place = 0;
        _protocol = BitConverter.ToUInt64(_data, place);
        place += sizeof(UInt64);
    }

    public void Perform_Task(Manager_Network _manager, Task _task)
    {
        UInt64 protocol = 0;
        Manager_Network.Log("버퍼 정보 - " + BitConverter.ToString(_task.buffer));
        _task.Decrypt(Manager_Network.Instance.m_Encryptor);
        GetProtocol(_task.buffer, ref protocol);

        _manager.e_ProtocolRecv.Invoke((PROTOCOL)protocol);

        Manager_Network.Log("Main Protocol == " + string.Format("{0:X}", protocol));
        switch ((PROTOCOL)(protocol & 0xffff000000000000))
        {
            case PROTOCOL.DISCONNECT:
                Manager_Network.Log("disconnect");
                _manager.e_Disconnected.Invoke();
                break;
            case PROTOCOL.GLOBAL:
                Manager_Network.Log("global");
                Global_Process(_manager, _task, protocol);
                break;
            case PROTOCOL.MNG_LOGIN:
                Manager_Network.Log("login");
                Login_Process(_manager, _task, protocol);
                break;
            case PROTOCOL.MNG_INGAME:
                Manager_Network.Log("ingame");
                Ingame_Process(_manager, _task, protocol);
                break;
        }
    }

    void Global_Process(Manager_Network _manager, Task _task, UInt64 _protocol)
    {
        if ((_protocol & (UInt64)PROTOCOL_GLOBAL.HEART_BEAT) > 0)
        {
            Manager_Network.Log("global heart beat");
            return;
        }
        if ((_protocol & (UInt64)PROTOCOL_GLOBAL.ENCRYPTION_KEY) > 0)
        {
            Manager_Network.Log("encrypt");
            byte[] code = new byte[8];
            Packet_Unpacker.UnPackPacket(_task.buffer, ref code);

            //Manager_Network.Log("키 대입");
            Manager_Network.Instance.m_Encryptor = new KJH_Crypto_2(code);
            Manager_Network.Log("암호화 키 취득 " + BitConverter.ToString(code));
            return;
        }
        return;
    }

    void Login_Process(Manager_Network _manager, Task _task, UInt64 _protocol)
    {
        if ((_protocol & (UInt64)PROTOCOL_LOGIN.LOGIN) > 0)
        {
            Manager_Network.Log("로그인 결과 취득");
            bool ok = (_protocol & (UInt64)PROTOCOL_LOGIN.SUCCESS) > 0;
            _manager.e_LoginResult.Invoke(ok);
        }
        if ((_protocol & (UInt64)PROTOCOL_LOGIN.REGISTER) > 0)
        {
            Manager_Network.Log("회원 가입 결과 취득");
            bool ok = (_protocol & (UInt64)PROTOCOL_LOGIN.SUCCESS) > 0;
            _manager.e_RegisterResult.Invoke(ok);
        }
        if ((_protocol & (UInt64)PROTOCOL_LOGIN.MATCH) > 0)
        {
            Manager_Network.Log("매치 결과 취득");
            if ((_protocol & (UInt64)PROTOCOL_LOGIN.RESULT) > 0)
            {
                Manager_Network.Log("매치 완료");
                User_Profile[] datas = null;
                Packet_Unpacker.UnPackPacket(_task.buffer, ref datas);
                _manager.e_Matched.Invoke(datas);
            }
            if ((_protocol & (UInt64)PROTOCOL_LOGIN.STOP) > 0)
            {
                Manager_Network.Log("매치 중지");
                _manager.e_Match_Stopped.Invoke();
            }
            return;
        }
    }

    void Ingame_Process(Manager_Network _manager, Task _task, UInt64 _protocol)
    {
        if ((_protocol & (UInt64)PROTOCOL_INGAME.HEARTBEAT) > 0)
        {
            Manager_Network.Log("인게임 하트 비트");
            Session_RoundData round = new Session_RoundData();
            User_Profile[] datas = null;
            Packet_Unpacker.UnPackPacket(_task.buffer, ref round, ref datas);
            _manager.e_HeartBeat.Invoke(round, datas);
        }
        if ((_protocol & (UInt64)PROTOCOL_INGAME.START) > 0)
        {
            Debug.Log("인게임 시작");
            _manager.e_GameStart.Invoke(1);
        }
        if ((_protocol & (UInt64)PROTOCOL_INGAME.END) > 0)
        {
            Debug.Log("인게임 끝");
            ushort end_reason_byte = 0;
            Packet_Unpacker.UnPackPacket(_task.buffer, ref end_reason_byte);
            SESSION_END_REASON end_reason = (SESSION_END_REASON)end_reason_byte;
            _manager.e_GameEnd.Invoke(end_reason);
        }
        if ((_protocol & (UInt64)PROTOCOL_INGAME.SESSION) > 0) // 세션
        {
            if ((_protocol & (UInt64)PROTOCOL_INGAME.SS_ROUND_READY) > 0) // 준비 명령
            {
                Debug.Log("인게임 라운드 준비");
                Session_RoundData roundData = new Session_RoundData();
                Packet_Unpacker.UnPackPacket(_task.buffer, ref roundData);
                _manager.e_RoundReady.Invoke(roundData.Current_Round);
            }
            if ((_protocol & (UInt64)PROTOCOL_INGAME.SS_ROUND_START) > 0) // 시작 명령
            {
                Debug.Log("인게임 라운드 시작");
                _manager.e_RoundStart.Invoke();
            }
            if ((_protocol & (UInt64)PROTOCOL_INGAME.SS_ROUND_END) > 0) // 시작 명령
            {
                Debug.Log("인게임 라운드 종료");
                _manager.e_RoundEnd.Invoke();
            }
            if ((_protocol & (UInt64)PROTOCOL_INGAME.SS_ROUND_SPAWN_ITEMS) > 0)
            {
                Debug.Log("아이템 스폰");
                Item_Data[] datas = null;
                Packet_Unpacker.UnPackPacket(_task.buffer, ref datas);
                _manager.e_ItemSpawn.Invoke(datas);
            }
        }
        if ((_protocol & (UInt64)PROTOCOL_INGAME.ITEM) > 0) // 아이템
        {
            if ((_protocol & (UInt64)PROTOCOL_INGAME.ITEM_GET) > 0) // 아이템 획득
            {
                int instance_id = 0;
                Packet_Unpacker.UnPackPacket(_task.buffer, ref instance_id);
                _manager.e_ItemGet.Invoke(instance_id);
            }
        }
        if ((_protocol & (UInt64)PROTOCOL_INGAME.INPUT) > 0)
        {
            Manager_Network.Log("인게임 인풋");
            User_Profile[] datas = null;
            Packet_Unpacker.UnPackPacket(_task.buffer, ref datas);
            _manager.e_PlayerInput.Invoke(datas);
        }
        if ((_protocol & (UInt64)PROTOCOL_INGAME.SHOT) > 0)
        {
            _protocol -= (UInt64)PROTOCOL.MNG_INGAME;
            _protocol -= (UInt64)PROTOCOL_INGAME.SHOT;
            Debug.Log("인게임 사격");
            if ((_protocol & (UInt64)PROTOCOL_INGAME.SHOT_HIT) > 0)
            {
                _protocol -= (UInt64)PROTOCOL_INGAME.SHOT_HIT;
                Debug.Log("프로토콜 확인: " + _protocol);
                UInt16 id = 0, damage = 0;
                Packet_Unpacker.UnPackPacket(_task.buffer, ref id, ref damage);
                Debug.Log("사격 - 피해: " + id + " // " + damage);
                _manager.e_PlayerHit.Invoke(id, damage);
            }
            if ((_protocol & (UInt64)PROTOCOL_INGAME.SHOT_STUN) > 0)
            {
                UInt16 id = 0, damage = 0;
                Packet_Unpacker.UnPackPacket(_task.buffer, ref id, ref damage);
                Debug.Log("사격 - 스턴: " + id + " // " + damage);
                _manager.e_PlayerStun.Invoke(id, damage);
            }
        }
    }
}