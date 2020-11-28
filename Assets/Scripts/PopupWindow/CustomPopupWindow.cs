using System;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
///  팝업 윈도우 타이틀 종류
/// </summary>
[Flags]
public enum PopupTitleCategory
{
    NOTICE = 0x01,
    QUESTION = 0x02,
    ERROR = 0x04
}

/// <summary>
/// 버튼결과
/// </summary>
public enum PopUpResult
{
    None,
    OK,
    Cancel,
    Abort,
    Retry,
    Ignore,
    Yes,
    No
}

/// <summary>
/// 팝업윈도우
/// </summary>
public class CustomPopupWindow : SingleToneMonoBehaviour<CustomPopupWindow>
{

    public static void Show(string _Title, string _Desc, Action<PopUpResult> _CallFunc)
    {
        Instance.ShowOnly(_Title, _Desc, _CallFunc);
    }

    public static void Show(string _Title, string _Desc)
    {
        Instance.ShowOnly(_Title, _Desc);
    }

    public static void Show(string _Title, string _Desc, params ButtonEventInfo[] _ButtonEvent)
    {
        Instance.ShowOnly(_Title, _Desc, _ButtonEvent);
    }
    #region 필드
    [Header("메시지박스 기본내용")]
    public Text Title = null;           // 타이틀명
    public Text Desc = null;            // 내용
    public bool m_LockButton = false;   //닫기 버튼 잠금여부
    public Button Close_Button = null; // 닫기버튼

    public Button BG_Button = null;    // 배경 버튼
    public Button OK_Button = null;   // OK 버튼
    public Button Cancel_Button = null; // 취소 버튼 <<임시>>

    public Button[] Buttons = null;     //버튼들

    private bool IsLockCloseButton => Instance.m_LockButton;

    public PopUpResult OutResult = PopUpResult.None;

    public Action<PopUpResult> CallBackFunc = null;

    public ButtonEventInfo[] ButtonEvent;
    public GameObject[] ButtonPrefab;
    #endregion


    #region 함수

    void ShowOnly(string _Title, string _Desc, Action<PopUpResult> _CallFunc)
    {
        BG_Button.gameObject.SetActive(true);
        gameObject.SetActive(true);
        Title.text = _Title;
        Desc.text = _Desc;
        CallBackFunc = _CallFunc;
    }
    void ShowOnly(string _Title, string _Desc)
    {
        BG_Button.gameObject.SetActive(true);
        gameObject.SetActive(true);
        Title.text = _Title;
        Desc.text = _Desc;
    }
    void ShowOnly(string _Title, string _Desc, params ButtonEventInfo[] _ButtonEvent)
    {
        int index = 0;
        BG_Button.gameObject.SetActive(true);
        gameObject.SetActive(true);
        Title.text = _Title;
        Desc.text = _Desc;
        foreach (var item in _ButtonEvent)
        {
            index++;
            ButtonEvent[index].ButtonName = item.ButtonName;
            ButtonEvent[index].ButtonAction = item.ButtonAction;
        }
    }

    void InvokeButtons()
    {
        for (int idx = 0; idx < ButtonPrefab.Length; idx++)
        {
            Instantiate(ButtonPrefab[idx], transform);
        }
    }


    void InitButtonConfig()
    {
        //Buttons[0].onClick.AddListener(_On_CallOK);
        OK_Button.onClick.AddListener(_On_CallOK);
        Cancel_Button.onClick.AddListener(_On_CallCancel);
        BG_Button.onClick.AddListener(_On_CallCancel);
        Close_Button.onClick.AddListener(_On_CallCancel);
    }

    // 버튼액션
    void CallBack_Function()
    {
        BG_Button.gameObject.SetActive(false);
        GetComponent<Small_Window>().Close();
        CallBackFunc?.Invoke(OutResult);
    }

    #region 버튼이벤트
    void _On_CallOK()
    {
        OutResult = PopUpResult.OK;
        CallBack_Function();
    }

    void _On_CallCancel()
    {
        if (!m_LockButton)
        {
            OutResult = PopUpResult.Cancel;
            CallBack_Function();
        }
    }
    #endregion

    void Awake()
    {
        InitButtonConfig();
        transform.gameObject.SetActive(false);
        BG_Button.gameObject.SetActive(false);
        SetInstance(this);

    }

    #endregion
}

public struct ButtonEventInfo
{
    public string ButtonName;
    public Action ButtonAction;
}
