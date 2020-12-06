using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using System.Collections;
using System.Threading.Tasks;

namespace Greyzone.GUI
{
    public class ToolTip : MonoBehaviour
    {
        [Header("메시지 텍스트")]
        public TextMeshPro msg_TMP;
        public TextMeshProUGUI msg_TMP_UI;
        public Text        msg_Normal;
        public GameObject  msgBackGround;
        public bool        useTMPMsg;

        [Header("페이드")]
        public float       fadeDuration; // 노출시간
        public float       fadeOpacity;  // 투명도

        [Header("소리")]
        public AudioSource tooltipSound;
        public void ShowMessage(MessageStyle _MsgStyle, string _Desc, Vector3 _TargetPos = default)
        {
            switch(_MsgStyle)
            {
                case MessageStyle.NORMAL_MSG:
                    msg_Normal.text = _Desc;
                    break;
                case MessageStyle.ON_HEAD_MSG:
                    gameObject.transform.localPosition = _TargetPos;
                    msgBackGround.SetActive(true);
                    msg_TMP.text = _Desc;
                    msg_TMP.CrossFadeAlpha(fadeOpacity, fadeDuration, ignoreTimeScale: false);
                    tooltipSound.PlayOneShot(tooltipSound.clip);
                    break;
                case MessageStyle.ON_SCREEN_UP_MSG:
                    msgBackGround.GetComponent<Image>().CrossFadeAlpha(fadeOpacity, fadeDuration, ignoreTimeScale: false);
                    msg_TMP_UI.text = _Desc;
                    msg_TMP_UI.CrossFadeAlpha(fadeOpacity, fadeDuration, ignoreTimeScale: false);
                    tooltipSound.PlayOneShot(tooltipSound.clip);
                    break;                 
            }
        }


        public void ViewSideInItemMessage(Item _Item, Vector3 _PlayerPos, float _Dist)
        {
            transform.localPosition = _Item.transform.localPosition;

            msg_TMP.text = _Item.itemName + "\n\n" + "습득하기";

            StartCoroutine(UpdateItemMessagePos(_Item, _PlayerPos, _Dist));     
        }



        #region 코루틴
        IEnumerator UpdateItemMessagePos(Item _Item, Vector3 _PlayerPos, float _Dist)
        {
            yield return new WaitForSeconds(0.5f);
            while (true)
            {
                if (Vector3.Distance(_PlayerPos, _Item.transform.localPosition) < _Dist)
                {
                    this.gameObject.SetActive(true);
                    //tooltipSound.PlayOneShot(tooltipSound.clip);
                    yield return new WaitForSeconds(0.5f);
                }
                else
                {
                    this.gameObject.SetActive(false);
                    break;
                }
            }

        }
        #endregion


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