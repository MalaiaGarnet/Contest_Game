using System.Collections;
using UnityEngine;

public class Item_Bind : Item
{
    //public AudioSource   SFX;
    public float duration = 8.0f;

    void Awake()
    {
        itemID = 2001;
        itemType = ItemType.TRAP;
        itemName = "바인드";
        itemCount = 0;
        itemDesc = "8초간 발이 묶임";
    }
    public override void Item_Active()
    {
        CharacterController owner = gameObject.GetComponentInParent<CharacterController>();
        StartCoroutine(RecoverySpeed(owner));
    }

    IEnumerator RecoverySpeed(CharacterController _Owner)
    {
        float oriSpeed = _Owner.moveSpeed;
        _Owner.moveSpeed = 0;
        yield return new WaitForSeconds(duration);
        _Owner.moveSpeed = oriSpeed;
        yield return null;
    }

    public override void Item_Passive()
    {
        // 라운드 종료
    }
}
