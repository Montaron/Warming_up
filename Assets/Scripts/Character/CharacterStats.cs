using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    [SerializeField] private WeaponData weaponData_source;
    [SerializeField] private CharacterStatsData characterStatsData_source;
    public CharacterStatsData characterStatsData { get; private set; }

    public event Action OnCharacterDeath;
    // Start is called abefore the first frame update
    void Start()
    {
               characterStatsData = Instantiate(characterStatsData_source); 
    }

    public void EquipWeapon(WeaponData weaponData)
    {
        weaponData_source = weaponData;
    }

    public void TakeDamage(int damage)
    {
        characterStatsData.health -= damage;
        if (characterStatsData.health <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        OnCharacterDeath?.Invoke();
    }
}
