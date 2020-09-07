using System;
using System.Text;
using Network.Data;
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

        place += sizeof(UInt16);

        _short = BitConverter.ToUInt16(_data, place);
        place += sizeof(UInt16);
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
    public static void UnPackPacket(byte[] _data, ref UInt16 _tile_key, ref UInt16 _player_index)
    {
        int place = 0;

        place += sizeof(UInt64);

        _tile_key = BitConverter.ToUInt16(_data, place);
        place += sizeof(UInt16);

        _player_index = BitConverter.ToUInt16(_data, place);
        place += sizeof(UInt16);
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
}

