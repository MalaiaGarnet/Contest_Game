using Network.Data;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

/// <summary> 패킷 제작기 </summary>
public class Packet_Packer
{
    public static byte[] PackPacket(ref int _size, UInt64 _protocol)
    {
        byte[] data = new byte[1024];
        int place = 0;

        place += sizeof(int);

        Buffer.BlockCopy(BitConverter.GetBytes(_protocol), 0, data, place, sizeof(UInt64));
        place += sizeof(UInt64);
        _size += sizeof(UInt64);

        place = 0;
        Buffer.BlockCopy(BitConverter.GetBytes(_size), 0, data, place, sizeof(int));

        _size += sizeof(int);

        return data;
    }
    public static byte[] PackPacket(ref int _size, UInt64 _protocol, string _string)
    {
        byte[] data = new byte[1024];
        int place = 0;

        int NickNamesize = _string.Length * 2;

        place += sizeof(int);

        Buffer.BlockCopy(BitConverter.GetBytes(_protocol), 0, data, place, sizeof(UInt64));
        place += sizeof(UInt64);
        _size += sizeof(UInt64);

        Buffer.BlockCopy(BitConverter.GetBytes(NickNamesize), 0, data, place, sizeof(int));
        place += sizeof(int);
        _size += sizeof(int);

        Buffer.BlockCopy(Encoding.Unicode.GetBytes(_string), 0, data, place, _string.Length * 2);
        place += NickNamesize;
        _size += NickNamesize;

        place = 0;

        Buffer.BlockCopy(BitConverter.GetBytes(_size), 0, data, place, sizeof(int));

        _size += sizeof(int);

        return data;
    }
    public static byte[] PackPacket(ref int _size, UInt64 _protocol, UInt64 _data)
    {
        byte[] data = new byte[1024];
        int place = 0;

        place += sizeof(int);

        Buffer.BlockCopy(BitConverter.GetBytes(_protocol), 0, data, place, sizeof(UInt64));
        place += sizeof(UInt64);
        _size += sizeof(UInt64);

        Buffer.BlockCopy(BitConverter.GetBytes(_data), 0, data, place, sizeof(UInt64));
        place += sizeof(UInt64);
        _size += sizeof(UInt64);

        place = 0;

        Buffer.BlockCopy(BitConverter.GetBytes(_size), 0, data, place, sizeof(int));

        _size += sizeof(int);

        return data;
    }
    public static byte[] PackPacket(ref int _size, UInt64 _protocol, ushort _data)
    {
        byte[] data = new byte[1024];
        int place = 0;

        place += sizeof(int);

        Buffer.BlockCopy(BitConverter.GetBytes(_protocol), 0, data, place, sizeof(UInt64));
        place += sizeof(UInt64);
        _size += sizeof(UInt64);

        Buffer.BlockCopy(BitConverter.GetBytes(_data), 0, data, place, sizeof(ushort));
        place += sizeof(ushort);
        _size += sizeof(ushort);

        place = 0;

        Buffer.BlockCopy(BitConverter.GetBytes(_size), 0, data, place, sizeof(int));

        _size += sizeof(int);

        //  _size = (_size / 8 + 1) * 8;

        return data;
    }
    public static byte[] PackPacket(ref int _size, UInt64 _protocol, int _data)
    {
        byte[] data = new byte[1024];
        int place = 0;

        place += sizeof(int);

        Buffer.BlockCopy(BitConverter.GetBytes(_protocol), 0, data, place, sizeof(UInt64));
        place += sizeof(UInt64);
        _size += sizeof(UInt64);

        Buffer.BlockCopy(BitConverter.GetBytes(_data), 0, data, place, sizeof(int));
        place += sizeof(int);
        _size += sizeof(int);

        place = 0;

        Buffer.BlockCopy(BitConverter.GetBytes(_size), 0, data, place, sizeof(int));

        _size += sizeof(int);

        return data;
    }
    public static byte[] PackPacket(ref int _size, UInt64 _protocol, UInt64 _x, UInt64 _y)
    {
        byte[] data = new byte[1024];
        int place = 0;

        place += sizeof(int);

        Buffer.BlockCopy(BitConverter.GetBytes(_protocol), 0, data, place, sizeof(UInt64));
        place += sizeof(UInt64);
        _size += sizeof(UInt64);

        Buffer.BlockCopy(BitConverter.GetBytes(_x), 0, data, place, sizeof(UInt64));
        place += sizeof(UInt64);
        _size += sizeof(UInt64);

        Buffer.BlockCopy(BitConverter.GetBytes(_y), 0, data, place, sizeof(UInt64));
        place += sizeof(UInt64);
        _size += sizeof(UInt64);

        place = 0;

        Buffer.BlockCopy(BitConverter.GetBytes(_size), 0, data, place, sizeof(int));

        _size += sizeof(int);

        return data;
    }
    public static byte[] PackPacket(ref int _size, UInt64 _protocol, Int16 _x, Int16 _y)
    {
        byte[] data = new byte[1024];
        int place = 0;

        place += sizeof(int);

        Buffer.BlockCopy(BitConverter.GetBytes(_protocol), 0, data, place, sizeof(UInt64));
        place += sizeof(UInt64);
        _size += sizeof(UInt64);

        Buffer.BlockCopy(BitConverter.GetBytes(_x), 0, data, place, sizeof(Int16));
        place += sizeof(Int16);
        _size += sizeof(Int16);

        Buffer.BlockCopy(BitConverter.GetBytes(_y), 0, data, place, sizeof(Int16));
        place += sizeof(Int16);
        _size += sizeof(Int16);

        place = 0;

        Buffer.BlockCopy(BitConverter.GetBytes(_size), 0, data, place, sizeof(int));

        _size += sizeof(int);

        return data;
    }
    public static byte[] PackPacket(ref int _size, UInt64 _protocol, Userdata _data)
    {
        byte[] data = new byte[1024];
        int place = 0;

        place += sizeof(int);

        Buffer.BlockCopy(BitConverter.GetBytes(_protocol), 0, data, place, sizeof(UInt64));
        place += sizeof(UInt64);
        _size += sizeof(UInt64);

        Buffer.BlockCopy(Encoding.Unicode.GetBytes(_data.id), 0, data, place, 64);
        place += 64;
        _size += 64;

        Buffer.BlockCopy(Encoding.Unicode.GetBytes(_data.pw), 0, data, place, 64);
        place += 64;
        _size += 64;

        Buffer.BlockCopy(Encoding.Unicode.GetBytes(_data.nickname), 0, data, place, 64);
        place += 64;
        _size += 64;

        place = 0;

        Buffer.BlockCopy(BitConverter.GetBytes(_size), 0, data, place, sizeof(int));

        _size += sizeof(int);

        return data;
    }
    public static byte[] PackPacket(ref int _size, UInt64 _protocol, User_Input _input, Vector3 _pre_pos)
    {
        byte[] data = new byte[1024];
        int place = 0;

        place += sizeof(int);

        // protocol
        Buffer.BlockCopy(BitConverter.GetBytes(_protocol), 0, data, place, sizeof(UInt64));
        place += sizeof(UInt64);
        _size += sizeof(UInt64);

        // input
        _input.Write_Bytes(ref data, ref place, ref _size);

        // pre_position
        Buffer.BlockCopy(BitConverter.GetBytes(_pre_pos.x), 0, data, place, sizeof(float));
        place += sizeof(float); _size += sizeof(float);
        Buffer.BlockCopy(BitConverter.GetBytes(_pre_pos.y), 0, data, place, sizeof(float));
        place += sizeof(float); _size += sizeof(float);
        Buffer.BlockCopy(BitConverter.GetBytes(_pre_pos.z), 0, data, place, sizeof(float));
        place += sizeof(float); _size += sizeof(float);

        place = 0;

        Buffer.BlockCopy(BitConverter.GetBytes(_size), 0, data, place, sizeof(int));

        _size += sizeof(int);

        return data;
    }

    public static byte[] PackPacket(ref int _size, UInt64 _protocol, List<UInt16> _ids, List<Vector3> _impact_pos)
    {
        byte[] data = new byte[1024];
        int place = 0;

        place += sizeof(int);

        // protocol
        Buffer.BlockCopy(BitConverter.GetBytes(_protocol), 0, data, place, sizeof(UInt64));
        place += sizeof(UInt64);
        _size += sizeof(UInt64);

        // arr size
        Buffer.BlockCopy(BitConverter.GetBytes(_ids.Count), 0, data, place, sizeof(Int32));
        place += sizeof(Int32);
        _size += sizeof(Int32);

        for (int i = 0; i < _ids.Count; i++)
        {
            // id
            Buffer.BlockCopy(BitConverter.GetBytes(_ids[i]), 0, data, place, sizeof(UInt16));
            place += sizeof(UInt16); _size += sizeof(UInt16);

            // impact pos position
            Buffer.BlockCopy(BitConverter.GetBytes(_impact_pos[i].x), 0, data, place, sizeof(float));
            place += sizeof(float); _size += sizeof(float);
            Buffer.BlockCopy(BitConverter.GetBytes(_impact_pos[i].y), 0, data, place, sizeof(float));
            place += sizeof(float); _size += sizeof(float);
            Buffer.BlockCopy(BitConverter.GetBytes(_impact_pos[i].z), 0, data, place, sizeof(float));
            place += sizeof(float); _size += sizeof(float);
        }

        place = 0;
        Buffer.BlockCopy(BitConverter.GetBytes(_size), 0, data, place, sizeof(int));

        _size += sizeof(int);

        return data;
    }

}

