using UnityEngine;

public class DRComponent : BaseBuffComponent
{
    BuffType buffType => BuffType.DamageReduction;
    public float reductionAmount;
}