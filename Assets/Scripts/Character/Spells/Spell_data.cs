using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public abstract class Spell_data : ScriptableObject
{
    public string spellName; 
    public isInterruptableBy interruptableBy;
    public CastableContext castContext;
    public abstract ISpellPhase CreateSpellRuntime();
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
[Flags]
public enum isInterruptableBy
{
    None = 0,
    Movement = 1,
    Stun = 2,
    Recast = 4,
}