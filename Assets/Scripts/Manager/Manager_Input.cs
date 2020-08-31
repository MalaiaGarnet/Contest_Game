using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


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
};

public class Manager_Input : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
