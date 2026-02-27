using System.Collections;
using UnityEngine;
public class CharacterCombat : MonoBehaviour
{
    [SerializeField] private ChargeSpell_data chargeSpellData;
    private ISpellPhase chargeSpell;
    private SpellCancelationToken cancelationToken;
    void Start()
    {
        if (chargeSpellData != null)
        {
            chargeSpell = chargeSpellData.CreateSpellRuntime();
            if (chargeSpell.Validate(gameObject))
            {
                Debug.Log("Charge spell validated successfully for " + gameObject.name);
            }
            else
            {
                Debug.LogError("Charge spell validation failed for " + gameObject.name);
            }
        }
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && chargeSpell != null)
        {
            StartCoroutine(CastSpell(chargeSpell));
        }
    }
    private IEnumerator CastSpell(ISpellPhase spell)
    {
        cancelationToken = new SpellCancelationToken();
        spell.OnInit(gameObject, cancelationToken);
        spell.OnPhaseStart(gameObject);
        yield return StartCoroutine(spell.OnPhaseUpdate(gameObject));
        spell.OnPhaseEnd(gameObject);
    }

}