using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// TODO : 요기 조만간 수정좀 해봐야할거같네요
public class Character_SoundPlayer : MonoBehaviour
{
    public AudioClip m_FootSound;

    public AudioSource m_LeftFoot, m_RightFoot, m_Torso;
    public AudioSource m_Drone_FrontFoot, m_Drone_BackFoot;

    public void FootSound(string _pos)
    {
        AudioSource source = null;
        switch (_pos)
        {
            case "left":
                source = m_LeftFoot;
                break;
            case "right":
                source = m_RightFoot;
                break;
            case "front":
                source = m_Drone_FrontFoot;
                break;
            case "back":
                source = m_Drone_BackFoot;
                break;
            default:
                source = m_Torso;
                break;
        }
        if (source == null) // 소스를 못 찾았으면 패스
            return;
        // if (source.isPlaying) // 해당 소스에서 이미 소리를 들려주고 있다면 패스
        //    return;

        source.clip = m_FootSound;
        source.Play();
    }
}
