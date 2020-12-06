using System.Collections;
using System;
using UnityEngine;

namespace Greyzone.GUI
{
    public class TooltipManager : SingleToneMonoBehaviour<TooltipManager>
    {
        public ToolTip tooltip_HeadMessage;
        public ToolTip tooltip_ScreenMsg;

        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
        }

        void Start()
        {
            tooltip_HeadMessage.gameObject.SetActive(false);
            tooltip_ScreenMsg.gameObject.SetActive(false);
        }

        public void InvokeTooltip(Action<ToolTip> _ToolTipAction, MessageStyle _MsgStyle)
        {
            switch (_MsgStyle)
            {
                case MessageStyle.NORMAL_MSG:
                    break;
                case MessageStyle.ON_HEAD_MSG:
                    _ToolTipAction.Invoke(tooltip_HeadMessage);
                    break;
                case MessageStyle.ON_SCREEN_UP_MSG:
                    _ToolTipAction.Invoke(tooltip_ScreenMsg);
                    break;

            }
        }
    }

    public struct TooltipInfo
    {
        public ToolTip toolTip;
        public MessageStyle msgStyle;
        public string  Desc;
        public Vector3 msgHeadPos;
    }
}