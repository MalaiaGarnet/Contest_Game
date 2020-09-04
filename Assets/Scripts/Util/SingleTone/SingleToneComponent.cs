using UnityEngine;

/// <summary>
/// 유니티 컴포넌트들을 싱글톤으로 구현하는 클래스
/// </summary>
/// <typeparam name="T"></typeparam>
public class SingleToneComponent<T> : MonoBehaviour where T : SingleToneComponent<T>
{
    private static T m_Instance;

    public static T Instance
    {
        get
        {
            if (m_Instance == null)
            {
                GameObject gameObject = new GameObject(typeof(T).Name, typeof(T));
                m_Instance = gameObject.GetComponent<T>();
                DontDestroyOnLoad(m_Instance.gameObject);
            }
            return m_Instance;
        }
    }
}