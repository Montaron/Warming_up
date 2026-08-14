using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellProjectile : MonoBehaviour
{
    private GameObject source;
    private Vector3 direction;
    private float speed;
    private float lifetime;
    private float elapsed;
    private float damage;

    public void Initialize(Vector3 direction, ProjectileSpell_data data, GameObject caster, float damage)
    {
        this.direction = direction.normalized;
        speed = data.projectileSpeed;
        lifetime = data.projectileLifetime;
        elapsed = 0f;
        this.damage = damage;
        source = caster;
    }

    private void Update()
    {
        transform.position += direction * speed * Time.deltaTime;

        elapsed += Time.deltaTime;
        if (elapsed >= lifetime)
            Destroy(gameObject);
    }
    private void OnTriggerEnter(Collider other) 
    {
        if (other.gameObject == source)
            return;
        if (ComponentUtils.TryGetDamageable(other, out IDamageable damageable))
        {
            var damageData = new DamageData
            {
                damage = damage,
                attacker = source,
                target = other.gameObject,
            };
            damageable.TakeDamage(damageData);
        }
        Destroy(gameObject);
    }
}