using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scene_Lobby : MonoBehaviour
{
    public Scene_Title s_Title;
    public RectTransform[] m_Circles;
    bool m_Activated = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Logout()
    {
        Manager_Network.Instance.Logout();
        s_Title.gameObject.SetActive(true);
        gameObject.SetActive(false);
    }
}
