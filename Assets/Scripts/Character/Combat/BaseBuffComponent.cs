using UnityEngine;

public abstract class BaseBuffComponent : ScriptableObject
{
    BuffType buffType
    public bool isPermanent;
    public float buffDuration;
}

public enum BuffType
{
    HoT,
    DoT,
    DamageReduction,
}