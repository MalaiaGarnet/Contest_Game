
/*
public class KJH_Crypto
{
    public byte[] key;

    public KJH_Crypto(byte[] _key)
    {
        key = new byte[32];
        Buffer.BlockCopy(_key, 0, key, 0, 32);
    }

    public void Encrypt(ref byte[] _input, int _length, ref byte[] _output)
    {
        byte[] output = new byte[_length];

        Manager_Network.Log("암호화 - " + _input.Length + " // " + output.Length);

        int key_index = 0;
        for (int place = 0; place < _length; place += 8)
        {
            key_index = key_index + 1 >= 32 ? 0 : key_index + 1;
            Queue<byte> temp = new Queue<byte>(); // 큐에 데이터 대입
            for (int i = 0; i < 8; i++)
                temp.Enqueue(_input[place + i]);

            // Manager_Network.Log("암호화 블럭 - " + BitConverter.ToString(temp.ToArray()));

            for(int i = 0; i < key[key_index]; i++) // 바이트 회전
            {
                byte j = temp.Dequeue();
                temp.Enqueue(j);
            }

            // Manager_Network.Log("암호화 후 - " + BitConverter.ToString(temp.ToArray()));

            Buffer.BlockCopy(temp.ToArray(), 0, output, place, 8);
        }

        _output = output;
    }

    public void Decrypt(ref byte[] _input, int _length, ref byte[] _output)
    {
        byte[] output = new byte[_length];
        Manager_Network.Log("복호화 - " + _input.Length + " // " + output.Length);

        // Manager_Network.Log("복호화 전 - " + BitConverter.ToString(_input.ToArray()));

        int key_index = 0;
        for (int place = 0; place < _length; place += 8)
        {
            key_index = key_index + 1 >= 32 ? 0 : key_index + 1;

            Queue<byte> temp = new Queue<byte>(); // 큐에 데이터 대입
            for (int i = 0; i < 8; i++)
                temp.Enqueue(_input[place + i]);

            // Manager_Network.Log("복호화 블럭 - " + BitConverter.ToString(temp.ToArray()));

            for (int i = 0; i < 256 - key[key_index]; i++) // 바이트 회전
            {
                byte j = temp.Dequeue();
                temp.Enqueue(j);
            }

            // Manager_Network.Log("복호화 후 - " + BitConverter.ToString(temp.ToArray()));

            Buffer.BlockCopy(temp.ToArray(), 0, output, place, 8);
        }

        _output = output;
    }
}

*/