using System;
using UnityEngine;
public interface IDamageAble
{
    int Helath { get; set; }
    void OnTakeDamage(GameObject _Attacker, RaycastHit _Victim);
    void OnDeallingDamage(float _Damage);
}

