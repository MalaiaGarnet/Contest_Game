using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Scene_Title : MonoBehaviour
{
    enum SUB_STATE { MIN = 0, LOGIN, REGISTER, MAX }

    public Animation m_Animation;
    public Scene_Login s_Login;

    public GameObject[] m_Sub_Scenes;
    public GameObject m_Button_Prev;
    public GameObject m_Button_Next;
    SUB_STATE m_Sub_State = SUB_STATE.LOGIN;

    public Animation m_login_anim;
    public InputField m_login_id;
    public InputField m_login_pw;

    public Animation m_register_anim;
    public InputField m_register_id;
    public InputField m_register_pw;
    public InputField m_register_nickname;

    void Awake()
    {
        Update_SubState(false);
    }

    IEnumerator Start()
    {
        while (Manager_Network.Instance == null)
            yield return new WaitForEndOfFrame();

        Manager_Network.Instance.e_RegisterResult.AddListener(new UnityAction<bool>(When_Get_Register_Result));
    }

    void Update()
    {

    }

    /// <summary>
    /// 이전 m_Sub_State로 전환
    /// </summary>
    public void To_Prev()
    {
        if (m_Sub_State - 1 == SUB_STATE.MIN)
            return;
        m_Sub_State -= 1;
        Update_SubState(false);
    }
    /// <summary>
    /// 다음 m_Sub_State로 전환
    /// </summary>
    public void To_Next()
    {
        if (m_Sub_State + 1 == SUB_STATE.MAX)
            return;
        m_Sub_State += 1;
        Update_SubState(true);
    }
    /// <summary>
    /// m_Sub_State 기반 오브젝트 업데이트
    /// </summary>
    void Update_SubState(bool _to_right)
    {
        // 현재 m_Sub_State에 해당되는 것만 활성화, 나머지는 비활성화
        for (int i = 0; i < m_Sub_Scenes.Length; i++)
            m_Sub_Scenes[i].SetActive(i == (int)m_Sub_State - 1);

        // 화살표 버튼 활성화
        m_Button_Prev.SetActive(m_Sub_State - 1 != SUB_STATE.MIN);
        m_Button_Next.SetActive(m_Sub_State + 1 != SUB_STATE.MAX);

        // sub_state 별 처리
        string right_sign = _to_right ? "Right" : "Left";
        switch (m_Sub_State)
        {
            case SUB_STATE.LOGIN:
                m_login_anim.clip = m_login_anim.GetClip("GUI_SubLogin_Fade_In_from_" + right_sign);
                m_login_anim.Play();
                break;
            case SUB_STATE.REGISTER:
                m_register_anim.clip = m_register_anim.GetClip("GUI_SubRegister_Fade_In_from_" + right_sign);
                m_register_anim.Play();
                break;
        }
    }

    public void Try_Login()
    {
        // ID와 PW 안 적거나 짧은 경우를 처리
        if (m_login_id.text.Length < 3 || m_login_pw.text.Length < 3)
        {
            CustomPopupWindow.Show("error", "ID, 암호는 최소 3글자 이상이어야 합니다.");
            return;
        }

        StartCoroutine(Login_Process());
    }
    IEnumerator Login_Process()
    {
        m_Animation.Play("GUI_Scene_Title_Fade_Out");
        yield return new WaitForSecondsRealtime(0.4f);
        m_login_anim.Play("GUI_SubLogin_Fade_Out_to_Left");
        yield return new WaitForSecondsRealtime(1.2f);

        s_Login.gameObject.SetActive(true);
        s_Login.Try_Login(m_login_id.text, m_login_pw.text);
        gameObject.SetActive(false);
    }

    public void Try_Register()
    {
        // ID와 PW, 닉네임 안 적거나 짧은 경우를 처리
        if (m_register_id.text.Length < 3 || m_register_pw.text.Length < 3 || m_register_nickname.text.Length < 3)
        {
            CustomPopupWindow.Show("error", "ID, 암호, 닉네임은 최소 3글자 이상이어야 합니다.");
            return;
        }

        Manager_Network.Instance.Register(m_register_id.text, m_register_pw.text, m_register_nickname.text);
    }
    void When_Get_Register_Result(bool _result)
    {
        if (_result)
            CustomPopupWindow.Show("notice", "회원가입에 성공했습니다.");
        else
            CustomPopupWindow.Show("notice", "회원가입에 실패했습니다.\n다른 ID를 사용해보십시오.");
    }
}
