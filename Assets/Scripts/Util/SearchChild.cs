using UnityEngine;

public class SearchChild
{
    static public Transform GetChildName(Transform _ParentTrans, string _ChildName)
    {
        for (int index = 0; index < _ParentTrans.childCount; index++)
        {
            if (_ParentTrans.GetChild(index).name == _ChildName)
            {
                return _ParentTrans.GetChild(index);
            }
        }
        return null;
    }
}

