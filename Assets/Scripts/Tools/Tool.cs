using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// 툴 베이스 클래스
/// </summary>
public abstract class Tool : MonoBehaviour
{
    protected UnityAction<string, bool> m_Button_Trigger_Action;

    public Tool()
    {
        m_Button_Trigger_Action = new UnityAction<string, bool>(onButtonTrigger);
    }

    /// <summary>
    /// 사격 버튼을 눌렀을 때
    /// </summary>
    public abstract void onFire(bool _pressed);
    /// <summary>
    /// 상호작용 버튼을 눌렀을 때
    /// </summary>
    public abstract void onInteract(bool _pressed);
    /// <summary>
    /// 아무 버튼이나 눌렀을 때
    /// </summary>
    /// <param name="_name">버튼명</param>
    /// <param name="_pressed">누름 여부</param>
    public virtual void onButtonTrigger(string _name, bool _pressed)
    {
        switch (_name)
        {
            case "Fire":
                onFire(_pressed);
                break;
            case "Interact":
                onInteract(_pressed);
                break;
        }
    }

    Event_Button_Triggered saved_event;
    /// <summary>
    /// 이벤트에 자신의 액션을 등록
    /// </summary>
    public void Register(Event_Button_Triggered _event)
    {
        saved_event = _event;
        _event.AddListener(m_Button_Trigger_Action);
    }
    /// <summary>
    /// 이벤트에 자신의 액션을 해제
    /// </summary>
    public void Unregister()
    {
        if (saved_event != null)
        {
            saved_event.RemoveListener(m_Button_Trigger_Action);
            saved_event = null;
        }
    }

}
