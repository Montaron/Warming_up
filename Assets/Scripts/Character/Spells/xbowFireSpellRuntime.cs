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
    public override IEnumerator StartSpell(GameObject caster)
    {
        // Loop phase — play and wait for clip to finish
        yield return PlayPhase(data.animationTriggerLoop, data.loopClipStateName, OnLoopPhaseUpdate, loopLoopPhase, data.loopClipSpeedMultiplier);

        OnLoopPhaseEnd();
        if (token.IsCanceled && token.spellCancelBy == SpellCancelBy.MovementKey)
        {
            animator.SetTrigger("Exit_Loop");
            yield break;
        }
        // End phase — play and wait for clip to finish
        yield return PlayPhase(data.animationTriggerEnd, data.endClipStateName, OnEndPhaseUpdate, loopEndPhase, data.endClipSpeedMultiplier);

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
