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
        Debug.Log("CANCEL");
        IsCanceled = true;
        spellCancelBy = cancelBy;
        OnSpellCanceled?.Invoke(cancelBy);
    }

    public void RequestSkip(SpellCancelBy cancelBy)
    {
        Debug.Log("Request SKIP");
        SkipRequested = true;
        OnPhaseSkipRequested?.Invoke(cancelBy);
    }

    public void ResetSkip()
    {
        Debug.Log("Skip reset");
        SkipRequested = false;
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