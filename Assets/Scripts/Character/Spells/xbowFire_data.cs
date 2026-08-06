using UnityEngine;

[CreateAssetMenu(fileName = "FireArrowSpell", menuName = "New FireArrow Spell")]
public class xbowFire_data : ProjectileSpell_data
{
    public override ISpell CreateSpellRuntime(GameObject caster, GameObject target)
    {
        return new xbowFireSpellRuntime(caster, this, target);
    }
}