using System;
using System.Collections;
using UnityEngine;
public abstract class BaseSpellRuntime : ISpell
{
    protected GameObject caster;
    protected Spell_data data;
    protected Animator animator;
    protected CharacterMovement_iso movement;
    protected SpellFateToken token;
    protected virtual bool loopStartPhase => false;
    protected virtual bool loopLoopPhase  => false;
    protected virtual bool loopEndPhase   => false;
    public event Action<SpellPhaseTrigger> OnSpellPhaseReached;
    protected void RaiseOnSpellPhaseReached(SpellPhaseTrigger phaseTrigger) => OnSpellPhaseReached?.Invoke(phaseTrigger);
    protected BaseSpellRuntime(GameObject caster, Spell_data data)
    {
        this.caster   = caster;
        this.data     = data;
        this.animator = caster.GetComponentInChildren<Animator>();
        if (animator == null)
        {
            animator = caster.GetComponent<Animator>();
        }
        movement = caster.GetComponent<CharacterMovement_iso>();
    }

    public virtual IEnumerator StartSpell(GameObject caster)
    {
        yield return PlayPhase(data.animationTriggerStart, data.startClipStateName, OnStartPhase_Update, loopStartPhase, data.startClipSpeedMultiplier);
        
        yield return PlayPhase(data.animationTriggerLoop, data.loopClipStateName, OnLoopPhase_Update, loopLoopPhase, data.loopClipSpeedMultiplier);

        yield return PlayPhase(data.animationTriggerEnd, data.endClipStateName, OnEndPhase_Update, loopEndPhase, data.endClipSpeedMultiplier);
    }
    //Start methods
    protected virtual void OnStartPhase_Enter()
    {
        RaiseOnSpellPhaseReached(SpellPhaseTrigger.OnStartPhase_Enter);
    }
    protected virtual void OnStartPhase_Update(){ }
    protected virtual void OnStartPhase_End()   
    { 
        RaiseOnSpellPhaseReached(SpellPhaseTrigger.OnStartPhase_End);
    }
    //Loop methods
    protected virtual void OnLoopPhase_Enter()  
    {
        RaiseOnSpellPhaseReached(SpellPhaseTrigger.OnLoopPhase_Enter);
    }
    protected virtual void OnLoopPhase_Update() { }
    protected virtual void OnLoopPhase_End()    
    { 
        RaiseOnSpellPhaseReached(SpellPhaseTrigger.OnLoopPhase_End);
    }

    protected virtual void OnEndPhase_Enter()   
    {
        RaiseOnSpellPhaseReached(SpellPhaseTrigger.OnEndPhase_Enter);
    }
    protected virtual void OnEndPhase_Update()  { }
    protected virtual void OnEndPhase_End()     
    {
        RaiseOnSpellPhaseReached(SpellPhaseTrigger.OnEndPhase_End);
    }
    //Exit methods
    protected virtual void OnPhaseExit()
    {
        RaiseOnSpellPhaseReached(SpellPhaseTrigger.OnPhaseExit);
    }

    protected bool CancelledExit()
    {
        if (!token.IsCanceled) return false;
        OnPhaseExit();
        animator.SetTrigger("Cancel_Exit");
        return true;
    }

    protected IEnumerator PlayPhase(string trigger, string animationStateName, Action OnUpdate, bool isLooping = false, float animSpeedMultiplier = 1f)
    {
        Debug.Log(animationStateName);
        if (string.IsNullOrEmpty(trigger) || string.IsNullOrEmpty(animationStateName)) yield break;
        animator.Play(animationStateName, 0, 0f);
        animator.speed = animSpeedMultiplier; 
        yield return null;
        try 
        {
            if (isLooping)
            {
                while (!token.IsCanceled && !token.SkipRequested)
                {
                    OnUpdate?.Invoke();
                    yield return null;
                }
            }
            else
            {
                while (animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1f
               && !token.IsCanceled && !token.SkipRequested
               && animator.GetCurrentAnimatorStateInfo(0).IsName(animationStateName))
                {
                    OnUpdate?.Invoke();
                    yield return null;
                }
            }
        }
        finally
        {
            animator.speed = 1f;
        }
        token.ResetSkip();
    }

    public virtual bool Validate(GameObject caster, SpellFateToken token)
    {
        if (caster == null)
            return false;

        if (animator == null)
            return false;

        if (data == null)
            return false;
        if (token != null)
            this.token = token;
        else
            return false;

        return true;
    }

    public virtual void SpellEnd(){ }
}
// Spawner — call this from wherever the spell logic triggers the projectile release
public static class SpellProjectileFactory
{
    public static SpellProjectile Spawn(ProjectileSpell_data data, GameObject caster, float raw_damage)
    {
        if (data.projectilePrefab == null)
        {
            Debug.LogError($"Spell '{data.spellName}' has no projectile prefab assigned.");
            return null;
        }

        Collider target = caster.GetComponent<CharacterCombat>().currentTarget;
        Vector3 direction;
        if (target != null)
        {
            direction = (target.transform.position - caster.transform.position).normalized;
            direction.y = 0f;
        }
        else
            direction = caster.transform.forward;
        Vector3 spawnPosition = caster.transform.position + direction * data.projectileSpawnOffset_X + Vector3.up * data.projectileSpawnOffset_Z;
        Quaternion spawnRotation = Quaternion.LookRotation(direction) * Quaternion.Euler(new Vector3(0, 90, 0));

        GameObject instance = UnityEngine.Object.Instantiate(data.projectilePrefab, spawnPosition, spawnRotation);
        SpellProjectile projectile = instance.GetComponent<SpellProjectile>();

        if (projectile == null)
        {
            Debug.LogError($"Projectile prefab for '{data.spellName}' is missing a SpellProjectile component.");
            UnityEngine.Object.Destroy(instance);
            return null;
        }

        projectile.Initialize(direction, data, caster, raw_damage);
        return projectile;
    }
}