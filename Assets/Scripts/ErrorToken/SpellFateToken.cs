using System;
using UnityEngine;

public class SpellFateToken
{
    public bool IsCanceled { get; private set; }
    //public bool SkipPhase { get; private set; }
    public event Action<SpellCancelBy> OnSpellCanceled;
    public SpellCancelBy spellCancelBy{ get; private set; }

    public void Cancel(SpellCancelBy cancelBy)
    {
        IsCanceled = true;
        spellCancelBy = cancelBy;
        Debug.Log("Token cancellation reason " + cancelBy);
        OnSpellCanceled?.Invoke(cancelBy);
    }
}

public enum SpellCancelBy
{
    None,
    ObstacleHit,
    EnemyHit,
    keyDown,
    MovementKey,
    GameEvent,
    KeyUp,
    Stun,
}