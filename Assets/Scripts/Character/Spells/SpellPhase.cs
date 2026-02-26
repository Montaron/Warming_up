using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "SpellPhase", menuName = "New Spell Phase")]
public class SpellPhase : ScriptableObject
{
    public float duration = 10f;
    public List<SpellEffect> effects;

    public IEnumerator ExecuteSpell(GameObject caster, System.Action<bool> result)
    {
        foreach (var effect in effects)
        {
            if (!effect.Validate(caster))
            {
                Debug.LogError($"{name} : Validation failed for {effect.name} on {caster.name}.");
                result(false);
                yield break;
            }
            result(true);
        }
        foreach (var effect in effects)
        {
            effect.OnPhaseStart(caster);
        }

        float timer = 0f;
        while (timer < duration)
        {
            foreach (var effect in effects)
            {
                effect.OnPhaseUpdate(caster, timer);
            }
            timer += Time.deltaTime;
            yield return null;
        }

        foreach (var effect in effects)
        {
            effect.OnPhaseEnd(caster);
        }
    }
}
