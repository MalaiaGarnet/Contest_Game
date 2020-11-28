using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Widget_IP_Reminder : MonoBehaviour
{
    public InputField IP_Input;

    // Start is called before the first frame update
    IEnumerator Start()
    {
        while (Manager_Network.Instance == null)
            yield return new WaitForFixedUpdate();

        if (PlayerPrefs.HasKey("Saved_IP"))
            IP_Input.SetTextWithoutNotify(PlayerPrefs.GetString("Saved_IP"));
        Manager_Network.Instance.Change_IP(IP_Input.text);

        yield return null;
    }

    public void onTextChange(string _text)
    {
        PlayerPrefs.SetString("Saved_IP", _text);
    }
}
