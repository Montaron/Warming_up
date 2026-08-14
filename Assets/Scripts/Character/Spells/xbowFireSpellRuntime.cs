using UnityEngine;
using System.Collections;
using System;

public class xbowFireSpellRuntime : BaseSpellRuntime
{
    public xbowFire_data data { get; private set; }
    protected override bool loopLoopPhase => true;
    private GameObject target;
    private float damage;
    private float elapsedTime;

    public xbowFireSpellRuntime(GameObject caster, xbowFire_data data, GameObject target)
            : base(caster, data)
    {
        if (target != null)
        {
            this.target = target;
        }
        if (data != null)
        {
            this.data = data;
        }
    }
    protected override void OnLoopPhase_Enter()
    {
        base.OnLoopPhase_Enter();
        elapsedTime = 0f;
    }
    protected override void OnLoopPhase_Update()
    {
        elapsedTime += Time.deltaTime;
    }
    protected override void OnLoopPhase_End()
    {
        base.OnLoopPhase_Enter();
        if (data.timeRange <= 0f || data.RampUpDelay >= elapsedTime)
        {
            damage = data.damage;
            return;
        }
        damage = Mathf.CeilToInt(Mathf.Lerp(data.damage, data.maxDamage, Mathf.Clamp01(elapsedTime / data.timeRange)));
    }
     
    public override IEnumerator StartSpell(GameObject caster)
    {
        if (CancelledExit()) yield break;
        OnLoopPhase_Enter();
        yield return PlayPhase(data.animationTriggerLoop, data.loopClipStateName, OnLoopPhase_Update, loopLoopPhase, data.loopClipSpeedMultiplier);
        OnLoopPhase_End();
        if (CancelledExit()) yield break;
        SpellProjectileFactory.Spawn(data, caster, damage);
        OnEndPhase_Enter();
        yield return PlayPhase(data.animationTriggerEnd, data.endClipStateName, OnEndPhase_Update, loopEndPhase, data.endClipSpeedMultiplier);
        OnEndPhase_End();
    }

    public override bool Validate(GameObject caster, SpellFateToken token)
    {
        if (!base.Validate(caster, token)) return false;
        return true;
    }

    public bool Validate(GameObject caster)
    {
        return true;
    }
}