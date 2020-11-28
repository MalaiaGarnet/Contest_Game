using Cinemachine;
using Devcat;
using System.Collections;
using UnityEngine;
using UnityEngine.Playables;

/// <summary>
/// 데스 카메라 컨트롤 클래스입니다.
/// 
/// </summary>
public class DeathCam : MonoBehaviour
{
    public PlayableDirector PlayableDirector;
    public CinemachineVirtualCamera[] VCams = new CinemachineVirtualCamera[4];
    public GameObject Guaurd;
    public CinemachineBrain DeathCamBrain;

    enum VCamIndex
    {
        BACK,
        FRONT,
        LEFT,
        RIGHT
    }

    void Start()
    {
        // 1인칭 카메라랑 일단 분리
        DeathCamBrain.enabled = false;
    }

    public void EnumValueChange()
    {

    }
    public void StartCam()
    {
        int back = 0;
        var value = new EnumDictionary<VCamIndex, int>()
        {
            { VCamIndex.BACK, 0},
            { VCamIndex.FRONT, 1 },
            { VCamIndex.LEFT, 2 },
            { VCamIndex.RIGHT, 3 }
        };

        // 켜지면 이 카메라가 발격 (더 좋은 방법 떠오르기전 까진 일단 이렇게...)
        DeathCamBrain.enabled = true;
        VCams[0].LookAt = Guaurd.transform;
        VCams[3].Follow = Guaurd.transform;
        PlayableDirector.Play();
        StartCoroutine(EndCam());
    }

    IEnumerator EndCam()
    {
        yield return new WaitForSeconds((float)PlayableDirector.duration);
        DeathCamBrain.enabled = false;
        yield return null;
    }
}
