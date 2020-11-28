using System.Collections.Generic;
using UnityEngine.UI;

public class GUI_Widget_Announce : GUI_Widget_Base
{
    public Text announce;
    List<string> msg_list = new List<string>();

    void Start()
    {
        Initialize_Texts();
    }

    void Initialize_Texts()
    {
        msg_list = new List<string>();
        Update_Text();
    }

    void Add_Text(string _msg)
    {
        msg_list.Add(_msg);
        if (msg_list.Count > 5)
            msg_list.RemoveAt(0);
    }

    void Update_Text()
    {
        announce.text = "";
        for (int i = 0; i < msg_list.Count; i++)
            announce.text += msg_list[i];
    }
}
