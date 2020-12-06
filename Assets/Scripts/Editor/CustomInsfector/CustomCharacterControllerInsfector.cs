using UnityEngine;
using UnityEditor;
using System;


[CustomEditor(typeof(CharacterController), true)]
public class CustomCharacterControllerInsfector : Editor
{
    CharacterController m_TargetVars;
    public bool isDebug;

    [Serializable]
    public struct CustomTex2D
    {
        public int width;
        public int height;
        public Color color;

        public CustomTex2D(int _Width, int _Height, Color _Color)
        {
            this.width = _Width;
            this.height = _Height;
            this.color = _Color;
        }
    };

    public struct CustomLabel
    {
        public FontStyle   fontstyle;
        public int         fontSize;
        public Color       fontColor;
        public CustomTex2D cusTex2D;

        public CustomLabel(FontStyle _FontStyle, int _FontSize, Color _FontColor, CustomTex2D _Tex2D)
        {
            this.fontstyle = _FontStyle;
            this.fontSize = _FontSize;
            this.fontColor = _FontColor;
            this.cusTex2D = _Tex2D;
        }
    };

    void OnEnable()
    {
        // target은 위의 CustomEditor() 애트리뷰트에서 설정해 준 타입의 객체에 대한 레퍼런스
        // object형이므로 실제 사용할 타입으로 캐스팅 해 준다.
        m_TargetVars = target as CharacterController;
    }

    /// <summary>
    /// 커스텀에디터 구현 함수 재 정의.
    /// 인스펙터에서 보여지는 정보를 여기서 고쳐줍니다.
    /// </summary>
    public override void OnInspectorGUI()
    {
        GUIStyle debugLabel = CustomizeGUIStyle(new CustomLabel(FontStyle.Bold, 12, Color.white, new CustomTex2D(10, 5, Color.black)));

        EditorGUILayout.LabelField(new GUIContent("디버깅 인스펙터 활성화 여부"), debugLabel);
 
        isDebug = EditorGUILayout.Toggle(new GUIContent("디버깅중입니까?", "디버깅할때만 나타낼 값을 보여줍니다"), isDebug);
        if(isDebug)
        {
            EditorGUILayout.LabelField(new GUIContent("나의 캐릭터 프로필값"), CustomizeGUIStyle(new CustomLabel(FontStyle.Bold, 12, Color.white, new CustomTex2D(10, 5, Color.black))));
            UseProperty("m_MyProfile");
            UseProperty("m_Is_Stunned");
        }

        EditorGUILayout.LabelField(new GUIContent("캐릭터 스탯 및 물리메테리얼"), CustomizeGUIStyle(new CustomLabel(FontStyle.Bold, 12, Color.white, new CustomTex2D(10, 5, Color.black))));
        m_TargetVars.battery = EditorGUILayout.IntField(new GUIContent("배터리", "배터리 수치"), m_TargetVars.battery);
        m_TargetVars.moveSpeed = EditorGUILayout.FloatField(new GUIContent("이동속도 (m/s)", "이동속도"), m_TargetVars.moveSpeed);
        UseProperty("noFriction", "마찰력 없음", "마찰력을 없애는 메테리얼");
        UseProperty("fullFriction", "마찰력 있음", "마찰력이 있는 메테리얼");

        EditorGUILayout.Space();

        EditorGUILayout.LabelField(new GUIContent("캐릭터 무기 및 좌표"), CustomizeGUIStyle(new CustomLabel(FontStyle.Bold, 12, Color.white, new CustomTex2D(10, 5, Color.black))));
        UseProperty("m_ToolAxis", "툴 좌표", "툴을 든 손의 좌표");
        UseProperty("m_Tools", "툴들", "툴들의 배열");
        UseProperty("m_CameraAxis", "카메라(시선) 좌표", "캐릭터가 바라볼 카메라의 위치");
        if(isDebug)
        {
            EditorGUILayout.Space();
            UseProperty("m_Before_Position", "이전 위치", "캐릭터의 이전에 있던 위치");
        }
        EditorGUILayout.Space();
        EditorGUILayout.LabelField(new GUIContent("아이템 인식 설정"), CustomizeGUIStyle(new CustomLabel(FontStyle.Bold, 12, Color.white, new CustomTex2D(10, 5, Color.grey))));

        UseProperty("acquireDist", "아이템 인식거리", "아이템 인식거리");
        EditorGUILayout.Space();
        UseProperty("itemSearchDuration", "아이템 탐색 주기", "아이템 탐색 주기");
        UseProperty("ItemLayer", "아이템 레이어", "아이템 레이어");


        if (GUI.changed)
        {
            EditorUtility.SetDirty(target);
        }
    }

    GUIStyle CustomizeGUIStyle(CustomLabel _Label)
    {
        GUIStyle style = new GUIStyle();

        style.normal.textColor = _Label.fontColor;
        style.normal.background = CreateLabelBackGround(_Label.cusTex2D.width, _Label.cusTex2D.height, _Label.cusTex2D.color);
        style.fontStyle = _Label.fontstyle;
        style.fontSize = _Label.fontSize;
        style.alignment = TextAnchor.MiddleCenter;

        return style;
    }

    Texture2D CreateLabelBackGround(int _Width, int _Height, Color _Color)
    {
        Texture2D texture2D = new Texture2D(_Width, _Height);
     
        Color32[] pixels = texture2D.GetPixels32();

        for(int i = 0; i< pixels.Length; ++i)
        {
            pixels[i] = _Color;
        }

        texture2D.SetPixels32(pixels);

        texture2D.Apply();
        return texture2D;
    }
    
    

    /// <summary>
    /// 해당 변수를 원래의 Public 형태로 사용하게 만들어줍니다.
    /// </summary>
    /// <param name="_PropName">대상 변수이름</param>
    void UseProperty(string _PropName)
    {
        SerializedProperty prop = serializedObject.FindProperty(_PropName);
        EditorGUI.BeginChangeCheck(); //값이 바뀌는지 검사시작
        EditorGUILayout.PropertyField(prop, true); // 배열까지 싸그리 필드생성되게끔

        if (EditorGUI.EndChangeCheck()) //만약 검사가 끝날무렵 필드에 변화가 생겼다면
        {
            serializedObject.ApplyModifiedProperties(); // 원래 변수에 값 적용
        }
    }

    void UseProperty(string _VarName, string _PropName, string _PropDesc)
    {
        SerializedProperty prop = serializedObject.FindProperty(_VarName);
        EditorGUI.BeginChangeCheck(); //값이 바뀌는지 검사시작
        EditorGUILayout.PropertyField(prop, new GUIContent(_PropName, _PropDesc), true); // 배열까지 싸그리 필드생성되게끔 필드는 커스터마이징

        if (EditorGUI.EndChangeCheck()) //만약 검사가 끝날무렵 필드에 변화가 생겼다면
        {
            serializedObject.ApplyModifiedProperties(); // 원래 변수에 값 적용
        }
    }
}