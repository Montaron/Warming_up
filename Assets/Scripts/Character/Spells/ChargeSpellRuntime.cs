using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ChargeSpellRuntime : ISpellPhase
{
    private char_mov_iso characterMovement;
    private ChargeSpell_data data;
    public ChargeSpellRuntime(ChargeSpell_data data)
    {
        this.data = data;
    }
    
    public void OnPhaseStart(GameObject caster)
    {
        Debug.Log("Charge Spell Started");
    }
    public IEnumerator OnPhaseUpdate(GameObject caster)
    {
        float elapsedTime = 0f;
        while (elapsedTime < data.chargeDuration)
        {
            float t = Mathf.Clamp01(elapsedTime / data.timeToReachMaxMultiplier);
            float speedMultiplier = 
                    Mathf.Lerp(data.speedMultiplierStart, data.speedMultiplierMax, t);
            characterMovement.ModifySpeed(speedMultiplier);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }
    public void OnPhaseEnd(GameObject caster)
    {
        Debug.Log("Charge Spell Ended");
    }
    public bool Validate(GameObject caster)
    {
        if (caster.TryGetComponent(out char_mov_iso movement))
        {
            characterMovement = movement;
            return true;
        }    
        return false;
    }
}
