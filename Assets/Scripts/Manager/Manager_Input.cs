using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;

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

public struct SInteractAction
{
    public SlowTapInteraction  SlowTap;
    public TapInteraction      Tap;
    public PressInteraction    Press;
    public MultiTapInteraction MultiTap;
    public HoldInteraction     Hold;

    public IInputInteraction inputInteraction;
    public void SetInteractAction(ref object _Objects)
    {
    }
}

public class Manager_Input : SingleToneMonoBehaviour<Manager_Input>
{
    private PlayerInputAction m_InputAction;
    public  User_Input        m_Player_Input;
    private SInteractAction   m_InteractAction;
    
    void Awake()
    {
        m_InputAction = new PlayerInputAction();
        m_InputAction.PlayerMoves.Move.performed += context => InputAct_Moving(context); // 이동 전달
    }

    public void InputAct_Moving(InputAction.CallbackContext _Context)
    {
        
        m_Player_Input.Move_X += _Context.ReadValue<Vector2>().x;
        m_Player_Input.Move_Y += _Context.ReadValue<Vector2>().y;

        if(_Context.interaction is PressInteraction)//만약 누르고있을때
        {
            m_InteractAction.Press.behavior = PressBehavior.PressOnly;
            if(m_InteractAction.Press.behavior == PressBehavior.PressOnly)
            {
 
            }
        }
    }

    public bool GetButtonDown()
    {
        return false;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
