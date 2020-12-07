using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

namespace Network.Data
{
    /// <summary> 버퍼 데이터 </summary>
    public class Task
    {
        public byte[] buffer = new byte[4096];
        public int datasize = 0;

        public void Encrypt(KJH_Crypto_2 _encryptor)
        {
            if (_encryptor == null)
                return;

            datasize += 8 - (datasize % 8);

            // 버퍼의 내용에서 사이즈 빼서 옮겨 담기
            byte[] temp_byte = new byte[datasize];
            Buffer.BlockCopy(buffer, 4, temp_byte, 0, datasize);

            Manager_Network.Log("암호화 전 버퍼 - " + BitConverter.ToString(temp_byte));

            // 내용 암호화
            byte[] encrypted = null;
            encrypted = _encryptor.Encrypt(ref temp_byte);
            datasize = encrypted.Length;

            // 암호화 한 것을 버퍼로 다시 옮기기
            Buffer.BlockCopy(encrypted, 0, buffer, 4, datasize);
            Buffer.BlockCopy(BitConverter.GetBytes(datasize), 0, buffer, 0, sizeof(int));
            datasize += sizeof(int);
            Manager_Network.Log("(사이즈포함) 암호화 된 버퍼 크기 = " + datasize);
        }
        public void Decrypt(KJH_Crypto_2 _decryptor)
        {
            if (_decryptor == null)
                return;

            Manager_Network.Log("복호화 수행 크기 = " + datasize);

            // 버퍼의 내용에서 사이즈 빼서 옮겨 담기
            byte[] temp_byte = new byte[datasize];
            Buffer.BlockCopy(buffer, 0, temp_byte, 0, datasize);

            // 내용 복호화
            byte[] decrypted = null;
            decrypted = _decryptor.Decrypt(ref temp_byte);

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


    public enum ROUND_END_REASON : ushort
    {
        NOT_ENDED = 0,
        TIME_OVER = 1,
        ALL_ROGUE_DEAD = 2,
        ALL_ROGUE_ESCAPED = 3,
    };
    public enum SESSION_END_REASON : ushort
    {
        NOT_ENDED = 0,
        NORMALLY_END = 1, // 정상 종료
        USER_TOO_SHORT = 2, // 누가 나가서 유저가 너무 적어짐
        CRITICAL_ERROR = 3, // 치명적인 오류
    };
    [Serializable]
    public struct Session_RoundData
    {
        public UInt16 Current_Round;
        public UInt64 Time_Left;
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
        public ushort HP;       // HP
        public ushort Battery;  // 배터리
        public ushort Score;    // 점수

        public Vector3 Current_Pos;
        public Vector3 Current_Rot;
        public User_Input User_Input;

        public UInt16 Current_Tool; // 현재 몇 번 무기를 들고 있나
        public UInt16 Tool_1;
        public UInt16 Tool_2;
        public UInt16 Tool_3;
        public UInt16 Tool_4;

        public bool m_Using_Skill = false;

        public void Round_Init()
        {
            HP = 1000;
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
        public bool Role_Skill;

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

            // UnityEngine.Debug.Log("들오는 사이즈 = " + Move_X + ", " + Move_Y);

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
            Role_Skill = BitConverter.ToBoolean(_data, _place);
            _place += sizeof(bool);
            _place += sizeof(bool);
            _place += sizeof(bool);
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

            Buffer.BlockCopy(BitConverter.GetBytes(Role_Skill), 0, _data, _place, sizeof(bool));
            _place += sizeof(bool); _size += sizeof(bool);
            Buffer.BlockCopy(BitConverter.GetBytes(Role_Skill), 0, _data, _place, sizeof(bool));
            _place += sizeof(bool); _size += sizeof(bool);
            Buffer.BlockCopy(BitConverter.GetBytes(Role_Skill), 0, _data, _place, sizeof(bool));
            _place += sizeof(bool); _size += sizeof(bool);
            Buffer.BlockCopy(BitConverter.GetBytes(Role_Skill), 0, _data, _place, sizeof(bool));
            _place += sizeof(bool); _size += sizeof(bool);
        }
    };

    /// <summary> 아이템 정보 </summary>
    [Serializable]
    public struct Item_Data
    {
        public int IID;
        public int OID;
        public Vector3 Position;
        public Vector3 Rotation;

        public void Read_Bytes(byte[] _data, ref int _place)
        {
            IID = BitConverter.ToInt32(_data, _place);
            _place += sizeof(int);

            OID = BitConverter.ToInt32(_data, _place);
            _place += sizeof(int);

            float x = BitConverter.ToSingle(_data, _place);
            _place += sizeof(float);
            float y = BitConverter.ToSingle(_data, _place);
            _place += sizeof(float);
            float z = BitConverter.ToSingle(_data, _place);
            _place += sizeof(float);
            Position = new Vector3(x, y, z);

            x = BitConverter.ToSingle(_data, _place);
            _place += sizeof(float);
            y = BitConverter.ToSingle(_data, _place);
            _place += sizeof(float);
            z = BitConverter.ToSingle(_data, _place);
            _place += sizeof(float);
            Rotation = new Vector3(x, y, z);
        }
    }
}
