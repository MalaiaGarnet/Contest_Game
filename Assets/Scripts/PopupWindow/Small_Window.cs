using UnityEngine;
using UnityEngine.UI;

public class Small_Window : MonoBehaviour
{
    public Button m_CloseButton;
    public Animator m_Animator;

    public void Close()
    {
        m_Animator.SetTrigger("Deactivate");
    }

    public void Deactive()
    {
        gameObject.SetActive(false);
    }
}
