using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Foot_Sound : MonoBehaviour
{
    AudioSource m_Audio;

    void Start()
    {
        m_Audio = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider other)
    {
        // if (!m_Audio.isPlaying)
            m_Audio.Play();
    }

}
