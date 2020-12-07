using Cinemachine;
using System.Collections;
using UnityEngine;
using UnityEngine.Playables;
using Greyzone.GUI;

/// <summary>
/// 데스 카메라 컨트롤 클래스입니다.
/// 
/// </summary>
public class DeathCam : SingleToneComponent<DeathCam>
{
    public PlayableDirector PlayableDirector;
    public CinemachineVirtualCamera[] VCams = new CinemachineVirtualCamera[4];

    public Transform Killer { get; protected set; }
    public Transform DeadPlayer { get; protected set; }

    public CinemachineBrain DeathCamBrain;

    enum VCamIndex
    {
        BACK,
        FRONT,
        LEFT,
        RIGHT
    }

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        PlayableDirector.gameObject.SetActive(false);
        ActiveDeathCamVirtualCameras();
    }

    public void FindSetDeathPlayer(Transform _Ragdoll)
    {
        BindTarget(0, Killer, _Ragdoll);    //Back
        BindTarget(1, _Ragdoll, Killer);    //Front
        BindTarget(2, Killer, _Ragdoll);    //Left
        BindTarget(3, _Ragdoll, Killer);    //Right

        DeadPlayer = _Ragdoll;
    }

    public void FindSetKillPlayer(Transform _Killer)
    {
        Killer = _Killer;
    }

    void BindTarget(byte _CamIndex, Transform _LookAt, Transform _Follow)
    {
        VCams[_CamIndex].LookAt = _LookAt;
        VCams[_CamIndex].Follow = _Follow;
    }

    void ActiveDeathCamVirtualCameras()
    {
        for (byte index = 0; index < VCams.Length; index++)
        {
            VCams[index].VirtualCameraGameObject.SetActive(true);
        }
    }

    public void StartCam()
    {
        // 켜지면 이 카메라가 발격 (더 좋은 방법 떠오르기전 까진 일단 이렇게...)
        DeathCamBrain.gameObject.SetActive(true);
        PlayableDirector.gameObject.SetActive(true);
        PlayableDirector.Play();
        TooltipManager.Instance.tooltip_HeadMessage.ShowMessage(MessageStyle.ON_HEAD_MSG, "NO SIGNAL...", DeadPlayer.localPosition);
        StartCoroutine(EndCam());
    }

    IEnumerator EndCam()
    {
        yield return new WaitForSeconds((float)PlayableDirector.duration);
        DeathCamBrain.gameObject.SetActive(false);
        PlayableDirector.gameObject.SetActive(false);
        PlayableDirector.Stop();
        TooltipManager.Instance.tooltip_HeadMessage.HideMessage();
        yield return null;
    }
}
