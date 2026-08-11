using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatsComponent : MonoBehaviour, IDamageable
{
    [SerializeField] private Stats_data stats_ini;
    public Stats_data stats_current { get; private set; }

    // Events
    public event Action OnHealthZero;
    public event Action<DamageData> DamageData;
    void Start()
    {
               stats_current = Instantiate(stats_ini); 
    }

    public void ReduceHealth(float damage)
    {
        stats_current.health -= damage;
        if (stats_current.health <= 0)
        {
            // Debug.Log(GetCurrentHealth());
            Die();
        }
    }

    public float GetCurrentHealth()
    {
        return stats_current.health;
    }

    void Die()
    {
        OnHealthZero?.Invoke();
    }
    
    public void TakeDamage(DamageData damageData)
    {
        ReduceHealth(damageData.damage);
    }

}
