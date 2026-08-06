using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellProjectile : MonoBehaviour
{
    private Vector3 direction;
    private float speed;
    private float lifetime;
    private float elapsed;

    public void Initialize(Vector3 direction, float speed, float lifetime = 5f)
    {
        this.direction = direction.normalized;
        this.speed = speed;
        this.lifetime = lifetime;
        this.elapsed = 0f;
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
        // Hit resolution (damage, VFX, etc.) goes here
        Destroy(gameObject);
    }
}
