using UnityEngine;
using UnityEngine.Playables;
using Cinemachine;

public class DeathCam : MonoBehaviour
{
    public PlayableDirector PlayableDirector;
    public CinemachineVirtualCamera VirtualCameraLeft;
    public CinemachineVirtualCamera VirtualCameraFront;
    public GameObject Guaurd;

    public void StartCam()
    {
        VirtualCameraLeft.LookAt = Guaurd.transform;
        VirtualCameraFront.Follow = Guaurd.transform;
        PlayableDirector.Play();
    }
}
