using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatsComponent : MonoBehaviour
{
    [SerializeField] private Stats_data stats_ini;
    public Stats_data stats_current { get; private set; }

    public event Action OnHealthZero;
    // Start is called abefore the first frame update
    void Start()
    {
               stats_current = Instantiate(stats_ini); 
    }

    public void ReduceHealth(float damage)
    {
        stats_current.health -= damage;
        if (stats_current.health <= 0)
        {
            Debug.Log(GetCurrentHealth());
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
}
