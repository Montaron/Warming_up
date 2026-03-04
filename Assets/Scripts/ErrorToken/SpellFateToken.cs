using System;

public class SpellFateToken
{
    public bool IsCanceled { get; private set; }
    public event Action<SpellCancelBy> OnSpellCanceled;

    public void Cancel(SpellCancelBy cancelBy)
    {
        IsCanceled = true;
        OnSpellCanceled?.Invoke(cancelBy);
    }
}

public enum SpellCancelBy
{
    None,
    ObstacleHit,
    EnemyHit,
    InputSpell,
    InputMovement,
    GameEvent
}