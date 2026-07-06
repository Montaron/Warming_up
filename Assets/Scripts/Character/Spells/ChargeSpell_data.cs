using UnityEngine;
[CreateAssetMenu(fileName = "ChargeSpell", menuName = "New Charge Spell")]
public class ChargeSpell_data : Spell_data
{
    public float speedMultiplierStart = 2f;
    public float speedMultiplierMax = 5f;
    public float timeToReachMaxMultiplier = 10f;
    public float chargeDuration = 5f;
    public float damage = 100f;

    public override ISpell CreateSpellRuntime(GameObject caster, GameObject target)
    {
        return  new ChargeSpellRuntime(caster, this, target);
    }
}