using UnityEngine;
public abstract class ProjectileSpell_data : Spell_data
{
    public float damage = 100f;
    public float projectile_speed = 1f;
    public float projectileSpeed;
    public float projectileSpawnOffset;
    public float projectileLifetime;
    public GameObject projectilePrefab;

}