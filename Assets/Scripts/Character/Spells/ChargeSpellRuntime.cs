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

    public override bool Validate(GameObject caster, SpellFateToken token)
    {
        if (!base.Validate(caster, token)) return false;
        base.movement.OnHitObstacle += HandleObstacleHit;
        return true;
    }
    protected override void OnLoopPhaseUpdate()
    {
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
            Debug.Log($"{damageData.target.name} took {damageData.damage} from {damageData.attacker.name ?? "unknown"}");
        }
        token.Cancel(SpellCancelBy.ObstacleHit);
    }
}
