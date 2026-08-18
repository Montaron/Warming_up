using JetBrains.Annotations;
using System.Collections;
using UnityEngine;
public class DashSpellRuntime : BaseSpellRuntime
{
    public Spell_data spell_data { get; private set; }
    private float dashSpeed;
    private Vector3 dashDirection;

    public DashSpellRuntime(GameObject caster, DashSpell_data data)
            : base(caster, data)
    {
        if (data != null)
        {
            spell_data = data;
            dashSpeed = data.DashSpeed;
        }
    }
    protected override void OnStartPhase_Enter()
    {
        //dash to the mouse
        dashDirection = movement.GetMouseDirection();
        movement.ModifySpeed(dashSpeed);
        base.OnStartPhase_Enter();
    }
    protected override void OnStartPhase_Update()
    {
        movement.MoveCharacterTo(dashDirection);
    }
    public override IEnumerator StartSpell(GameObject caster)
    {
        if (CancelledExit()) yield break;
        OnStartPhase_Enter();
        yield return PlayPhase(data.animationTriggerStart, data.startClipStateName, OnStartPhase_Update, loopStartPhase, data.startClipSpeedMultiplier);
        OnStartPhase_End();
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
                attacker = caster,
                target = collider.gameObject,
            };
            token.RequestSkip(SpellCancelBy.ObstacleHit);
        }
    }
}