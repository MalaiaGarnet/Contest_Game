using UnityEngine;

public class Cloaking : MonoBehaviour
{
    [Header("능력부여할 캐릭터")]
    public GameObject owner;
    private Material m_OwnerMat;
    [Header("대상 쉐이더")]
    public Shader oriShader;
    public Shader cloakShader;

    [Header("클로킹 옵션")]
    public float cloakDuration;
    [Range(0, 1)]
    public float cloakOpacity;
    [Range(0, 3)]
    public float cloakDeform;
    public int rimPower;
    [ColorUsage(true)]
    public Color rimColor;

    private const int MAX_PROPLENGTH = 512;
    private string[] m_cloakPropNames;

    // 스크립트가 시작될때
    void Awake()
    {
        InitMatchingCloakingShader();
    }

    void InitMatchingCloakingShader()
    {
        m_cloakPropNames = new string[MAX_PROPLENGTH];

        oriShader = Shader.Find(oriShader.name);
        cloakShader = Shader.Find(cloakShader.name);

        m_OwnerMat = owner.GetComponent<Renderer>().material;
        m_OwnerMat.shader = oriShader;

        if (cloakShader != null)
        {
            /// Test용 (추후 방식 바뀔예정)
            /// 쉐이더 프로퍼티 순서(쉐이더에서 선언된 프로퍼티 순서에 따라 매겨짐)
            /// 0 -> MainTex
            /// 1 -> DeformMap
            /// 2 -> NormalMap
            /// 3 -> Opacity
            /// 4 -> DeformIntense
            /// 5 -> RimPow
            /// 6 -> RimColor
            ///
            for (int i = 0; i <= cloakShader.GetPropertyCount(); i++)
            {
                m_cloakPropNames[i] = cloakShader.GetPropertyName(i);
                //Debug.Log(cloakShader.GetPropertyNameId(i) + "아이디의 프로퍼티 이름" + m_cloakPropNames[i]);
            }

            Shader.SetGlobalFloat(m_cloakPropNames[3], cloakOpacity);
            Shader.SetGlobalFloat(m_cloakPropNames[4], cloakDeform);
            Shader.SetGlobalFloat(m_cloakPropNames[5], rimPower);
            Shader.SetGlobalColor(m_cloakPropNames[6], rimColor);
        }

    }

    public void StartCloaking(bool _IsPress)
    {
        if (!_IsPress)
            return;


    }

    public void StopCloaking(bool _IsPress)
    {

    }
}
