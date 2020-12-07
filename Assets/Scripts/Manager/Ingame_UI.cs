using System.Collections;
using UnityEngine;
using UnityEngine.Events;


public class Event_UI_Initialize : UnityEvent { }

public class Ingame_UI : SingleToneMonoBehaviour<Ingame_UI>
{
    [Header("주시할 플레이어")]
    public CharacterController m_Player;

    [Header("곁다리")]
    public GameObject m_Header;
    public GameObject m_Footer;
    public GameObject m_LeftSide;
    public GameObject m_RightSide;

    [Header("센터 오브젝트들")]
    public GameObject m_Crosshair;
    public Scene_Loader m_Ingame_Scene_Loader;
    public Round_Indicator m_Ingame_Round_Indicator;
    public GameObject m_Dead_Indicator;
    public GameObject m_Stun_Indicator;
    public GUI_Menu m_Menu;
    public GameObject m_Scoreboard;
    public GameObject m_Session_End;

    public Event_UI_Initialize e_Initialize = new Event_UI_Initialize();

    UnityAction<int> a_When_Damaged;
    UnityAction<int> a_When_Stunned;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        a_When_Damaged = new UnityAction<int>(When_Damaged);
        a_When_Stunned = new UnityAction<int>(When_Stunned);
    }

    private IEnumerator Start()
    {
        while (Manager_Ingame.Instance == null)
            yield return new WaitForEndOfFrame();

        if (!Manager_Ingame.Instance.m_DebugMode)
        {
            while (Manager_Network.Instance == null)
                yield return new WaitForEndOfFrame();
            m_Scoreboard.GetComponent<GUI_Scoreboard>().Register_Event();
        }
    }

    public void Initialize()
    {
        m_Header.SetActive(false);
        m_Footer.SetActive(false);
        m_LeftSide.SetActive(false);
        m_RightSide.SetActive(false);
        m_Ingame_Scene_Loader.gameObject.SetActive(false);
        m_Ingame_Round_Indicator.gameObject.SetActive(false);
        m_Dead_Indicator.SetActive(false);
        m_Stun_Indicator.SetActive(false);
        m_Menu.Activate(false);
        m_Scoreboard.SetActive(false);
        m_Session_End.SetActive(false);
        Lock_Cursor(false);
    }

    public bool Can_Move()
    {
        if (m_Ingame_Scene_Loader.gameObject.activeSelf)
            return false;
        if (m_Dead_Indicator.activeSelf)
            return false;
        if (m_Stun_Indicator.activeSelf)
            return false;
        if (m_Menu.gameObject.activeSelf)
            return false;
        return true;
    }

    public void Show_SessionEnd()
    {
        m_Session_End.SetActive(true);
    }

    /// <summary>
    /// 플레이어 전환
    /// </summary>
    /// <param name="_cc"></param>
    public void Set_Player(CharacterController _cc)
    {
        // 이전 플레이어 단계에서 해줄 거
        if (m_Player != null)
        {
            m_Player.e_Damaged.RemoveListener(a_When_Damaged);
            m_Player.e_Stunned.RemoveListener(a_When_Stunned);
        }

        // 현재 플레이어 갱신 후 해줄 거
        m_Player = _cc;
        m_Player.e_Damaged.AddListener(a_When_Damaged);
        m_Player.e_Stunned.AddListener(a_When_Stunned);
        When_Damaged(0);
        e_Initialize.Invoke();
    }

    public void Show(bool _enable)
    {
        gameObject.SetActive(_enable);
    }

    public void Lock_Cursor(bool _enable)
    {
        Cursor.lockState = _enable ? CursorLockMode.Locked : CursorLockMode.None;
        Cursor.visible = !_enable;
        m_Crosshair.SetActive(_enable);
    }

    public void When_Damaged(int _damage)
    {
        // TODO 좀 있다 끄거나 다른 방법 찾기 (데스캠)
        // m_Dead_Indicator.SetActive(m_Player.m_MyProfile.HP <= 0);
    }

    Coroutine stun_coroutine;
    public void When_Stunned(int _tick)
    {
        if (stun_coroutine != null)
            StopCoroutine(stun_coroutine);

        stun_coroutine = StartCoroutine(Stun_Process(_tick));
    }
    IEnumerator Stun_Process(int _tick)
    {
        Animation anim = m_Stun_Indicator.GetComponent<Animation>();
        anim.clip = anim.GetClip("GUI_Ingame_Stun_Indicator_Fade_In");
        m_Stun_Indicator.SetActive(true);
        yield return new WaitForSecondsRealtime(_tick / 1000f);

        anim.clip = anim.GetClip("GUI_Ingame_Stun_Indicator_Fade_Out");
        anim.Play();

        yield return new WaitForSecondsRealtime(2.0f);
        m_Stun_Indicator.SetActive(false);

        yield return null;
    }
}
