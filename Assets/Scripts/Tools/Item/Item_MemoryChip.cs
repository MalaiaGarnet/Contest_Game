public class Item_MemoryChip : Item
{
    void Awake()
    {
        itemID = 5;
        itemType = ItemType.SYSTEM;
        itemName = "메모리 칩";
        itemCount = 0;
        itemDesc = "로그가 훔쳐야 하는 물건";
    }
    public override void Item_Active() { }

    public override void Item_Passive()
    {
        itemCount++;
    }
}
