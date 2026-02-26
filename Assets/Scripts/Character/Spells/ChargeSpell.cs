using UnityEngine;
[CreateAssetMenu(fileName = "ChargeSpell", menuName = "New Charge Spell")]
public class ChargeSpell : SpellEffect
{
    public float speedMultiplierStart = 2f;
    public float speedMultiplierMax = 5f;
    public float timeToReachMaxMultiplier = 10f;
    private float currentMultiplier = 0f;
    private char_mov_iso charMov;
    private bool hasHitObstacle = false;
    private float chargeCompletion = 0f;

    public override bool Validate(GameObject caster)
    {
        if (!caster.TryGetComponent<char_mov_iso>(out charMov))
        {
            Debug.LogError($"{name} : ChargeSpell missing on {caster.name}.");
            return  false;
        }
        return true;
    }
    public override void OnPhaseStart(GameObject caster)
    {
        Debug.Log("Charge started!");
    }


    public override void OnPhaseUpdate(GameObject caster, float timeElapsed)
    {
        if (!hasHitObstacle)
        {
            chargeCompletion = timeElapsed / timeToReachMaxMultiplier;
            if (chargeCompletion > 1f)
                chargeCompletion = 1f;
            currentMultiplier = Mathf.Lerp(speedMultiplierStart, speedMultiplierMax, chargeCompletion);
            charMov.ModifySpeed(currentMultiplier);
        } 
    }

    public override void OnPhaseEnd(GameObject caster)
    {
        charMov.ResetSpeed();
        Debug.Log("Charge ended!");
    }
}