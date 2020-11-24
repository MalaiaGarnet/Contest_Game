using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class SlowMotion : MonoBehaviour
{
    public void SlowMotionSlowyOn(float _Time)
    {
        Time.timeScale -= _Time;
    }

    public void SlowMotionOn(float _Time)
    {
        Time.timeScale = _Time;
    }

    public void SlowMotionInstantOff()
    {
        Time.timeScale = 1.0f;
    }

    public void SlowMotionSlowyOff(float _Time)
    {
        Time.timeScale += _Time;
    }
}
