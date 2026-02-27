using System;

public class SpellCancelationToken
{
    public bool IsCanceled { get; private set; }
    public event Action OnSpellCanceled;

    public void Cancel()
    {
        IsCanceled = true;
        OnSpellCanceled?.Invoke();
    }
}