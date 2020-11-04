using UnityEngine;

/// <summary>
/// 자주 쓰이는 모노헤비어가 포함된 싱글톤 클래스!
/// </summary>
/// <typeparam name="T">싱글톤을 적용할 타입</typeparam>
public class SingleToneMonoBehaviour<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T _instance;

    /// <summary>
    ///  Inst방식으로 할때, NullExcept 에러 방지용
    /// </summary>
    /// <param name="_Inst"></param>
    public void SetInstance(T _Inst)
    {
        if (_instance == null)
        {
            _instance = _Inst;
        }
        else // 만약에 이미 있다면 스스로 파괴
        {
            Destroy(_Inst.gameObject);
            return;
        }
    }
    public static T Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType(typeof(T)) as T;
                if (_instance == null)
                {
                    Debug.Log("활성화되지 않은 인스턴스 : " + typeof(T) + "이 장면 내에 존재");
                }
            }
            return _instance;
        }
    }
}
