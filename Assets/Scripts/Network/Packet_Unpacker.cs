using Network.Data;
using System;
using System.Runtime.InteropServices;
using System.Text;
using UnityEngine;

/// <summary> 패킷 분해기 </summary>
public class Packet_Unpacker
{
    public static void UnPackPacket(byte[] _data, ref string _msg)
    {
        int msgSize = 0;

        int place = 0;

        place += sizeof(UInt64);

        msgSize = BitConverter.ToInt32(_data, place);
        place += sizeof(int);

        _msg = Encoding.Unicode.GetString(_data, place, msgSize);
        place += msgSize;
    }
    public static void UnPackPacket(byte[] _data, ref byte[] _bytecode)
    {
        int place = sizeof(UInt64);

        _bytecode = new byte[32];
        for (int i = 0; i < 32; i++)
            _bytecode[i] = _data[place + i];
    }
    public static void UnPackPacket(byte[] _data, ref UInt16 _short)
    {
        int place = 0;

        place += sizeof(UInt64);

        _short = BitConverter.ToUInt16(_data, place);
        place += sizeof(UInt16);
    }
    public static void UnPackPacket(byte[] _data, ref int _int)
    {
        int place = 0;

        place += sizeof(UInt64);

        _int = BitConverter.ToInt32(_data, place);
        place += sizeof(int);
    }
    public static void UnPackPacket(byte[] _data, ref UInt64 _int)
    {
        int place = 0;

        place += sizeof(UInt64);

        _int = BitConverter.ToUInt64(_data, place);
        place += sizeof(UInt64);
    }
    public static void UnPackPacket(byte[] _data, ref UInt16 _short, ref UInt64 _int)
    {
        int place = 0;

        place += sizeof(UInt64);

        _short = BitConverter.ToUInt16(_data, place);
        place += sizeof(UInt16);

        _int = BitConverter.ToUInt64(_data, place);
        place += sizeof(UInt64);
    }
    public static void UnPackPacket(byte[] _data, ref UInt16 _session_id, ref UInt16 _damage)
    {
        int place = 0;

        place += sizeof(UInt64);
        Debug.Log("기묘한 값 - " + BitConverter.ToString(_data));

        UInt64 id = BitConverter.ToUInt64(_data, place);
        _session_id = (UInt16)id;
        place += sizeof(UInt64);

        UInt64 damage = BitConverter.ToUInt64(_data, place);
        _damage = (UInt16)damage;
        place += sizeof(UInt64);
    }
    public static void UnPackPacket(byte[] _data, ref UInt16 _player_index, ref UInt64 _x, ref UInt64 _y)
    {
        int place = 0;

        place += sizeof(UInt64);

        _player_index = BitConverter.ToUInt16(_data, place);
        place += sizeof(UInt16);

        _x = BitConverter.ToUInt64(_data, place);
        place += sizeof(UInt64);

        _y = BitConverter.ToUInt64(_data, place);
        place += sizeof(UInt64);
    }
    public static void UnPackPacket(byte[] _data, ref bool _is_client, ref bool _is_correct)
    {
        int place = 0;

        place += sizeof(UInt64);

        _is_client = BitConverter.ToInt16(_data, place) == 1;
        place += sizeof(short);

        _is_correct = BitConverter.ToInt16(_data, place) == 1;
        place += sizeof(short);
    }
    public static void UnPackPacket(byte[] _data, ref User_Profile[] _datas)
    {
        int place = 0;
        UInt16 strlen = 0;
        place += sizeof(UInt64); // 프로토콜 점프

        UInt64 array_length = BitConverter.ToUInt64(_data, place); // 배열 길이 취득
        place += sizeof(UInt64);

        DebugLogger.Instance.AddText("<array length = " + array_length + " >");
        _datas = new User_Profile[array_length];
        for (uint i = 0; i < array_length; i++)
        {
            _datas[i] = new User_Profile();
            _datas[i].Session_ID = BitConverter.ToUInt16(_data, place);
            place += sizeof(UInt16);

            strlen = BitConverter.ToUInt16(_data, place);
            place += sizeof(UInt16);
            _datas[i].ID = Encoding.Unicode.GetString(_data, place, strlen);
            place += strlen;
            strlen = BitConverter.ToUInt16(_data, place);
            place += sizeof(UInt16);
            _datas[i].Nickname = Encoding.Unicode.GetString(_data, place, strlen);
            place += strlen;
            _datas[i].Role_Index = BitConverter.ToUInt16(_data, place);
            place += sizeof(UInt16);
            _datas[i].Is_Ready = BitConverter.ToBoolean(_data, place);
            place += sizeof(bool);

            // HP, Battery, Score
            _datas[i].HP = BitConverter.ToUInt16(_data, place);
            place += sizeof(ushort);
            _datas[i].Battery = BitConverter.ToUInt16(_data, place);
            place += sizeof(ushort);
            _datas[i].Score = BitConverter.ToUInt16(_data, place);
            place += sizeof(ushort);

            // 포지션
            float x = BitConverter.ToSingle(_data, place);
            place += sizeof(float);
            float y = BitConverter.ToSingle(_data, place);
            place += sizeof(float);
            float z = BitConverter.ToSingle(_data, place);
            place += sizeof(float);
            _datas[i].Current_Pos = new Vector3(x, y, z);

            // 로테이션
            x = BitConverter.ToSingle(_data, place);
            place += sizeof(float);
            y = BitConverter.ToSingle(_data, place);
            place += sizeof(float);
            z = BitConverter.ToSingle(_data, place);
            place += sizeof(float);
            _datas[i].Current_Rot = new Vector3(x, y, z);

            // 인풋
            _datas[i].User_Input.Read_Bytes(_data, ref place);

            // 툴
            _datas[i].Current_Tool = BitConverter.ToUInt16(_data, place);
            place += sizeof(UInt16);
            _datas[i].Tool_1 = BitConverter.ToUInt16(_data, place);
            place += sizeof(UInt16);
            _datas[i].Tool_2 = BitConverter.ToUInt16(_data, place);
            place += sizeof(UInt16);
            _datas[i].Tool_3 = BitConverter.ToUInt16(_data, place);
            place += sizeof(UInt16);
            _datas[i].Tool_4 = BitConverter.ToUInt16(_data, place);
            place += sizeof(UInt16);

            _datas[i].m_Using_Skill = BitConverter.ToBoolean(_data, place);
            place += sizeof(bool);
        }
    }
    public static void UnPackPacket(byte[] _data, ref Item_Data[] _datas)
    {
        Debug.Log("버퍼 정보 - " + BitConverter.ToString(_data));

        int place = 0;
        UInt16 strlen = 0;
        place += sizeof(UInt64); // 프로토콜 점프

        UInt64 array_length = BitConverter.ToUInt64(_data, place); // 배열 길이 취득
        place += sizeof(UInt64);

        _datas = new Item_Data[array_length];
        int size = Marshal.SizeOf(new Item_Data());
        // Debug.Log("아이템 자료형 크기 = " + size);
        for (int i = 0; i < (int)array_length; i++)
        {
            _datas[i] = new Item_Data();
            _datas[i].Read_Bytes(_data, ref place);

            // Debug.Log("아이템 id = " + _datas[i].OID + "\n위치 = " + _datas[i].Position + "\n회전 = " + _datas[i].Rotation);
        }
    }
    public static void UnPackPacket(byte[] _data, ref Session_RoundData _roundData)
    {
        int place = 0;
        place += sizeof(UInt64); // 프로토콜 점프

        _roundData.Current_Round = BitConverter.ToUInt16(_data, place); // 현재 라운드
        place += sizeof(UInt16);
        _roundData.Time_Left = BitConverter.ToUInt64(_data, place); // 남은 시간
        place += sizeof(UInt64);
    }
    public static void UnPackPacket(byte[] _data, ref Session_RoundData _roundData, ref User_Profile[] _datas)
    {
        int place = 0;
        UInt16 strlen = 0;
        place += sizeof(UInt64); // 프로토콜 점프

        ushort cur_round = BitConverter.ToUInt16(_data, place); // 라운드 데이터
        place += sizeof(UInt16);
        UInt64 time_left = BitConverter.ToUInt64(_data, place); // 라운드 데이터
        place += sizeof(UInt64);
        _roundData.Current_Round = cur_round;
        _roundData.Time_Left = time_left;

        UInt64 array_length = BitConverter.ToUInt64(_data, place); // 배열 길이 취득
        place += sizeof(UInt64);

        DebugLogger.Instance.AddText("<array length = " + array_length + " >");
        _datas = new User_Profile[array_length];
        for (uint i = 0; i < array_length; i++)
        {
            _datas[i] = new User_Profile();
            _datas[i].Session_ID = BitConverter.ToUInt16(_data, place);
            place += sizeof(UInt16);

            strlen = BitConverter.ToUInt16(_data, place);
            place += sizeof(UInt16);
            _datas[i].ID = Encoding.Unicode.GetString(_data, place, strlen);
            place += strlen;
            strlen = BitConverter.ToUInt16(_data, place);
            place += sizeof(UInt16);
            _datas[i].Nickname = Encoding.Unicode.GetString(_data, place, strlen);
            place += strlen;
            _datas[i].Role_Index = BitConverter.ToUInt16(_data, place);
            place += sizeof(UInt16);
            _datas[i].Is_Ready = BitConverter.ToBoolean(_data, place);
            place += sizeof(bool);

            // HP, Battery, Score
            _datas[i].HP = BitConverter.ToUInt16(_data, place);
            place += sizeof(ushort);
            _datas[i].Battery = BitConverter.ToUInt16(_data, place);
            place += sizeof(ushort);
            _datas[i].Score = BitConverter.ToUInt16(_data, place);
            place += sizeof(ushort);

            // 포지션
            float x = BitConverter.ToSingle(_data, place);
            place += sizeof(float);
            float y = BitConverter.ToSingle(_data, place);
            place += sizeof(float);
            float z = BitConverter.ToSingle(_data, place);
            place += sizeof(float);
            _datas[i].Current_Pos = new Vector3(x, y, z);

            // 로테이션
            x = BitConverter.ToSingle(_data, place);
            place += sizeof(float);
            y = BitConverter.ToSingle(_data, place);
            place += sizeof(float);
            z = BitConverter.ToSingle(_data, place);
            place += sizeof(float);
            _datas[i].Current_Rot = new Vector3(x, y, z);

            // 인풋
            _datas[i].User_Input.Read_Bytes(_data, ref place);

            // 툴
            _datas[i].Current_Tool = BitConverter.ToUInt16(_data, place);
            place += sizeof(UInt16);
            _datas[i].Tool_1 = BitConverter.ToUInt16(_data, place);
            place += sizeof(UInt16);
            _datas[i].Tool_2 = BitConverter.ToUInt16(_data, place);
            place += sizeof(UInt16);
            _datas[i].Tool_3 = BitConverter.ToUInt16(_data, place);
            place += sizeof(UInt16);
            _datas[i].Tool_4 = BitConverter.ToUInt16(_data, place);
            place += sizeof(UInt16);

            _datas[i].m_Using_Skill = BitConverter.ToBoolean(_data, place);
            place += sizeof(bool);
        }
    }
}

