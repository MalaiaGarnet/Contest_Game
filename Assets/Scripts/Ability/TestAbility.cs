using UnityEngine;

public class TestAbility : MonoBehaviour
{
    private void Awake()
    {
        float _FirstTime = 3.0f;

        while (_FirstTime > 0.0f)
        {
            _FirstTime -= Time.deltaTime;
            Debug.Log(Mathf.Lerp(1.0f, 0.0f, _FirstTime / 3.0f));
        }
    }
}
