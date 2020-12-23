using RootMotion.FinalIK;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;


/// <summary>
/// 200912 주현킴
/// 캐릭터 애니메이션 관리 클래스
/// </summary>
public class CharacterAnimator : MonoBehaviour
{
    // TODO : 애니메이터 정리 예정
    [Header("IK 핸들")]
    public GameObject IK_AimMode;   // 조준 모드시의 IK
    public GameObject IK_LookMode;  // 일반 모드시의 IK

    [Header("움직임 값")]
    public Vector2 m_StickValue;
    public Vector2 m_LastValue;

    [Header("로그 특수")]
    public GameObject m_LiveModel;
    public GameObject prefab_DeadEffect;
    public List<Material> m_RenderTextures = new List<Material>();

    public PlayerCameraActionManager ActionCamMgr;

    [Header("가드 특수")]
    public GameObject m_Head;
    public GameObject m_CamAxis;

    CharacterController pc;
    Animator m_Anim;
    bool m_Use_Role_Skill = false;
    Vector3 m_Cam_OriginPos;

    public LayerMask playerLayer;
    Coroutine m_Routine_Cloaking;

    Shader m_OriShader;
    Shader m_CloakShader;


    [Header("임시 캐릭터 효과음")]
    public AudioSource CloakingSound;

    void Start()
    {
        m_Cam_OriginPos = m_CamAxis.transform.localPosition;
        pc = GetComponent<CharacterController>();
        m_Anim = GetComponentInChildren<Animator>();

        pc.e_ToolChanged.AddListener(When_Tool_Changed);

        switch(pc.m_MyProfile.Role_Index)
        {
            case 1:
                pc.e_Stunned.AddListener(When_Stunned);
                break;
            case 2:
                pc.e_Damaged.AddListener(When_Damaged);
                break;
            default:
                break;
        }// TODO : 상위 오브젝트의 메테리얼 집합(Core, HeadCtrl)의 렌더러가 잡히지 않는 문제로 클로킹이 씹히는 중

        pc.e_RoleSkill_Toggle.AddListener(When_Role_Skill_Toggle);

        m_OriShader = Shader.Find("Project Droids/Droid HD");
        m_CloakShader = Shader.Find("Custom/Cloaking");

        ActionCamMgr = PlayerCameraActionManager.Instance;

        StartCoroutine(SlowInit());
    }

    IEnumerator SlowInit()
    {
        yield return new WaitForSeconds(2.0f);

        if (pc.m_MyProfile.Role_Index == 2)
        {
            Renderer[] renderers = GetComponentsInChildren<Renderer>();

            for (int i = 0; i < renderers.Length; i++)
            {
                m_RenderTextures.Add(renderers[i].material);
            }
        }
    }

    void FixedUpdate()
    {
        m_StickValue = new Vector2(pc.m_MyProfile.User_Input.Move_X, pc.m_MyProfile.User_Input.Move_Y);
        if (m_StickValue.y < 0.0f)
            m_StickValue.x = -m_StickValue.x;
        m_LastValue = Vector2.Lerp(m_LastValue, m_StickValue, 0.2f);

        m_Anim.SetFloat("Horizontal", m_LastValue.x);
        m_Anim.SetFloat("Vertical", m_LastValue.y);
    }

    void When_Tool_Changed(int _tool_index)
    {
        // 툴 오브젝트를 취득
        Tool tool = pc.m_Tools[_tool_index - 1];
        if (tool == null)
        {
            IK_AimMode.SetActive(false);
            m_Anim.SetBool("is_Aiming", false);
            // IK_LookMode.SetActive(true);
            return;
        }

        // 툴이 조준 가능함?
        I_IK_Shotable shotable = null;
        if (tool is I_IK_Shotable)
        {
            shotable = tool as I_IK_Shotable;
            AimIK aim = IK_AimMode.GetComponentInChildren<AimIK>(true);
            if (aim)
                aim.solver.transform = shotable.Get_Muzzle();
            LimbIK limb = IK_AimMode.GetComponentInChildren<LimbIK>(true);
            if (limb)
                limb.solver.target = shotable.Get_Lefthand_Grip();
        }

        // 조준 여부에 따른 애니메이션 처리
        IK_AimMode.SetActive(shotable != null);
        m_Anim.SetBool("is_Aiming", shotable != null);
        // IK_LookMode.SetActive(shotable == null);
    }

    /// <summary>
    /// 피해 받았을 때의 애니메이션 처리
    /// </summary>
    /// <param name="_damage"></param>
    void When_Damaged(int _damage)
    {
        int hp = pc.m_MyProfile.HP;
        if (hp == 0)
        {
            m_LiveModel.SetActive(false);
            GameObject eff = Instantiate(prefab_DeadEffect);
            Manager_Ingame.Instance.Add_Round_Object(eff);
            eff.transform.position = transform.position;
            DeathCam.Instance.InvokeDeathCamera(GetAnimatedGuardAction(eff.transform), eff.transform);
        }
    }

    Transform GetAnimatedGuardAction(Transform _Target)
    {
         Collider[] colliders = Physics.OverlapSphere(_Target.position, float.PositiveInfinity, playerLayer.value);
         foreach (var collider in colliders)
         {
             if (collider.gameObject.GetComponent<CharacterController>().IsGuard())
             {
                 return collider.transform;
             }
         }
        return m_CamAxis.transform;
    }

    /// <summary>
    /// 스턴 당했을 때의 애니메이션 처리
    /// </summary>
    /// <param name="_tick"></param>
    void When_Stunned(int _tick)
    {
        StartCoroutine(Stun_Process(_tick));
    }

    IEnumerator Stun_Process(int _tick)
    {
        m_Anim.SetTrigger("Stun");

        bool lookMode = IK_LookMode.activeSelf;
        bool aimMode = IK_AimMode.activeSelf;

        IK_LookMode.SetActive(false);
        IK_AimMode.SetActive(false);

        m_Anim.SetBool("is_Stunned", true);
        m_CamAxis.transform.SetParent(m_Head.transform);

        yield return new WaitForSecondsRealtime(_tick / 1000f - 3.0f);

        m_Anim.SetBool("is_Stunned", false);

        yield return new WaitForSecondsRealtime(3.0f);
        m_CamAxis.transform.SetParent(transform);
        m_CamAxis.transform.localPosition = m_Cam_OriginPos;

        IK_LookMode.SetActive(lookMode);
        IK_AimMode.SetActive(aimMode);

        yield return null;
    }

    void When_Role_Skill_Toggle(bool _enable)
    {
        m_Use_Role_Skill = _enable;

        switch (pc.m_MyProfile.Role_Index)
        {
            case 1:
                Role_Skill_Process_Guard();
                break;
            case 2:
                Role_Skill_Process_Rogue();
                break;
            default:
                return;
        }
    }

    void Role_Skill_Process_Guard()
    {
        Debug.Log("가드 스킬 토글 - " + m_Use_Role_Skill);
    }

    void Role_Skill_Process_Rogue()
    {
        Debug.Log("로그 스킬 토글 - " + m_Use_Role_Skill);

        if (m_Routine_Cloaking != null)
        {
            StopCoroutine(StartCloaking(0.0f));
            StopCoroutine(RestoreCloaking(0.0f));
        }

        if (m_Use_Role_Skill)
        {
            CloakingSound.PlayOneShot(CloakingSound.clip);
            StartCoroutine(StartCloaking(0.0f));
        }
        else
        {
            CloakingSound.PlayOneShot(CloakingSound.clip);
            StartCoroutine(RestoreCloaking(0.0f));            
        }
    }



    IEnumerator StartCloaking(float _FirstTime)
    {
        while (_FirstTime < 3.0f)
        {
            yield return new WaitForEndOfFrame();
            _FirstTime += Time.deltaTime;

            //Core.GetComponent<Renderer>().material.shader = m_CloakShader;

            for(int i = 0; i < m_RenderTextures.Count; i++)
            {
                m_RenderTextures[i].shader = m_CloakShader;
                m_RenderTextures[i].SetFloat("_CutRender", Mathf.Lerp(0.0f, 1.0f, _FirstTime * 0.3f));
            }
        }

        m_Routine_Cloaking = null;
    }

    IEnumerator RestoreCloaking(float _FirstTime)
    {      
        while (_FirstTime < 3.0f)
        {
            yield return new WaitForEndOfFrame();
            _FirstTime += Time.deltaTime;

            for (int i = 0; i < m_RenderTextures.Count; i++)
            {
                m_RenderTextures[i].shader = m_OriShader;
                m_RenderTextures[i].SetFloat("_CutRender", Mathf.Lerp(1.0f, 0.0f, _FirstTime * 0.3f));

                float cutvar = m_RenderTextures[i].GetFloat("_CutRender");
                if (cutvar <= 0.0f)
                {
                    m_RenderTextures[i].shader = m_OriShader;
                }
            }
        }

        m_Routine_Cloaking = null;
    }
}
