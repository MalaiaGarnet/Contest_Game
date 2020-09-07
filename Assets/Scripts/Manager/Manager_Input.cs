using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Network.Data;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;

public class Manager_Input : SingleToneMonoBehaviour<Manager_Input>
{
    public Vector3 m_Pre_Position; // 예측한 좌표

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
