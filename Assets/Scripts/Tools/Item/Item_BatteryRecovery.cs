using UnityEngine;

public class Item_BatteryRecovery : Item
{
    public ushort chargeAmount;
    void Awake()
    {
        itemID = 4;
        itemType = ItemType.SYSTEM;
        itemName = "충전소";
        itemCount = 0;
        itemDesc = "배터리 충전 오브젝트";
    }
    public override void Item_Active()
    {
    }

    public override void Item_Passive()
    {
    }

    private void OnTriggerEnter(Collider other)
    {
        CharacterController character = other.gameObject.GetComponentInParent<CharacterController>();
        if (character.m_MyProfile.Battery < character.battery)
        {
            character.m_MyProfile.Battery += chargeAmount;
        }
    }
}