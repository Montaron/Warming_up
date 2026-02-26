using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon Data", menuName = "Weapon Data")]
public class WeaponData : ScriptableObject
{
    public GameObject weaponPrefab;
    public string weaponName;
    public int damage;

    public Vector3 gripPositionOffset;
}
