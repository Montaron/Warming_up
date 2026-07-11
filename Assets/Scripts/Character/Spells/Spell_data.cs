using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using Unity.VisualScripting;

public abstract class Spell_data : ScriptableObject
{
    public string spellName; 
    public isInterruptableBy interruptableBy;
    public CastableContext castContext;
    public CharacterStateType stateTransitionOnCast;

    [Header("Animation Clips")]
    public AnimationClip startClip;
    public string startClipStateName;
    public AnimationClip loopClip;
    public string loopClipStateName;
    public AnimationClip endClip;
    public string endClipStateName;

    [Header("Animation Clips Speed Multiplier")]
    public float startClipSpeedMultiplier = 1f;
    public float loopClipSpeedMultiplier = 1f;
    public float endClipSpeedMultiplier = 1f;

    [Header("Animation Triggers")]
    public string animationTriggerStart;
    public string animationTriggerLoop;
    public string animationTriggerEnd;
    public abstract ISpell CreateSpellRuntime(GameObject caster, GameObject target);
}

[Flags]
public enum CastableContext
{
    None = 0,
    Iddle = 1,
    Running = 2,
    Stunned = 4,
    Casting = 8,
}
public static class CastContextExtensions
{
    public static bool Allows(this CastableContext allowedContexts, CharacterStateType currentState)
    {
        // Map your CharacterStateType to CastContext
        CastableContext current = currentState switch
        {
            CharacterStateType.Iddle    => CastableContext.Iddle,
            CharacterStateType.Running => CastableContext.Running,
            CharacterStateType.Stunned => CastableContext.Stunned,
            CharacterStateType.Casting => CastableContext.Casting,
            _ => CastableContext.None
        };

        return allowedContexts.HasFlag(current);
    }
}
[Flags]
public enum isInterruptableBy
{
    None = 0,
    Movement = 1,
    Stun = 2,
    Recast = 4,
}