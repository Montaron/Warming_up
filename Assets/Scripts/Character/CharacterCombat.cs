using System;
using System.Collections;
using UnityEngine;
public class CharacterCombat : MonoBehaviour
{
    //[SerializeField] private ChargeSpell_data ChargeSpell_data; 
    private ISpellPhase currentSpell;
    private string currentSpellName;
    private SpellFateToken spellFateToken;
    private bool spellRunning;
    void Start()
    {

    }
    void Update()
    {
    }


    private void ResetSpell()
    {
        currentSpell = null;
        currentSpellName = null;
        spellFateToken.OnSpellCanceled -= spellFateToken_OnSpellCanceled;
        spellFateToken = null;
        spellRunning = false;
    }

    private ISpellPhase InitSpell(Spell_data spellData)
    {
        spellRunning = true;
        spellFateToken = new SpellFateToken();
        currentSpellName = spellData.spellName;
        spellFateToken.OnSpellCanceled += spellFateToken_OnSpellCanceled;
        return spellData.CreateSpellRuntime();
    }

    private void spellFateToken_OnSpellCanceled(SpellCancelBy by)
    {
        Debug.Log($"Spell {currentSpellName} canceled by {by}");
    }

    public void CastSpellRequest(Spell_data spellData)
    {
        if (spellData == null)
        {
            return;
        }
        if (spellRunning)
        {
            if (currentSpellName == spellData.spellName && spellData.interruptableBy == isInterruptableBy.Recast)
            {
                CancelCurrentSpell(SpellCancelBy.InputSpell);
                return;
            }
        }
        else
        {
            StartCoroutine(CastSpell(spellData));
        }
    }
    private IEnumerator CastSpell(Spell_data spellData)
    {
        currentSpell = InitSpell(spellData);
        currentSpell.Validate(gameObject);
        currentSpell.OnInit(gameObject, spellFateToken);
        currentSpell.OnPhaseStart(gameObject);
        yield return StartCoroutine(currentSpell.OnPhaseUpdate(gameObject));
        currentSpell.OnPhaseEnd(gameObject);
        ResetSpell();
    }

    private void CancelCurrentSpell(SpellCancelBy spellCancelBy)
    {
        if (currentSpell != null && !spellFateToken.IsCanceled)
            spellFateToken.Cancel(spellCancelBy);
    }
}