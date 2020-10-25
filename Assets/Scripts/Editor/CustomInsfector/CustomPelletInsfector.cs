using UnityEngine;
using UnityEditor;

[CanEditMultipleObjects]
[CustomEditor(typeof(Weapon_StunGun), true)]
public class CustomPelletInsfector : Editor
{
    Weapon_StunGun m_ShotGunInsf;

    void OnEnable()
    {
        // target은 위의 CustomEditor() 애트리뷰트에서 설정해 준 타입의 객체에 대한 레퍼런스
        // object형이므로 실제 사용할 타입으로 캐스팅 해 준다.
        m_ShotGunInsf = target as Weapon_StunGun;
    }

    /// <summary>
    /// 커스텀에디터 구현 함수 재 정의.
    /// 인스펙터에서 보여지는 정보를 여기서 고쳐줍니다.
    /// </summary>
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if(m_ShotGunInsf.m_IsDebug)
        {
            EditorGUILayout.LabelField("스턴 딜레이", m_ShotGunInsf.stunDuration.ToString());
        }
        if (GUI.changed)
        {
            EditorUtility.SetDirty(target);
        }
    }

    void DrawScript()
    {
        MonoScript script = null;
        script = EditorGUILayout.ObjectField(script, typeof(MonoScript), false) as MonoScript;
    }

    void DrawProperty(string _PropName)
    {
        serializedObject.Update();
        SerializedProperty prop = serializedObject.FindProperty(_PropName);
        EditorGUILayout.PropertyField(prop, true, new GUILayoutOption[0]);
        serializedObject.ApplyModifiedProperties();
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
}