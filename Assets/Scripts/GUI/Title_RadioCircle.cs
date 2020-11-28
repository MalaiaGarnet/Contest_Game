using UnityEngine;
using UnityEngine.UI;

public class Title_RadioCircle : MonoBehaviour
{
    public RectTransform[] m_Circles;

    public int Level { get; set; } = 0;

    private void Update()
    {
        for (int i = 0; i < m_Circles.Length; i++)
        {
            Image circle_image = m_Circles[i].gameObject.GetComponent<Image>();
            Color color = circle_image.color;
            circle_image.color = Color.Lerp(color, new Color(1f, 1f, 1f, i < Level ? 0.4f : 0.0f), i < Level ? 0.02f : 0.05f);

            float turning_angle = (8f * Time.deltaTime) * (i % 2 == 0 ? 1f : -1f);
            Vector3 rot = m_Circles[i].localRotation.eulerAngles;
            m_Circles[i].localRotation = Quaternion.Euler(rot + new Vector3(0f, 0f, turning_angle));
        }
    }
}
