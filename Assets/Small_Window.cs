using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Small_Window : MonoBehaviour
{
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
