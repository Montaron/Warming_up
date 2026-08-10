using UnityEngine;
using System.Collections;
using System;

public class ChargeSpellRuntime : BaseSpellRuntime
{
    public ChargeSpell_data charge_data { get; private set; }
    protected override bool loopLoopPhase => true;
    private GameObject target;
    private float currentSpeedMultiplier = 5f;

    public ChargeSpellRuntime(GameObject caster, ChargeSpell_data data, GameObject target)
            : base(caster, data)
    {
        if (target != null)
        {
            this.target = target;
        }
        if (data != null)
        { charge_data = data; }
    }

    public override IEnumerator StartSpell(GameObject caster)
    {
        if (CancelledExit()) yield break;
        OnStartPhase_Enter();
        yield return PlayPhase(data.animationTriggerStart, data.startClipStateName, OnStartPhase_Update, loopStartPhase, data.startClipSpeedMultiplier);
        OnStartPhase_End();
        if (CancelledExit()) yield break;

        OnLoopPhaseEnter();
        yield return PlayPhase(data.animationTriggerLoop, data.loopClipStateName, OnLoopPhase_Update, loopLoopPhase, data.loopClipSpeedMultiplier);
        OnLoopPhase_End();
        if (CancelledExit()) yield break;

        OnEndPhase_Enter();
        yield return PlayPhase(data.animationTriggerEnd, data.endClipStateName, OnEndPhase_Update, loopEndPhase, data.endClipSpeedMultiplier);
        OnEndPhase_End();
        if (CancelledExit()) yield break;
    }
    
    public override bool Validate(GameObject caster, SpellFateToken token)
    {
        if (!base.Validate(caster, token)) return false;
        base.movement.OnHitObstacle += HandleObstacleHit;
        return true;
    }

    protected override void OnStartPhase_Update()
    {
        base.OnStartPhase_Update;
        Debug.Log("Speed modifier by" + charge_data.speedIni);
        movement.ModifySpeed(charge_data.speedIni);
    }
    protected override void OnLoopPhase_Update()
    {
        base.OnLoopPhase_Update;
        currentSpeedMultiplier = Mathf.MoveTowards(
                    currentSpeedMultiplier,
                    charge_data.speedMultiplierMax,
                    charge_data.speedMultiplierMax / charge_data.timeToReachMaxMultiplier
                        * Time.deltaTime);

        movement.ModifySpeed(currentSpeedMultiplier);
        movement.MoveCharacterForward();
    }
    public override void SpellEnd()
    {
        movement.OnHitObstacle -= HandleObstacleHit;
        movement.ResetSpeed();
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

    private void HandleObstacleHit(Collider collider)
    {
        if (ComponentUtils.TryGetDamageable(collider, out IDamageable damageable))
        {
            var damageData = new DamageData
            {
                damage = charge_data.damage,
                attacker = caster,
                target = collider.gameObject,
            };
            damageable.TakeDamage(damageData);
            // Debug.Log($"{damageData.target.name} took {damageData.damage} from {damageData.attacker.name ?? "unknown"}");
        }
        token.Cancel(SpellCancelBy.ObstacleHit);
    }
}