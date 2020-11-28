using UnityEngine;

public class Item_Booster : Item
{
    public Shader Shader_Glow;
    //public TrailRenderer BoostTrail;
    //public AudioSource   SFX;
    public float modifySpeed = 0.15f;

    void Awake()
    {
        itemID = 1004;
        itemType = ItemType.REINFORCE;
        itemName = "레이더";
        itemCount = 0;
        itemDesc = "가드가 주변에 있을 때 아웃라인으로 표시됨";
    }
    public override void Item_Active()
    {
        CharacterController owner = gameObject.GetComponentInParent<CharacterController>();
        owner.moveSpeed *= modifySpeed;
        //BoostTrail.Play();
        //SFX.Play();
    }

    public override void Item_Passive()
    {
        // 라운드 종료
    }
}
