using Cinemachine;
using System.Collections;
using UnityEngine;
using UnityEngine.Playables;
using Greyzone.GUI;

/// <summary>
/// 데스 카메라 컨트롤 클래스입니다.
/// 
/// </summary>
public class DeathCam : SingleToneMonoBehaviour<DeathCam>
{
    public GameObject DeathCamTimeline;
    private PlayableDirector playableDirector;
    public CinemachineVirtualCamera[] VCams = new CinemachineVirtualCamera[4];

    public Transform Killer;
    public Transform DeadPlayer;

    private CinemachineBrain DeathCamBrain;
    public GameObject DeathCamBrainObject;

    IEnumerator Start()
    {
        yield return null;
        playableDirector = DeathCamTimeline.GetComponent<PlayableDirector>();
        DeathCamBrain = DeathCamBrainObject.GetComponent<CinemachineBrain>();
    }

    public void FindSetDeathPlayer(ref Transform _Ragdoll)
    {
        BindTarget(0, ref Killer, ref _Ragdoll);    //Back
        BindTarget(1, ref _Ragdoll, ref Killer);    //Front
        BindTarget(2, ref Killer, ref _Ragdoll);    //Left
        BindTarget(3, ref _Ragdoll, ref Killer);    //Right

        DeadPlayer = _Ragdoll;
    }

    public void FindSetKillPlayer(ref Transform _Killer)
    {
        Killer = _Killer;
    }

    void BindTarget(byte _CamIndex, ref Transform _LookAt, ref Transform _Follow)
    {
        VCams[_CamIndex].LookAt = _LookAt;
        VCams[_CamIndex].Follow = _Follow;
    }

    void ActiveDeathCamVirtualCameras()
    {
        for (byte index = 0; index < VCams.Length; index++)
        {
            if (DeathCamBrain != null)
            {
                VCams[index] = DeathCamBrain.ActiveVirtualCamera as CinemachineVirtualCamera;
                VCams[index].VirtualCameraGameObject.SetActive(true);
            }
        }
    }

    public void InvokeDeathCamera(ref Transform _Killer, ref Transform _Target)
    {
        gameObject.SetActive(true);
        FindSetKillPlayer(ref _Killer);
        FindSetDeathPlayer(ref _Target);
        DeathCamBrainObject.GetComponentInParent<GameObject>().SetActive(true);
        DeathCamBrainObject.SetActive(true);
        DeathCamTimeline.GetComponentInParent<GameObject>().SetActive(true);
        DeathCamTimeline.SetActive(true);
        StartCam();
    }

    public void StartCam()
    {
        // 켜지면 이 카메라가 발격 (더 좋은 방법 떠오르기전 까진 일단 이렇게...)
        Debug.Log("이쪽으로 갔다");
        ActiveDeathCamVirtualCameras();
        playableDirector.Play();
        TooltipManager.Instance.tooltip_HeadMessage.ShowMessage(MessageStyle.ON_HEAD_MSG, "NO SIGNAL...", DeadPlayer.localPosition);
        StartCoroutine(EndCam());
    }

    IEnumerator EndCam()
    {
        yield return new WaitForSeconds((float)playableDirector.duration);
        playableDirector.Stop();
        TooltipManager.Instance.tooltip_HeadMessage.HideMessage();
        DeathCamBrainObject.SetActive(false);        
        DeathCamTimeline.SetActive(false);
        DeathCamBrainObject.GetComponentInParent<GameObject>().SetActive(false);
        DeathCamTimeline.GetComponentInParent<GameObject>().SetActive(false);
        gameObject.SetActive(false);
        yield return null;
    }
}
