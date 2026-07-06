using System;
using UnityEngine;

public interface IDamageable
{
    public void TakeDamage(DamageData damageData);
    public event Action<DamageData> DamageData;
}

public struct DamageData
{
    public float damage;
    public GameObject attacker;
    public GameObject target;
}