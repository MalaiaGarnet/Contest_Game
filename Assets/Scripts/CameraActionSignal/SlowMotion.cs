using System.Collections;
using UnityEngine;
public class SlowMotion : MonoBehaviour
{
    public void SlowMotionSlowyOn(float _Time)
    {
        Time.timeScale -= _Time;
        StartCoroutine(SlowMotionSlowyOff(0.1f));
    }

    public void SlowMotionOn(float _Time)
    {
        Time.timeScale = _Time;
        StartCoroutine(SlowMotionInstantOff());
    }

    IEnumerator SlowMotionInstantOff()
    {
        yield return new WaitForSecondsRealtime(2.0f);
        Time.timeScale = 1.0f;
        yield return null;
    }

    IEnumerator SlowMotionSlowyOff(float _Time)
    {
        yield return new WaitForSecondsRealtime(2.0f);
        while (Time.timeScale < 1)
            Time.timeScale += _Time;
        yield return null;
    }
}
