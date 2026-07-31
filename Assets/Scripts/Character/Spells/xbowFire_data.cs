using UnityEngine;
[CreateAssetMenu(fileName = "FireArrowSpell", menuName = "New FireArrow Spell")]
public class xbowFire_data : Spell_data
{
    public float damage = 100f;
    public float projectile_speed = 1f;

    public GameObject projectilePrefab;

    public override ISpell CreateSpellRuntime(GameObject caster, GameObject target)
    {
        return new xbowFireSpellRuntime(caster, this, target);
    }
}