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
        Debug.Log("Updating during EndPhase...");
    }

    public override IEnumerator StartSpell(GameObject caster)
    {
        RaiseOnSpellPhaseReached(SpellPhaseTrigger.OnPhaseLoop_Enter);
        // Loop phase — play and wait for clip to finish
        yield return PlayPhase(data.animationTriggerLoop, data.loopClipStateName, OnLoopPhaseUpdate, loopLoopPhase, data.loopClipSpeedMultiplier);

        RaiseOnSpellPhaseReached(SpellPhaseTrigger.OnPhaseLoop_End);
        if (token.IsCanceled && token.spellCancelBy == SpellCancelBy.MovementKey)
        {
            RaiseOnSpellPhaseReached(SpellPhaseTrigger.OnPhaseExit_Enter);
            animator.SetTrigger("Exit_Loop");
            yield break;
        }
        // End phase — play and wait for clip to finish
        RaiseOnSpellPhaseReached(SpellPhaseTrigger.OnPhaseEnd_Enter);
        SpellProjectileFactory.Spawn(xbowfire_data, caster);
        yield return PlayPhase(data.animationTriggerEnd, data.endClipStateName, OnEndPhaseUpdate, loopEndPhase, data.endClipSpeedMultiplier);
        RaiseOnSpellPhaseReached(SpellPhaseTrigger.OnPhaseEnd_End);
        OnEndPhaseEnd();
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
        return  true;
    }
}
