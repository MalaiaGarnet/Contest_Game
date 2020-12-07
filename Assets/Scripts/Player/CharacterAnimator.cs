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

    [Header("가드 특수")]
    public GameObject m_Head;
    public GameObject m_CamAxis;

    CharacterController pc;
    Animator m_Anim;
    bool m_Use_Role_Skill = false;
    Vector3 m_Cam_OriginPos;

    public LayerMask playerLayer;

    [Header("임시 캐릭터 효과음")]
    public AudioSource CloakingSound;

    void Start()
    {
        m_Cam_OriginPos = m_CamAxis.transform.localPosition;
        pc = GetComponent<CharacterController>();
        m_Anim = GetComponentInChildren<Animator>();

        pc.e_ToolChanged.AddListener(When_Tool_Changed);

        if (pc.m_MyProfile.Role_Index == 1)
            pc.e_Stunned.AddListener(When_Stunned);
        if (pc.m_MyProfile.Role_Index == 2)
        {
            pc.e_Damaged.AddListener(When_Damaged);
            foreach (MeshRenderer mr in GetComponentsInChildren<MeshRenderer>())
            {
                if (mr.gameObject.layer == LayerMask.NameToLayer("Player"))
                    m_RenderTextures.Add(mr.material);
            }
        }
        pc.e_RoleSkill_Toggle.AddListener(When_Role_Skill_Toggle);
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
        Debug.Log("으앙 아픔 - " + hp);
        if (hp == 0)
        {
            Debug.Log("으앙 진짜 아픔;");
            m_LiveModel.SetActive(false);
            GameObject eff = Instantiate(prefab_DeadEffect);
            Manager_Ingame.Instance.Add_Round_Object(eff);
            eff.transform.position = transform.position;
            InvokeDeathCamera(eff.transform);
        }
    }

    void InvokeDeathCamera(Transform _Target)
    {
        DeathCam.Instance.FindSetKillPlayer(GetAnimatedGuardAction(_Target));
        DeathCam.Instance.FindSetDeathPlayer(_Target);
        DeathCam.Instance.StartCam();
    }

    Transform GetAnimatedGuardAction(Transform _Target)
    {
            Collider[] colliders = Physics.OverlapSphere(_Target.localPosition, float.PositiveInfinity, playerLayer.value);
            foreach (Collider collider in colliders)
            {
                if (collider.GetComponentInParent<CharacterController>().IsGuard())
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
        if (pc.m_MyProfile.Role_Index == 1)
            StartCoroutine(Role_Skill_Process_Guard());
        else
            StartCoroutine(Role_Skill_Process_Rogue());
    }

    IEnumerator Role_Skill_Process_Guard()
    {
        Debug.Log("가드 스킬 토글 - " + m_Use_Role_Skill);
        yield return null;
    }

    IEnumerator Role_Skill_Process_Rogue()
    {
        Debug.Log("로그 스킬 토글 - " + m_Use_Role_Skill);

        if (m_Use_Role_Skill)
        {
            CloakingSound.PlayOneShot(CloakingSound.clip);
            for (float i = 0f; i <= 1.0f; i += Time.deltaTime)
            {
                foreach (Material mat in m_RenderTextures)
                {
                    mat.shader = Shader.Find("Custom/Cloaking");
                    MatShaderModifyr.ChangeBlendRenderType(mat, BlendMode.Transparent, "Transparent");
                    // mat.SetFloat("_CutRender", i);
                    Shader.SetGlobalFloat(Shader.PropertyToID("_CutRender"), i);
                    // mat.SetFloat("_Opacity", Mathf.Max(0.0f, 1.0f - i));
                }
                yield return new WaitForEndOfFrame();
            }
            foreach (Material mat in m_RenderTextures)
            {
                mat.shader = Shader.Find("Custom/Cloaking");
                MatShaderModifyr.ChangeBlendRenderType(mat, BlendMode.Transparent, "Transparent");
                // mat.SetFloat("_CutRender", 1.0f);
                Shader.SetGlobalFloat(Shader.PropertyToID("_CutRender"), 1.0f);
                // mat.SetFloat("_Opacity", 0.0f);
            }

            yield return null;
        }
        else
        {
            CloakingSound.PlayOneShot(CloakingSound.clip);
            for (float i = 0f; i <= 1.0f; i += Time.deltaTime)
            {
                foreach (Material mat in m_RenderTextures)
                {
                    //mat.SetFloat("_CutRender", Mathf.Max(1.0f - i));
                    Shader.SetGlobalFloat(Shader.PropertyToID("_CutRender"), i);
                    //mat.SetFloat("_Opacity", Mathf.Min(1.0f, i));
                    if (mat.GetFloat("_CutRender") <= 0.0f)
                    {
                        if (mat.shader.GetInstanceID() != Shader.Find("Project Droids/Droid HD").GetInstanceID())
                        {
                            MatShaderModifyr.ChangeBlendRenderType(mat, BlendMode.Opaque, "Opaque");
                            mat.shader = Shader.Find("Project Droids/Droid HD");
                        }
                    }
                }
                yield return new WaitForEndOfFrame();
            }
            foreach (Material mat in m_RenderTextures)
            {
                //mat.SetFloat("_CutRender", 0.0f);
                Shader.SetGlobalFloat(Shader.PropertyToID("_CutRender"), 1.0f);
                //mat.SetFloat("_Opacity", 1.0f);
                if (mat.shader.GetInstanceID() != Shader.Find("Project Droids/Droid HD").GetInstanceID())
                {
                    MatShaderModifyr.ChangeBlendRenderType(mat, BlendMode.Opaque, "Opaque");
                    mat.shader = Shader.Find("Project Droids/Droid HD");
                }
            }

            yield return null;
        }
        yield return null;
    }

    /*IEnumerator Cloaking()
    {
        for (float i = 0f; i <= 1.0f; i += (Time.fixedDeltaTime / 10))
        {
            foreach (Material mat in m_RenderTextures)
            {
                mat.shader = Shader.Find("Custom/Cloaking");
                mat.SetFloat("_Cut", Mathf.Min(1.0f, i));
                mat.SetFloat("_Opacity", Mathf.Max(0.0f, 1.0f - i));
            }
            yield return new WaitForEndOfFrame();
        }
        foreach (Material mat in m_RenderTextures)
        {
            mat.shader = Shader.Find("Custom/Cloaking");
            mat.SetFloat("_Opacity", 0.0f);
        }

        yield return null;
    }*/

    /*void Cloaking()
    {
        for (float i = 0f; i <= 1.0f; i += (Time.fixedDeltaTime / 10))
        {
            for (int j = 0; j < m_RenderTextures.Count; j++)
            {
                m_RenderTextures[j].shader = Shader.Find("Custom/Cloaking");
                m_RenderTextures[j].SetFloat("_Cut", Mathf.Min(1.0f, i));
                m_RenderTextures[j].SetFloat("_Opacity", Mathf.Max(0.0f, 1.0f - i));
            }           
        }
        for (int i = 0; i < m_RenderTextures.Count; i++)
        {
            m_RenderTextures[i].SetFloat("_Cut", 1.0f);
            m_RenderTextures[i].SetFloat("_Opacity", 0.0f);
        }
    }*/

    /*IEnumerator RestroreFromCloaking()
    {
        for (float i = 0f; i <= 1.0f; i += (Time.fixedDeltaTime / 10))
        {
            foreach (Material mat in m_RenderTextures)
            {
                mat.SetFloat("_Cut", Mathf.Max(0.0f, 1.0f - i));
                mat.SetFloat("_Opacity", Mathf.Min(1.0f, i));
                if (mat.GetFloat("_Cut") <= 0.0f)
                {
                    if (mat.shader.GetInstanceID() != Shader.Find("Project Droids/Droid HD").GetInstanceID())
                    {
                        MatShaderModifyr.ChangeBlendRenderType(mat, BlendMode.Opaque, "Opaque");
                        mat.shader = Shader.Find("Project Droids/Droid HD");
                    }
                }
            }
            yield return new WaitForEndOfFrame();
        }
        foreach (Material mat in m_RenderTextures)
        {
            mat.SetFloat("_Cut", 0.0f);
            mat.SetFloat("_Opacity", 1.0f);
            if (mat.shader.GetInstanceID() != Shader.Find("Project Droids/Droid HD").GetInstanceID())
            {
                MatShaderModifyr.ChangeBlendRenderType(mat, BlendMode.Opaque, "Opaque");
                mat.shader = Shader.Find("Project Droids/Droid HD");
            }
        }

        yield return null;
    }*/
}
