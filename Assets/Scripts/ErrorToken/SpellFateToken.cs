using System;
using UnityEngine;

public class SpellFateToken
{
    public bool IsCanceled { get; private set; }
    public bool SkipRequested { get; private set; }
    //public bool SkipPhase { get; private set; }
    public event Action<SpellCancelBy> OnSpellCanceled;
    public event Action<SpellCancelBy> OnPhaseSkipRequested;
    public SpellCancelBy spellCancelBy{ get; private set; }

    public void Cancel(SpellCancelBy cancelBy)
    {
        IsCanceled = true;
        spellCancelBy = cancelBy;
        Debug.Log("Token CANCEL reason " + cancelBy);
        OnSpellCanceled?.Invoke(cancelBy);
    }

    public void RequestSkip(SpellCancelBy cancelBy)
    {
        SkipRequested = true;
        Debug.Log("Token SKIP reason " + cancelBy);
        OnPhaseSkipRequested?.Invoke(cancelBy);
    }

    public void ResetSkip() => SkipRequested = false;
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