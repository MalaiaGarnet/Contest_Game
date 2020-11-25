using System.Collections;
using System.Collections.Generic;
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
    }

    IEnumerator SlowMotionSlowyOff(float _Time)
    {
        yield return new WaitForSecondsRealtime(2.0f);
        Time.timeScale += _Time;
    }
}
