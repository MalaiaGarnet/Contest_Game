using UnityEngine;

public class Manager_Tool : SingleTone<Manager_Tool>
{
    /// <summary>
    /// 툴 프리팹 취득<br/>
    /// 모든 툴 오브젝트는 Resources/Prefab/Tools 에 등록한다<br/>
    /// 프리팹의 이름은 Tool_[OID] 로 등록하시오 (Tool_4001, Tool_2001 등)
    /// </summary>
    /// <param _oid="_oid">오브젝트 ID</param>
    /// <returns></returns>
    public GameObject Get_Tool_Prefab(ushort _oid)
    {
        if (_oid == 0)
            return null;

        return Resources.Load<GameObject>("Prefabs/Tools/Tool_" + _oid);
    }
}
