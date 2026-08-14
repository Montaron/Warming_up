using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponHandler : MonoBehaviour
{
    [SerializeField] List<WeaponHandler_data> weaponHandler_data;
    

    // Fields
    private bool wasAttacking;
    private CharacterCombat combat;
    private Dictionary<WeaponType, WeaponHandler_data> dataByType;
    private Dictionary<WeaponType, GameObject> instanceByType;

    private WeaponType currentWeaponType = WeaponType.None;

    private void Awake()
    {
        combat = GetComponent<CharacterCombat>();
        dataByType = new Dictionary<WeaponType, WeaponHandler_data>();
        instanceByType = new Dictionary<WeaponType, GameObject>();

        foreach (var handler in weaponHandler_data)
        {
            GameObject instance = Instantiate(handler.weapon_prefab, handler.idle_socket.position, handler.idle_socket.rotation, handler.idle_socket);
            instance.transform.localPosition = Vector3.zero;
            instance.transform.localRotation = Quaternion.identity;
            instance.SetActive(false);

            dataByType[handler.weaponType] = handler;
            instanceByType[handler.weaponType] = instance;
        }
    }

    private void Start()
    {
        combat.OnSpellStarted += OnSpellStarted;
        combat.OnSpellEnded += OnSpellEnded;
    }

    private void OnSpellEnded(Spell_data data)
    {
        Equip_Weapon_Idle(data.weaponType);
    }

    private void OnSpellStarted(Spell_data data)
    {
        Equip_Weapon_Attack(data.weaponType);
    }

    public void Equip_Weapon_Idle(WeaponType weapon)
    {
        ApplyPose(weapon, isAttacking: false);
    }

    public void Equip_Weapon_Attack(WeaponType weapon)
    {
        ApplyPose(weapon, isAttacking: true);
    }

    private void ApplyPose(WeaponType weapon, bool isAttacking)
    {
        if (currentWeaponType == weapon && wasAttacking == isAttacking)
            return;

        if (instanceByType.TryGetValue(currentWeaponType, out var oldInstance))
            oldInstance.SetActive(false);

        currentWeaponType = weapon;

        if (!dataByType.TryGetValue(weapon, out var handler)) return;
        if (!instanceByType.TryGetValue(weapon, out var instance)) return;

        Transform targetSocket = isAttacking ? handler.attack_socket : handler.idle_socket;
        wasAttacking = isAttacking;
        instance.SetActive(true);
        instance.transform.SetParent(targetSocket, false);
        instance.transform.localPosition = Vector3.zero;
        instance.transform.localRotation = Quaternion.identity;
    }
}

[System.Serializable]
public class WeaponHandler_data
{
    public GameObject weapon_prefab;
    public WeaponType weaponType;
    public Transform idle_socket;
    public Transform attack_socket;
}