using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class Spell_data : ScriptableObject
{
    public string spellName;
    public abstract ISpellPhase CreateSpellRuntime();
}