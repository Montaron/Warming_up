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
        { 
            xbowfire_data = data; 
        }
    }
    protected override void OnEndPhaseUpdate()   
    { 
        Debug.Log("Updating during End...");
    }

    public override IEnumerator StartSpell(GameObject caster)
    {
        if (CancelledExit()) yield break;
        OnLoopPhase_Enter();
        yield return PlayPhase(data.animationTriggerLoop, data.loopClipStateName, OnLoopPhaseUpdate, loopLoopPhase, data.loopClipSpeedMultiplier);
        OnLoopPhase_End();
        if (CancelledExit()) yield break;

        SpellProjectileFactory.Spawn(xbowfire_data, caster);
        OnEndPhase_Enter();
        yield return PlayPhase(data.animationTriggerEnd, data.endClipStateName, OnEndPhaseUpdate, loopEndPhase, data.endClipSpeedMultiplier);
        OnEndPhase_End();
    }

    public override bool Validate(GameObject caster, SpellFateToken token)
    {
        if (!base.Validate(caster, token)) return false;
        return true;
    }

    public bool Validate(GameObject caster)
    {
        return  true;
    }
}
