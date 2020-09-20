using System;
using System.Diagnostics;
using System.Drawing;
using System.Text;
using UnityEngine;
using UnityEngine.Events;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Serialization.Formatters.Binary;

namespace Network.Data
{
    /// <summary> 버퍼 데이터 </summary>
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

            Manager_Network.Log("복호화 완료");

            // 복호화 한 것을 버퍼로 다시 옮기기
            Buffer.BlockCopy(decrypted, 0, buffer, 0, datasize);
        }
    }

    /// <summary> 유저 데이터 </summary>
    [Serializable]
    public class Userdata
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

    /// <summary> 유저 프로필 </summary>
    [Serializable]
    public class User_Profile
    {
        // 핵심
        public UInt16 Session_ID;

        public string ID;
        public string Nickname;

        public UInt16 Role_Index;
        public bool Is_Ready;

        // 인게임
        public Vector3 Current_Pos;
        public Vector3 Current_Rot;
        public User_Input User_Input;

        public UInt16 Current_Tool; // 현재 몇 번 무기를 들고 있나
        public UInt16 Tool_1;
        public UInt16 Tool_2;
        public UInt16 Tool_3;
        public UInt16 Tool_4;

        public static void UnPackPacket(byte[] _data, ref User_Profile[] _datas)
        {
            int place = 0;
            UInt16 strlen = 0; 
            place += sizeof(UInt64); // 프로토콜 점프

            UInt64 array_length = BitConverter.ToUInt64(_data, place); // 배열 길이 취득
            place += sizeof(UInt64);

            DebugLogger.Instance.AddText("<array length = " + array_length + " >");
            _datas = new User_Profile[array_length];
            for(uint i = 0; i < array_length; i++)
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

                DebugLogger.Instance.AddText("[ user profile " + i + " ]");
                DebugLogger.Instance.AddText("Session_ID: " + _datas[i].Session_ID);
                DebugLogger.Instance.AddText("NickName: " + _datas[i].Nickname);
                DebugLogger.Instance.AddText("Role_Index: " + _datas[i].Role_Index);
                DebugLogger.Instance.AddText("Is_Ready: " + _datas[i].Is_Ready);
                DebugLogger.Instance.AddText("Current_pos: " + _datas[i].Current_Pos);
                DebugLogger.Instance.AddText("Current_rot: " + _datas[i].Current_Rot);
                DebugLogger.Instance.AddText("User_Input_View x: " + _datas[i].User_Input.View_X+ 
                                                                "User_Input_View y: "+ _datas[i].User_Input.View_Y);
                DebugLogger.Instance.AddText("User_Input_Move x: " + _datas[i].User_Input.View_X + 
                                                "User_Input_Move y: " + _datas[i].User_Input.View_Y);
            }
        }
    }

    /// <summary> 유저 입력 </summary>
    [Serializable]
    public struct User_Input
    {
        public float Move_X;
        public float Move_Y;
        public float View_X;
        public float View_Y;
        public bool Fire;
        public bool Jump;
        public bool Interact;
        public bool Menu;
        public bool Tool_1;
        public bool Tool_2;
        public bool Tool_3;
        public bool Tool_4;

        public static User_Input Read_Bytes_New(byte[] _data, ref int _place)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                User_Input ui = new User_Input();
                BinaryFormatter bf = new BinaryFormatter();
                int type_size = Marshal.SizeOf(ui.GetType());

                ms.Write(_data, _place, type_size);
                ms.Seek(0, SeekOrigin.Begin);

                return (User_Input)bf.Deserialize(ms);
            }
        }

        public void Read_Bytes(byte[] _data, ref int _place)
        {
            Move_X = BitConverter.ToSingle(_data, _place);
            _place += sizeof(float);
            Move_Y = BitConverter.ToSingle(_data, _place);
            _place += sizeof(float);
            View_X = BitConverter.ToSingle(_data, _place);
            _place += sizeof(float);
            View_Y = BitConverter.ToSingle(_data, _place);
            _place += sizeof(float);

            Fire = BitConverter.ToBoolean(_data, _place);
            _place += sizeof(bool);
            Jump = BitConverter.ToBoolean(_data, _place);
            _place += sizeof(bool);
            Interact = BitConverter.ToBoolean(_data, _place);
            _place += sizeof(bool);
            Menu = BitConverter.ToBoolean(_data, _place);
            _place += sizeof(bool);
            Tool_1 = BitConverter.ToBoolean(_data, _place);
            _place += sizeof(bool);
            Tool_2 = BitConverter.ToBoolean(_data, _place);
            _place += sizeof(bool);
            Tool_3 = BitConverter.ToBoolean(_data, _place);
            _place += sizeof(bool);
            Tool_4 = BitConverter.ToBoolean(_data, _place);
            _place += sizeof(bool);
        }
        public void Write_Bytes(ref byte[] _data, ref int _place, ref int _size)
        {
            /*
            BinaryFormatter bf = new BinaryFormatter();
            using (MemoryStream ms = new MemoryStream())
            {
                bf.Serialize(ms, this);
                int type_size = Marshal.SizeOf(GetType());
                Buffer.BlockCopy(ms.ToArray(), 0, _data, _place, type_size);
                _place += type_size; _size += type_size;
            }*/

            Buffer.BlockCopy(BitConverter.GetBytes(Move_X), 0, _data, _place, sizeof(float));
            _place += sizeof(float); _size += sizeof(float);
            Buffer.BlockCopy(BitConverter.GetBytes(Move_Y), 0, _data, _place, sizeof(float));
            _place += sizeof(float); _size += sizeof(float);
            Buffer.BlockCopy(BitConverter.GetBytes(View_X), 0, _data, _place, sizeof(float));
            _place += sizeof(float); _size += sizeof(float);
            Buffer.BlockCopy(BitConverter.GetBytes(View_Y), 0, _data, _place, sizeof(float));
            _place += sizeof(float); _size += sizeof(float);

            Buffer.BlockCopy(BitConverter.GetBytes(Fire), 0, _data, _place, sizeof(bool));
            _place += sizeof(bool); _size += sizeof(bool);
            Buffer.BlockCopy(BitConverter.GetBytes(Jump), 0, _data, _place, sizeof(bool));
            _place += sizeof(bool); _size += sizeof(bool);
            Buffer.BlockCopy(BitConverter.GetBytes(Interact), 0, _data, _place, sizeof(bool));
            _place += sizeof(bool); _size += sizeof(bool);
            Buffer.BlockCopy(BitConverter.GetBytes(Menu), 0, _data, _place, sizeof(bool));
            _place += sizeof(bool); _size += sizeof(bool);
            Buffer.BlockCopy(BitConverter.GetBytes(Tool_1), 0, _data, _place, sizeof(bool));
            _place += sizeof(bool); _size += sizeof(bool);
            Buffer.BlockCopy(BitConverter.GetBytes(Tool_2), 0, _data, _place, sizeof(bool));
            _place += sizeof(bool); _size += sizeof(bool);
            Buffer.BlockCopy(BitConverter.GetBytes(Tool_3), 0, _data, _place, sizeof(bool));
            _place += sizeof(bool); _size += sizeof(bool);
            Buffer.BlockCopy(BitConverter.GetBytes(Tool_4), 0, _data, _place, sizeof(bool));
            _place += sizeof(bool); _size += sizeof(bool);
        }
    };
}
