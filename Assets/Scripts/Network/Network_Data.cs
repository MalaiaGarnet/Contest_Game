using System;
using System.Diagnostics;
using System.Drawing;
using System.Text;
using UnityEngine;
using UnityEngine.Events;

namespace Network.Data
{
    /// <summary>
    /// 버퍼 데이터
    /// </summary>
    public class Task
    {
        public byte[] buffer = new byte[4096];
        public int datasize = 0;

        public void Encrypt(KJH_Crypto _encryptor)
        {
            if (_encryptor == null)
                return;

            datasize = (datasize / 8 + 1) * 8;

            // 버퍼의 내용에서 사이즈 빼서 옮겨 담기
            byte[] temp_byte = new byte[datasize];
            Buffer.BlockCopy(buffer, 4, temp_byte, 0, datasize);

            // 내용 암호화
            byte[] encrypted = null;
            _encryptor.Encrypt(ref temp_byte, datasize, ref encrypted);

            // 암호화 한 것을 버퍼로 다시 옮기기
            Buffer.BlockCopy(encrypted, 0, buffer, 4, datasize);
            Buffer.BlockCopy(BitConverter.GetBytes(datasize), 0, buffer, 0, sizeof(int));
            datasize += sizeof(int);
            Manager_Network.Log("암호화 된 버퍼 크기 = " + datasize);
            Manager_Network.Log("암호화 수행");
        }
        public void Decrypt(KJH_Crypto _decryptor)
        {
            if (_decryptor == null)
                return;

            Manager_Network.Log("복호화 수행 크기 = " + datasize);

            // 버퍼의 내용에서 사이즈 빼서 옮겨 담기
            byte[] temp_byte = new byte[datasize];
            Buffer.BlockCopy(buffer, 0, temp_byte, 0, datasize);

            // 내용 복호화
            byte[] decrypted = null;
            _decryptor.Decrypt(ref temp_byte, datasize, ref decrypted);

            // 복호화 한 것을 버퍼로 다시 옮기기
            Buffer.BlockCopy(decrypted, 0, buffer, 0, datasize);
        }
    }

    /// <summary>
    /// 유저 데이터
    /// </summary>
    public struct Userdata
    {
        public string id;
        public string pw;
        public string nickname;
        public Userdata(string _id, string _pw, string _nick)
        {
            id = _id;
            for (int i = id.Length; i < 32; i++)
                id += "\0";
            pw = _pw;
            for (int i = pw.Length; i < 32; i++)
                pw += "\0";
            nickname = _nick;
            for (int i = nickname.Length; i < 32; i++)
                nickname += "\0";
        }
    }

    /// <summary>
    /// 유저 프로필
    /// </summary>
    public struct User_Profile
    {
        public UInt16 Session_ID;
        public string ID;
        public string Nickname;

        public UInt16 Role_Index;
        public bool Is_Ready;

        public Vector3 Current_Pos;
        public Vector3 Current_Rot;
        public User_Input User_Input;

        public static void UnPackPacket(byte[] _data, ref User_Profile[] _datas)
        {
            int place = 0;
            place += sizeof(UInt64); // 프로토콜 점프

            UInt64 array_length = BitConverter.ToUInt64(_data, place); // 배열 길이 취득
            place += sizeof(int);

            DebugLogger.Instance.AddText("<array length = " + array_length + " >");
            _datas = new User_Profile[array_length];
            for(uint i = 0; i < array_length; i++)
            {
                _datas[i] = new User_Profile();
                _datas[i].Session_ID = BitConverter.ToUInt16(_data, place);
                place += sizeof(UInt16);
                _datas[i].ID = Encoding.Unicode.GetString(_data, place, 32);
                place += 64;
                _datas[i].Nickname = Encoding.Unicode.GetString(_data, place, 32);
                place += 64;
                _datas[i].Role_Index = BitConverter.ToUInt16(_data, place);
                place += sizeof(UInt16);
                _datas[i].Is_Ready = BitConverter.ToBoolean(_data, place);
                place += sizeof(bool);

                float x = BitConverter.ToSingle(_data, place);
                place += sizeof(float);
                float y = BitConverter.ToSingle(_data, place);
                place += sizeof(float);
                float z = BitConverter.ToSingle(_data, place);
                place += sizeof(float);
                _datas[i].Current_Pos = new Vector3(x, y, z);

                x = BitConverter.ToSingle(_data, place);
                place += sizeof(float);
                y = BitConverter.ToSingle(_data, place);
                place += sizeof(float);
                z = BitConverter.ToSingle(_data, place);
                place += sizeof(float);
                _datas[i].Current_Rot = new Vector3(x, y, z);

                _datas[i].User_Input.Read_Bytes(_data, ref place);

                DebugLogger.Instance.AddText("[ user profile " + i + " ]");
                DebugLogger.Instance.AddText("Session_ID: " + _datas[i].Session_ID);
                DebugLogger.Instance.AddText("NickName: " + _datas[i].Nickname);
                DebugLogger.Instance.AddText("Role_Index: " + _datas[i].Role_Index);
                DebugLogger.Instance.AddText("Is_Ready: " + _datas[i].Is_Ready);
                DebugLogger.Instance.AddText("Current_pos: " + _datas[i].Current_Pos);
                DebugLogger.Instance.AddText("Current_rot: " + _datas[i].Current_Rot);
            }
        }
    }


    /// <summary>
    /// 패킷 제작기
    /// </summary>
    public class Packer
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

            _size = (_size / 8 + 1) * 8;

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
    }

    public class Sender
    {
        /// <summary>
        /// 나 살아있다고 알리기
        /// </summary>
        public static void Send_Heartbeat()
        {
            Task task = new Task();
            UInt64 protocol = (UInt64)PROTOCOL.GLOBAL | (UInt64)PROTOCOL_GLOBAL.HEART_BEAT;
            task.buffer = Packer.PackPacket(ref task.datasize, protocol, "");
            // task.Encrypt(Manager_Network.Instance.m_Encryptor);

            Manager_Network.Log("buffer size = " + task.datasize);

            Manager_Packet.Instance.SendEnqueue(task);
        }

        /// <summary>
        /// 프로토콜 보내기
        /// </summary>
        public static void Send_Protocol(UInt64 _protocol)
        {
            Task task = new Task();
            task.buffer = Packer.PackPacket(ref task.datasize, _protocol);

            task.Encrypt(Manager_Network.Instance.m_Encryptor);
            Manager_Packet.Instance.SendEnqueue(task);
        }

        /// <summary>
        /// 로그인 & 회원가입시의 유저데이터 보내기
        /// </summary>
        public static void Send_Userdata(UInt64 _protocol, Userdata _data)
        {
            Task task = new Task();
            task.buffer = Packer.PackPacket(ref task.datasize, _protocol, _data);

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
            task.buffer = Packer.PackPacket(ref task.datasize, protocol, _Type);
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
            task.buffer = Packer.PackPacket(ref task.datasize, protocol, "");
            task.Encrypt(Manager_Network.Instance.m_Encryptor);

            Manager_Packet.Instance.SendEnqueue(task);
        }
    }

    /// <summary>
    /// 패킷 분해기
    /// </summary>
    public class Unpacker
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
            for(int i = 0; i < 32; i++)
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

}
