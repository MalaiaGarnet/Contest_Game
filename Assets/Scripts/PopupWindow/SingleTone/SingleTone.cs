/// <summary>
/// 자주 쓰이는 싱글톤 클래스!
/// </summary>
/// <typeparam name="T">싱글톤을 적용할 타입</typeparam>
public class SingleTone<T> where T : class, new()
{
    private static T m_Instance = null;

    private static readonly object syslock = new object();

    public static T Instance
    {
        get
        {
            if (m_Instance == null)
            {
                lock (syslock)
                {
                    if (m_Instance == null)
                    {
                        m_Instance = new T();
                    }
                    return m_Instance;
                }
            }
            return m_Instance;
        }
    }
}
