using UnityEngine;
using Debug = UnityEngine.Debug;
using PopupWindow = CustomPopupWindow;

public class TestCall : MonoBehaviour
{
    public void _On_TestPopupWindowCall()
    {
        PopupWindow.Show("테스트!", "첫메시지박스에요.", TestCallBackFunction);
    }

    void TestCallBackFunction(PopUpResult OutResul)
    {
        Debug.LogFormat("값 확인용 : {0}", OutResul);
    }
}
