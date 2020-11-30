using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using System.Threading.Tasks; 

namespace Greyzone.GUI
{
    public class ToolTip : MonoBehaviour
    {
        [Header("메시지 텍스트")]
        public TextMeshPro msg_TMP;
        public Text        msg_Normal;
        public Texture2D   msgBackGround;
        public bool        useTMPMsg;

        [Header("페이드")]
        public float       fadeDuration; // 노출시간
        public float       fadeOpacity;  // 투명도

        [Header("소리")]
        public AudioSource tooltipSound;

        // Use this for initialization
        void Start()
        {
            
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void ShowMessage(MessageStyle _MsgStyle)
        {
            switch(_MsgStyle)
            {
                case MessageStyle.NORMAL_MSG:
                    break;
                case MessageStyle.ON_HEAD_MSG:
                    break;
                case MessageStyle.ON_SCREEN_UP_MSG:
                    break;
            }
        }

        /// <summary>
        /// 머리위에 메시지를 띄웁니다.
        /// </summary>
        /// <param name="_TargetHead"></param>
        public void DrawOnHeadMessage(GameObject _TargetHead)
        {
            if (useTMPMsg)
            {
                msg_TMP.transform.position = _TargetHead.transform.position; // 머리위에 오게 ㅇㅇ
            }
            else
            {
                msg_Normal.transform.position = _TargetHead.transform.position; // 머리위에 오게 ㅇㅇ
            }
            //MessageFadeOn();
        }

        async void MessageFadeOn()
        {
            await Task.Delay((int)fadeDuration);
        }

        private void OnDestroy()
        {
            if (gameObject != null)
                Destroy(gameObject);
        }
    }

    [Flags]
    public enum MessageStyle
    {
        NORMAL_MSG,
        ON_HEAD_MSG,
        ON_SCREEN_UP_MSG,
    }
}