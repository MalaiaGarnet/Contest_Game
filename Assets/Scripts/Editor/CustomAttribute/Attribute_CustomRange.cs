using UnityEngine;
using UnityEditor;
using System;
[CustomPropertyDrawer(typeof(CustomRange))]
public class Attribute_CustomRange : PropertyDrawer
{
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return EditorGUIUtility.singleLineHeight * 2f;
    }

    public override void OnGUI(Rect _RecPos, SerializedProperty _Prop, GUIContent _Label)
    {
        //EditorGUI.BeginChangeCheck();
        CustomRange range = attribute as CustomRange;

        Rect rangePos = new Rect(_RecPos.x, _RecPos.y, _RecPos.width, _RecPos.height - 15.0f);
        Rect rectPos = new Rect(_RecPos.x, _RecPos.y + 25.0f, _RecPos.width - 5.0f, 20.0f);

        GUIContent[] guis = new GUIContent[2];

        guis[0] = new GUIContent("최소 인식 거리 ", "최소거리 지정");
        guis[1] = new GUIContent("최대 인식 거리 ", "최대거리 지정");

        float[] values = new float[2];
        values[0] = range.Value;    // 최소
        values[1] = range.MaxValue; // 최대
        
        EditorGUI.MultiFloatField(rangePos, guis, values);

        switch (_Prop.propertyType)
        {
            case SerializedPropertyType.Integer:
                EditorGUI.IntSlider(rectPos, _Prop, Convert.ToInt32(values[0]), Convert.ToInt32(values[1]), _Label);
                break;
            case SerializedPropertyType.Float:
                EditorGUI.Slider(rectPos, _Prop, values[0], values[1], _Label);
                break;
            default:
                EditorGUI.LabelField(rectPos, _Label.text, "정수, 실숫값만 지원됩니다.");
                break;
        }
    }
}