using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System;

public class ChargeSpellRuntime : ISpellPhase
{
    private char_mov_iso characterMovement;
    private ChargeSpell_data data;
    private SpellCancelationToken cancellationToken;
    public ChargeSpellRuntime(ChargeSpell_data data)
    {
        this.data = data;
    }

    public void OnInit(GameObject caster, SpellCancelationToken cancelationToken)
    {
        this.cancellationToken = cancelationToken;
        characterMovement.OnHitObstacle += HandleObstacleHit;
    }
    public void OnPhaseStart(GameObject caster)
    {
        if (cancellationToken.IsCanceled)
        {
            Debug.Log("Charge Spell Start Canceled");
            return;
        }
        else
        {            
            Debug.Log("Charge Spell Start");
        }
    }
    public IEnumerator OnPhaseUpdate(GameObject caster)
    {
        float elapsedTime = 0f;
        while (elapsedTime < data.chargeDuration)
        {
            if (cancellationToken.IsCanceled)
            {
                Debug.Log("Charge Spell Canceled");
                yield break;
            }
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
        characterMovement.OnHitObstacle -= HandleObstacleHit;
        characterMovement.ResetSpeed();
        if (cancellationToken.IsCanceled)
        {
            Debug.Log("Charge Spell Ended Prematurely");
        }
        else
        {
            Debug.Log("Charge Spell Ended Normally");
        }
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

    private void HandleObstacleHit()
    {
        cancellationToken.Cancel();
    }
}
