using System;
using UnityEngine;

[Serializable]
public abstract class Item : Tool
{
    public ushort   itemID;
    public ItemType itemType;
    public string   itemName;
    public string   itemDesc;
    public bool     isUseEnergy;
    public byte     itemCount;
    public ushort   itemEnergy;

    public bool     isActive;

    public Item() { }
    public Item(ushort _ID,
                ItemType _Type,
                string _Name,
                string _Desc,
                byte _Count,
                bool _UseEnergy = false,
                ushort _Energy = 0,
                bool _IsActive = false)
    {
        itemID = _ID;
        itemType = _Type;
        itemName = _Name;
        itemDesc = _Desc;
        itemCount = _Count;
        isUseEnergy = _UseEnergy;
        if(isUseEnergy)
        {
            itemEnergy = _Energy;
        }
        isActive = _IsActive;
    }

    void Start()
    {
        if (!isActive)
            Item_Passive();
    }
    public override void onFire(bool _pressed)
    {
        if (!_pressed)
            return;
        Item_Active();
    }

    public override void onInteract(bool _pressed)
    {
        
    }
    public abstract void Item_Active();
    public abstract void Item_Passive();
}

[Flags]
public enum ItemType
{
    NONE = 0,
    SYSTEM,
    REINFORCE,
    TRAP,
    SECURITY
}