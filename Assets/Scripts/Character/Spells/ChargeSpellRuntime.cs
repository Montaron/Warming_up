using UnityEngine;
using System.Collections;
using System;

public class ChargeSpellRuntime : ISpellPhase
{
    private CharacterMovement_iso characterMovement;
    public ChargeSpell_data data { get; private set; }
    private SpellFateToken spellFateToken;
    private CharacterCombat characterCombat;

    public ChargeSpellRuntime(ChargeSpell_data data)
    {
        this.data = data;
    }

    public void OnInit(GameObject caster, SpellFateToken cancelationToken)
    {
        this.spellFateToken = cancelationToken;
        characterMovement.OnHitObstacle += HandleObstacleHit;
    }
    public void OnPhaseStart(GameObject caster)
    {
        if (spellFateToken.IsCanceled)
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
            characterMovement.LockDirection = true;
            characterMovement.ForceForward = true;
            if (spellFateToken.IsCanceled)
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
        characterMovement.LockDirection = false;
        characterMovement.ForceForward = false;
        if (spellFateToken.IsCanceled)
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
        if (caster.TryGetComponent(out CharacterMovement_iso movement))
        {
            characterMovement = movement;
            return true;
        }    
        return false;
    }

    private void HandleObstacleHit()
    {
        spellFateToken.Cancel(SpellCancelBy.ObstacleHit);
    }
}
