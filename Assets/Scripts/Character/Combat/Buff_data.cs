using UnityEngine;

public abstract class Buff_data : ScriptableObject
{
    public string buffName;
    public float duration;
    public BuffEffect buffEffect;
    public BuffType BuffType => buffEffect switch
    {
        BuffEffect.HoT => BuffType.Buff,
        BuffEffect.DamageReduction => BuffType.Buff,

        BuffEffect.DoT => BuffType.Debuff,
        _ => throw new System.ArgumentOutOfRangeException()
    };
}

public enum BuffType
{
    Buff,
    Debuff,
}

public enum BuffEffect
{
    HoT,
    DoT,
    DamageReduction,
} 