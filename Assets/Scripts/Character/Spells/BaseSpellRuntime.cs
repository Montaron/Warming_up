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

    public IEnumerator StartSpell(GameObject caster)
    {
        // Start phase — play and wait for clip to finish
        yield return PlayPhase(data.animationTriggerStart, data.startClipStateName, OnStartPhaseUpdate, loopStartPhase, data.startClipSpeedMultiplier);
        //if (token.IsCanceled) yield break;

        OnStartPhaseEnd();

        // Loop phase — play and wait for clip to finish
        yield return PlayPhase(data.animationTriggerLoop, data.loopClipStateName, OnLoopPhaseUpdate, loopLoopPhase, data.loopClipSpeedMultiplier);
        //if (token.IsCanceled) yield break;

        OnLoopPhaseEnd();

        // End phase — play and wait for clip to finish
        yield return PlayPhase(data.animationTriggerEnd, data.endClipStateName, OnEndPhaseUpdate, loopEndPhase, data.endClipSpeedMultiplier);

        OnEndPhaseEnd();
    }
    protected virtual void OnStartPhaseStart()  {}
    protected virtual void OnLoopPhaseStart()  {}
    protected virtual void OnEndPhaseStart()  {}
    protected virtual void OnStartPhaseUpdate() { }
    protected virtual void OnLoopPhaseUpdate()  { }
    protected virtual void OnEndPhaseUpdate()   { }
    // Override in concrete spells to hook into phase transitions
    protected virtual void OnStartPhaseEnd() { }
    protected virtual void OnLoopPhaseEnd()  { }
    protected virtual void OnEndPhaseEnd()   { }

    // ─────────────────────────────────────────
    // Wait for animation clip to finish
    // ─────────────────────────────────────────

    private IEnumerator PlayPhase(string trigger, string animationStateName, Action OnUpdate, bool isLooping = false, float animSpeedMultiplier = 1f)
    {
        if (string.IsNullOrEmpty(trigger) || string.IsNullOrEmpty(animationStateName)) yield break;

        animator.Play(animationStateName, 0, 0f);
        animator.speed = animSpeedMultiplier; 
        // Debug.Log($"Playing trigger {trigger}");
        // Wait one frame for Animator to transition
        yield return null;
        // Wait for transition to fully complete before reading state
        //yield return new WaitUntil(() => !animator.IsInTransition(0));
        Debug.Log("Transition done, entering try block" + animator.GetCurrentAnimatorStateInfo(0).normalizedTime);
        try 
        {
            if (isLooping)
            {
                while (!token.IsCanceled)
                {
                    OnUpdate?.Invoke();
                    yield return null;
                }
            }
            else
            {
                while (animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1f
               && !token.IsCanceled
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
        // Wait until the clip finishes playing
    }

    public virtual bool Validate(GameObject caster, SpellFateToken token)
    {
        if (caster == null)
        {
            // Debug.LogWarning("Validate failed: caster is null");
            return false;
        }

        if (animator == null)
        {
            // Debug.LogWarning($"Validate failed: no Animator found on {caster.name}");
            return false;
        }

        if (data == null)
        {
            // Debug.LogWarning("Validate failed: spell data is null");
            return false;
        }
        if (token != null)
        {
            this.token = token;
        }
        else
        {
            // Debug.LogWarning("Validate failed: Token not found");
            return false;
        }

        return true;
    }

    public virtual void SpellEnd()
    {

    }

}