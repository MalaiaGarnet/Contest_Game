using System;
using UnityEngine;

[Serializable]
public abstract class Item : MonoBehaviour
{
    public ushort   itemID;
    public ItemType itemType;
    public string   itemName;
    public string   itemDesc;
    public bool     isUseEnergy;
    public byte     itemCount;
    public ushort   itemEnergy;

    public Item() { }
    public Item(ushort _ID,
                ItemType _Type,
                string _Name,
                string _Desc,
                byte _Count,
                bool _UseEnergy = false,
                ushort _Energy = 0)
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