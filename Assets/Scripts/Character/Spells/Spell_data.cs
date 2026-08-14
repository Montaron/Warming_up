using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using Unity.VisualScripting;

public abstract class Spell_data : ScriptableObject
{
    [Header("General information")]
    public string spellName; 
    public CastableContext castContext;
    public CharacterStateType stateTransitionOnCast;
    public SpellType spellType;
    public WeaponType weaponType;

    [Header("Interruption")]
    //Interruption : 
    // -Spell can be interrupted by actions (InterruptFlag) during either the current phase (SpellPhase) or interrupt all the spell sequence (the choice is found in SpellInterruptionType)
    // -Some Phases can have window where they cant be interrupted (SpellUnInterruptablePhaseWindow_data)
    public List<SpellInterruption_data> IsInterruptableBy;
    public List<SpellUnInterruptablePhaseWindow_data> hasUnInterruptableWindow;

    [Header("Animation Clips")]
    public string startClipStateName;
    public string loopClipStateName;
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
    Channeling = 8,
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
            CharacterStateType.Channeling => CastableContext.Channeling,
            _ => CastableContext.None
        };

        return allowedContexts.HasFlag(current);
    }
}

public enum WeaponType
{
    None,
    Crossbow,
    Two_Hander,
    Shield,
}

[Flags]
public enum InterruptFlag
{
    None = 0,
    Movement = 1,
    Stun = 2,
    KeyDown = 4,
    KeyUp = 8,
    EnnemyHit = 16,
}

public enum SpellInterruptionType
{
    SkipPhase,
    CancelSpell
}

[Serializable]
public struct SpellInterruption_data
{
    public SpellPhase Phase;
    public InterruptFlag Interrupt;
    public SpellInterruptionType Type;
}

[Serializable]
public struct SpellUnInterruptablePhaseWindow_data 
{
    public SpellPhase Phase;
    public InterruptFlag Interrupt;
    public float time_amount;
}
public enum SpellType
{
    Instant,
    Charged,
    Channeled
}

public enum SpellPhaseTrigger
{
    OnPhaseExit,
    OnStartPhase_Enter,
    OnStartPhase_End,
    OnLoopPhase_Enter,
    OnLoopPhase_End,
    OnEndPhase_Enter,
    OnEndPhase_End
}
[Flags]
public enum SpellPhase
{
    None = 0,
    Start = 1,
    Loop = 2,
    End = 4
}