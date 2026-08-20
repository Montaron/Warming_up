using UnityEngine;

[CreateAssetMenu(fileName = "DashSpell", menuName = "New Dash Spell")]
public class DashSpell_data : Spell_data
{
    public float DashSpeed;

    public override ISpell CreateSpellRuntime(GameObject caster, GameObject target)
    {
        return new DashSpellRuntime(caster, this);
    }
}