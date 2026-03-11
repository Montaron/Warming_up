using UnityEngine;

public static class ComponentUtils
{
    public static bool TryGetDamageable(Collider collider, out IDamageable damageable)
    {
        if (!collider.TryGetComponent<IDamageable>(out damageable))
            damageable = collider.GetComponentInParent<IDamageable>();
        return damageable != null;
    }
}