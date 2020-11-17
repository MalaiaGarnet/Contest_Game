using UnityEngine;
using System;
using Boo.Lang;
using System.Linq;

public class Item_AntiTrap : Item
{
    public float Range; 
    public void Awake()
    {
        itemID = 1002;
        itemType = ItemType.REINFORCE;
        itemName = "안티";
        itemCount = 0;
        itemDesc = "함정 무력화";      
    }
    public override void Item_Active()
    {
        if(Physics.OverlapSphere(GetComponentInParent<GameObject>().transform.position, Range).Any((x) => x.CompareTag("Trap")))
        {
           
        }
    }

    public override void Item_Passive()
    {
        
    }
}