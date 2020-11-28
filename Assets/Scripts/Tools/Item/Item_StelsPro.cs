using UnityEngine;

public class Item_StelsPro : Item
{
    public Shader Shader_Glow;
    public float dist;

    void Awake()
    {
        itemID = 1003;
        itemType = ItemType.REINFORCE;
        itemName = "스텔스 프로";
        itemCount = 0;
        itemDesc = "가드로부터 위치 노출 방지";
    }
    public override void Item_Active()
    {
        CharacterController owner = gameObject.GetComponentInParent<CharacterController>();
        Vector3 ownerpos = owner.m_MyProfile.Current_Pos;

    }

    public override void Item_Passive()
    {
        // 라운드 종료
    }
}
