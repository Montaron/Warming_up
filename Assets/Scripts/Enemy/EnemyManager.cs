using System;
using UnityEngine;

public class EnemyManager : MonoBehaviour, IDamageable
{
    private StatsComponent statsComponent;

    public event Action<DamageData> DamageData;

    void Awake()
    {
        statsComponent = GetComponent<StatsComponent>();
    }
    public void TakeDamage(DamageData damageData)
    {
        statsComponent.ReduceHealth(damageData.damage);
    }
    void OnEnable()
    {
        statsComponent.OnHealthZero += HandleZeroHealth;
    }
    void OnDisable() 
    {
        statsComponent.OnHealthZero -= HandleZeroHealth;        
    }

    private void HandleZeroHealth()
    {
        Debug.Log("Should be destroyed");
        Destroy(this.gameObject);
    }
}