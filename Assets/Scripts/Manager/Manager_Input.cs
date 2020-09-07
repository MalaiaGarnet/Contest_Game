using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;
using UnityEngine.Networking;
using Network.Data;

public class Manager_Input : SingleToneMonoBehaviour<Manager_Input>
{
    #region  필드와 프로퍼티

    public User_Input m_Player_Input;
    public Vector3 m_Pre_Position; // 현재 위치

    [Header("Movement Settings")]
    public float movementSpeed = 3;
    public float smoothingSpeed = 1;
    private Vector3 rawDirection;
    private Vector3 smoothDirection;
    private Vector3 movement;


    public PressInteraction pressEvent;

    public float TimeStamp { get; set; } = 0.100f; // 타임스팸프

    public bool IsMoving { get; set; } = false; // 이동중인지
    public Vector3 InputDirection { get; set; } // 입력 방향
    public PlayerInputAction InputActions { get; set; } // 인풋액션
    public GameObject ActorObj { get; set; } // 플레이어오브젝트
    public Rigidbody PlayerRigidbody { get; set; } // 리지드바디
    public Camera MainCamera { get; set; } // 메인카메라
    #endregion

    void Start()
    {
        if (Manager_Ingame.Instance.m_Game_Started)
        {
            if (InputActions == null)
            {
                InputActions = new PlayerInputAction();
                InputActions.PlayerMoves.PlayerMoving.performed += obj => OnPlayerMoving(obj);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Manager_Ingame.Instance.m_Game_Started)
        {
            if (InputDirection == Vector3.zero)
            {
                IsMoving = false;
            }
            else if (InputDirection != Vector3.zero)
            {
                IsMoving = true;
            }
        }
    }

    void FixedUpdate()
    {
        if (Manager_Ingame.Instance.m_Game_Started)
        {
            CalculateDesiredDirection();
            ConvertDirectionFromRawToSmooth();
            MoveThePlayer();
            TurnThePlayer();
        }
    }

    void CalculateDesiredDirection()
    {
        //Camera Direction
        var cameraForward = MainCamera.transform.forward;
        var cameraRight = MainCamera.transform.right;

        cameraForward.y = 0f;
        cameraRight.y = 0f;

        rawDirection = cameraForward * InputDirection.z + cameraRight * InputDirection.x;
    }

    void ConvertDirectionFromRawToSmooth()
    {
        if (IsMoving == true)
        {
            smoothDirection = Vector3.Lerp(smoothDirection, rawDirection, Time.deltaTime * smoothingSpeed);
        }
        else if (IsMoving == false)
        {
            smoothDirection = Vector3.zero;
        }

    }
    void TurnThePlayer()
    {
        if (IsMoving == true)
        {
            Quaternion newRotation = Quaternion.LookRotation(smoothDirection);
            PlayerRigidbody.MoveRotation(newRotation);
        }
    }

    void MoveThePlayer()
    {
        if (IsMoving == true)
        {
            movement.Set(smoothDirection.x, 0f, smoothDirection.z);
            movement = movement.normalized * movementSpeed * Time.deltaTime;
            PlayerRigidbody.MovePosition(transform.position + movement);
        }

    }


    /* void OnPlayerMoving(InputValue _Input)
     {
         // 인풋값 대입
         m_Player_Input.Move_X = _Input.Get<Vector2>().x;
         m_Player_Input.Move_Y = _Input.Get<Vector2>().y;

         InputDirection = new Vector3(m_Player_Input.Move_X, 0.0f, m_Player_Input.Move_Y);
         if(_Input.isPressed)
         {
             InputActions.PlayerMoves.PlayerMoving.performed += obj => OnPlayerMoving(obj);
         }

     } // 이곳에 충돌체크와 타임스팸프 갱신이 들어가야됨*/

    /// <summary>
    /// 이동 콜백
    /// </summary>
    /// <param name="_Input"> 인풋값</param> 
    public void OnPlayerMoving(InputAction.CallbackContext obj)
    {
        m_Player_Input.Move_X = obj.ReadValue<Vector2>().x;
        m_Player_Input.Move_Y = obj.ReadValue<Vector2>().y;
        InputDirection = new Vector3(m_Player_Input.Move_X, 0.0f, m_Player_Input.Move_Y);

        if (obj.interaction is PressInteraction)
        {
            float chkIndex = 0;
            chkIndex++;
            if (pressEvent == null)
            {
                pressEvent = new PressInteraction();
            }
            if (chkIndex == TimeStamp)
            {
                Debug.Log("인풋 : " + "{" + m_Player_Input.Move_X + "," + m_Player_Input.Move_Y + "}" + "좌표 : " + transform.position);
            }
            else
            {
                // Debug.Log("N인풋 : " + "{" + m_Player_Input.Move_X + "," + m_Player_Input.Move_Y + "}" + "좌표 : " + transform.position);
            }
        }
    }

    public bool GetButtonDown()
    {
        if (IsMoving)
        {
            if (m_Player_Input.Move_Y < 0.0)
            {
                return true;
            }
        }
        return false;
    }

    public bool GetButtonUp()
    {
        if (IsMoving)
        {
            if (m_Player_Input.Move_Y > 0.0)
            {
                return true;
            }
        }
        return false;
    }

    public bool GetButton()
    {
        if (IsMoving)
        {
            if (m_Player_Input.Move_X != 0)
            {
                return true;
            }
        }
        return false;
    }

    public User_Input GetUserInputs()
    {
        return m_Player_Input;
    }
}