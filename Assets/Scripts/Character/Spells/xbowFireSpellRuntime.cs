using UnityEngine;
using System.Collections;
using System;

public class xbowFireSpellRuntime : BaseSpellRuntime
{
    public xbowFire_data xbowfire_data { get; private set; }
    protected override bool loopLoopPhase => true;
    private GameObject target;

    public xbowFireSpellRuntime(GameObject caster, xbowFire_data data, GameObject target)
            : base(caster, data)
    {
        if (target != null)
        {
            this.target = target;
        }
        if (data != null)
        { xbowfire_data = data; }
    }
    public override bool Validate(GameObject caster, SpellFateToken token)
    {
        if (!base.Validate(caster, token)) return false;
        return true;
    }

    protected override void OnLoopPhaseUpdate()
    {
    }
    public override void SpellEnd()
    {
    }
    public bool Validate(GameObject caster)
    {
        if (caster.TryGetComponent(out CharacterMovement_iso movement))
        {
            this.movement = movement;
            return true;
        }
        return false;
    }
}
