using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Round_Indicator : MonoBehaviour
{
    public Text m_RoundText;

    public void Start_Round(int _round)
    {
        // 텍스트 대충 수정
        m_RoundText.text = "ROUND " + _round + "\nSTART";
        gameObject.SetActive(true);

        StartCoroutine(Show_Ingame_Round_Indicator());
    }
    public void End_Round(int _round)
    {
        // 텍스트 대충 수정
        m_RoundText.text = "ROUND " + _round + "\nFINISHED";
        gameObject.SetActive(true);

        StartCoroutine(Show_Ingame_Round_Indicator());
    }
    public void End_Game()
    {
        // 텍스트 대충 수정
        m_RoundText.text = "GAME ENDED";
        gameObject.SetActive(true);

        StartCoroutine(Show_Ingame_Round_Indicator());
    }

    public IEnumerator Show_Ingame_Round_Indicator()
    {
        // 아니메 끝날 때까지 대기
        while (GetComponent<Animation>().isPlaying)
            yield return new WaitForEndOfFrame();

        // 이후 오브젝트 꺼버리기
        gameObject.SetActive(false);
        yield return null;
    }
}
