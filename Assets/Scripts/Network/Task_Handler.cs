using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Events;
using Network.Data;
using System.Text;

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

        Manager_Network.Log("Main Protocol == " + ((PROTOCOL)protocol).ToString());
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
            Manager_Network.Log("heart beat");
            return;
        }
        if ((_protocol & (UInt64)PROTOCOL_GLOBAL.ENCRYPTION_KEY) > 0)
        {
            Manager_Network.Log("encrypt");
            byte[] code = new byte[32];
            Unpacker.UnPackPacket(_task.buffer, ref code);

            Manager_Network.Log("키 대입");
            Manager_Network.Instance.m_Encryptor = new KJH_Crypto(code);
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
        if ((_protocol & (UInt64)PROTOCOL_LOGIN.MATCHED) > 0)
        {
            return;
        }
    }

    void Ingame_Process(Manager_Network _manager, Task _task, UInt64 _protocol)
    {

    }
}