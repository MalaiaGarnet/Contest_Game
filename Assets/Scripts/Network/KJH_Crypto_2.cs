using System;

public class KJH_Crypto_2
{
    public byte[] key;

    public KJH_Crypto_2(byte[] _key)
    {
        key = new byte[8];
        Buffer.BlockCopy(_key, 0, key, 0, 8);
    }

    public static void Print_Bytes(string _msg, byte[] _bytes)
    {
        Console.Out.Write(_msg + " = ");
        for (int i = 0; i < _bytes.Length; i++)
            Console.Out.Write(string.Format("{0:x2}", _bytes[i]) + " ");
        Console.Out.WriteLine("");
    }

    public byte[] Encrypt(ref byte[] _input)
    {
        int new_len = _input.Length;
        if (_input.Length % 8 > 0)
            new_len += 8 - (_input.Length % 8);

        byte[] new_bytes = new byte[new_len];
        Buffer.BlockCopy(_input, 0, new_bytes, 0, _input.Length);

        // 라운드 1 - 바이트 단위 스왑
        for (int i = 0; i < new_len; i += 8)
        {
            for (int j = 0; j < 8; j++) // 키 길이만큼 스왑
            {
                int shuffle_index = key[j] % 8;
                byte temp = new_bytes[i + j];
                new_bytes[i + j] = new_bytes[i + shuffle_index];
                new_bytes[i + shuffle_index] = temp;
            }
        }

        return new_bytes;
    }

    public byte[] Decrypt(ref byte[] _input)
    {
        int new_len = _input.Length;
        if (_input.Length % 8 > 0)
            new_len += 8 - (_input.Length % 8);

        byte[] new_bytes = new byte[new_len];
        Buffer.BlockCopy(_input, 0, new_bytes, 0, _input.Length);

        // 라운드 1 - 바이트 단위 스왑
        for (int i = 0; i < new_len; i += 8)
        {
            for (int j = 7; j >= 0; j--) // 키 길이만큼 스왑
            {
                int shuffle_index = key[j] % 8;
                byte temp = new_bytes[i + j];
                new_bytes[i + j] = new_bytes[i + shuffle_index];
                new_bytes[i + shuffle_index] = temp;
            }
        }

        return new_bytes;
    }
}