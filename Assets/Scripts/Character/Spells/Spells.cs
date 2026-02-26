using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "Spell", menuName = "New Spellos")]
public class Spells : ScriptableObject
{
    public List<SpellPhase> phases;

    public IEnumerator Cast(GameObject caster)
    {
        Debug.Log("casting spell");
        foreach (var phase in phases)
        {
            bool phaseValidation = false;
            Debug.Log($"Starting phase: {phase.name}");
            //yield return phase.ExecutePhase(caster, result => phaseValidation = result);
            if (!phaseValidation)
            {
                Debug.LogError($"{name} : Phase {phase.name} failed validation. Casting aborted.");
                yield break;
            }
        }
    }
}