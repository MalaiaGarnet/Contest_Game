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

    public void Perform_Task(NetworkManager _manager, Task _task)
    {
        UInt64 protocol = 0;
        UnityEngine.Debug.Log("버퍼 정보 - " + BitConverter.ToString(_task.buffer));
        _task.Decrypt(NetworkManager.Instance.m_Encryptor);
        Debug.Log("1");

        GetProtocol(_task.buffer, ref protocol);
        Debug.Log("2");

        _manager.e_ProtocolRecv.Invoke((PROTOCOL)protocol);

        Debug.Log("Main Protocol == " + ((PROTOCOL)protocol).ToString());
        switch ((PROTOCOL)(protocol & 0xffff000000000000))
        {
            case PROTOCOL.DISCONNECT:
                Debug.Log("disconnect");
                _manager.e_Disconnected.Invoke();
                break;
            case PROTOCOL.GLOBAL:
                Debug.Log("global");
                Global_Process(_manager, _task, protocol);
                break;
            case PROTOCOL.MNG_LOGIN:
                Debug.Log("login");
                Login_Process(_manager, _task, protocol);
                break;
            case PROTOCOL.MNG_INGAME:
                Debug.Log("ingame");
                Ingame_Process(_manager, _task, protocol);
                break;
        }
    }

    void Global_Process(NetworkManager _manager, Task _task, UInt64 _protocol)
    {
        if ((_protocol & (UInt64)PROTOCOL_GLOBAL.HEART_BEAT) > 0)
        {
            Debug.Log("heart beat");
            return;
        }
        if ((_protocol & (UInt64)PROTOCOL_GLOBAL.ENCRYPTION_KEY) > 0)
        {
            Debug.Log("encrypt");
            byte[] code = new byte[32];
            Unpacker.UnPackPacket(_task.buffer, ref code);

            Debug.Log("키 대입");
            NetworkManager.Instance.m_Encryptor = new KJH_Crypto(code);
            Debug.Log("암호화 키 취득 " + BitConverter.ToString(code));
            return;
        }
        return;
    }

    void Login_Process(NetworkManager _manager, Task _task, UInt64 _protocol)
    {
        if ((_protocol & (UInt64)PROTOCOL_LOGIN.LOGIN) > 0)
        {
            Debug.Log("로그인 결과 취득");
            bool ok = (_protocol & (UInt64)PROTOCOL_LOGIN.SUCCESS) > 0;
            _manager.e_LoginResult.Invoke(ok);
        }
        if ((_protocol & (UInt64)PROTOCOL_LOGIN.REGISTER) > 0)
        {
            Debug.Log("회원 가입 결과 취득");
            bool ok = (_protocol & (UInt64)PROTOCOL_LOGIN.SUCCESS) > 0;
            _manager.e_RegisterResult.Invoke(ok);
        }
        if ((_protocol & (UInt64)PROTOCOL_LOGIN.MATCHED) > 0)
        {
            return;
        }
    }

    void Ingame_Process(NetworkManager _manager, Task _task, UInt64 _protocol)
    {

    }
}