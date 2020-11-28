using UnityEngine;

public class GUI_Widget_Projection : MonoBehaviour
{
    bool Is_Projection;
    MeshRenderer[] m_MapRenderers;
    Minimap m_Minimap;

    bool Initialized = false;

    private void Start()
    {
        if (Manager_Ingame.Instance.m_DebugMode)
            Initialize();
    }

    public void Initialize()
    {
        Manager_Ingame.Instance.e_RoundUpdate.AddListener(Update_Map);
        Manager_Input.Instance.e_Input_Projection.AddListener(Toggle_Projection);
    }

    void Update_Map(GameObject _map, Minimap _minimap)
    {
        Debug.Log("맵 업데이트됨");
        m_MapRenderers = _map.GetComponentsInChildren<MeshRenderer>();
        m_Minimap = _minimap;
        Initialized = true;
    }

    public void Toggle_Projection(bool _enable)
    {
        Debug.Log("프로젝션 토글 - " + Is_Projection);
        if (!Initialized)
            return;

        Is_Projection = _enable;

        foreach (MeshRenderer mr in m_MapRenderers)
            mr.enabled = !Is_Projection;

        m_Minimap.gameObject.SetActive(Is_Projection);
    }
}
