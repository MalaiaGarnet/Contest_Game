using System.Linq;
using UnityEngine;

public class Item_AntiTrap : Item
{
    public bool IsUseRange = false;
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
        Vector3 ownerpos = GetComponentInParent<GameObject>().transform.position;

        if (IsUseRange)
        {
            Collider[] colliders = Physics.OverlapSphere(ownerpos, Range);

            Collider collider;
            if (colliders.Any((x) => (x.GetComponent<Item>().itemType == ItemType.TRAP)))
            {
                for (int i = 0; i < colliders.Length; i++)
                {
                    if (colliders[i].CompareTag("Trap"))
                    {
                        // 함정이 무력화 되었다는 문구 띄워보면 어떨까..
                        Destroy(colliders[i]);
                    }
                }
            }
        }
        else
        {
            // 젠장 함정무력화를 어캐처리한담

        }//Physics.OverlapSphere(, Range).Any((x) => x.CompareTag("Trap"))
    }

    public override void Item_Passive()
    {

    }
}