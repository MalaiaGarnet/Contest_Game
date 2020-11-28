using UnityEngine;

public class Master : MonoBehaviour
{
    public static Master Instance;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        Application.targetFrameRate = 59;
    }
}
