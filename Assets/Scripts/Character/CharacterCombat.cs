using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEditor.SearchService;
using UnityEngine;
using System.Linq;

public class CharacterCombat : MonoBehaviour
{
    [Header("Targeting")]
    [SerializeField] private float rayDistance = 10f;
    [SerializeField] private float rayOriginHeight = 1f;
    [SerializeField] private LayerMask targetLayer;
    [SerializeField] private Color defaultHighlightColor = Color.yellow;
    [SerializeField] private Color selectedColor = Color.red;
    [SerializeField] private Vector3 boxHalfExtents = new Vector3(0.5f, 0.8f, 0f);

    //Events
    public event Action<Spell_data> OnSpellEnded;
    public event Action<CharacterStateType> OnCombatStateChange;

    public Collider currentTarget { get; private set; }
    private List<Collider> detectedTargets = new();

    //Others
    public string currentSpellName { get; private set; }
    public Spell_data currentSpellData { get; private set; }
    public bool spellRunning { get; private set; }
    public float spellElapsedTime { get; private set; }
    public SpellPhase currentPhase;

    private ISpell currentSpell;
    private SpellFateToken spellFateToken;

    void Start()
    {

    }
    void Update()
    {
        Debug.Log("Current Phase = " + currentPhase);
        if (spellRunning)
        {
            spellElapsedTime += Time.deltaTime;
        }
        ScanForTargets();
    }
    private void ScanForTargets()
    {
        detectedTargets.Clear();

        Vector3 rayOrigin = transform.position + Vector3.up * rayOriginHeight;
        Quaternion rotation = transform.rotation;

        RaycastHit[] hits = Physics.BoxCastAll(
            center: rayOrigin,
            halfExtents: boxHalfExtents,
            direction: transform.forward,
            orientation: rotation,
            maxDistance: rayDistance,
            layerMask: targetLayer
        );

        Collider closestCollider = null;
        float closestDistance = float.MaxValue;

        foreach (RaycastHit hit in hits)
        {
            if (!detectedTargets.Contains(hit.collider))
                detectedTargets.Add(hit.collider);

            float distance = hit.distance;
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestCollider = hit.collider;
            }
        }

        // Auto select closest if no target yet
        if (currentTarget == null && closestCollider != null)
            SelectTarget(closestCollider);

        // Clear if current target left the detection box
        if (currentTarget != null && !detectedTargets.Contains(currentTarget))
            ClearTarget();
    }

    // ─────────────────────────────────────────
    // Target Selection + Color
    // ─────────────────────────────────────────

    private void SelectTarget(Collider target)
    {
        // Reset previous
        if (currentTarget != null)
            SetColliderColor(currentTarget, Color.white);

        currentTarget = target;
        SetColliderColor(currentTarget, selectedColor);
    }

    private void ClearTarget()
    {
        if (currentTarget != null)
            SetColliderColor(currentTarget, Color.white);

        currentTarget = null;
    }

    private void SetColliderColor(Collider col, Color color)
    {
        if (col.TryGetComponent<Renderer>(out var renderer))
            renderer.material.color = color;
    }
    private void OnDrawGizmosSelected()
    {
        Vector3 origin = transform.position + Vector3.up * rayOriginHeight;
        Vector3 center = origin + transform.forward * (rayDistance / 2f);
        Vector3 size = new Vector3(boxHalfExtents.x * 2,
                                          boxHalfExtents.y * 2,
                                          rayDistance);

        Gizmos.color = Color.cyan;
        Gizmos.matrix = Matrix4x4.TRS(center, transform.rotation, Vector3.one);
        Gizmos.DrawWireCube(Vector3.zero, size);
        Gizmos.matrix = Matrix4x4.identity; // reset after
    }
    //Spell Handling
    private void ResetSpell()
    {
        ((BaseSpellRuntime) currentSpell).OnSpellPhaseReached -= HandleSpellPhaseChange;
        spellElapsedTime = 0f;
        currentSpell = null;
        currentSpellName = null;
        currentPhase = SpellPhase.None;
        spellFateToken.OnSpellCanceled -= spellFateToken_OnSpellCanceled;
        spellFateToken = null;
        spellRunning = false;
    }

    private ISpell InitSpell(Spell_data spellData)
    {
        spellFateToken = new SpellFateToken();
        currentSpellName = spellData.spellName;
        currentSpellData = spellData;
        spellFateToken.OnSpellCanceled += spellFateToken_OnSpellCanceled;
        BaseSpellRuntime spell = (BaseSpellRuntime) spellData.CreateSpellRuntime(gameObject, null);
        spell.OnSpellPhaseReached += HandleSpellPhaseChange;
        currentPhase = SpellPhase.None;
        return spell;
    }
    private void RequestStateChange(SpellPhase currentPhase, SpellType spellType)
    {
        switch (spellType)
        {
            case SpellType.Channeled:
                if (currentPhase == SpellPhase.Loop)
                    OnCombatStateChange?.Invoke(CharacterStateType.Channeling);
                break;
            case SpellType.Charged: 
                OnCombatStateChange?.Invoke(CharacterStateType.Attacking);
                break;
        }
    }

    private void HandleSpellPhaseChange(SpellPhaseTrigger phaseTrigger)
    {
    switch (phaseTrigger)
    {
        case SpellPhaseTrigger.OnStartPhase_Enter:
            currentPhase = SpellPhase.Start;
            break;
        case SpellPhaseTrigger.OnLoopPhase_Enter:
            currentPhase = SpellPhase.Loop;
            break;
        case SpellPhaseTrigger.OnEndPhase_Enter:
            currentPhase = SpellPhase.End;
            break;
        case SpellPhaseTrigger.OnPhaseExit:
            currentPhase = SpellPhase.None;
            break;
    }
        RequestStateChange(currentPhase, currentSpellData.spellType);
    }

    private void spellFateToken_OnSpellCanceled(SpellCancelBy by)
    {

    }

    public bool CastSpellRequest(Spell_data spellData)
    {
        if (spellData == null)
        {
            return false;
        }
        if (spellRunning)
        {
            return false;
        }
        else
        {
            StartCoroutine(CastSpell(spellData));
            return true;
        }
    }
    private IEnumerator CastSpell(Spell_data spellData)
    {
        currentSpell = InitSpell(spellData);
        currentSpell.Validate(gameObject, spellFateToken);
        spellRunning = true;
        yield return StartCoroutine(currentSpell.StartSpell(gameObject));
        currentSpell.SpellEnd();
        OnSpellEnded(spellData);
        ResetSpell();
    }
    public bool Test_TryInterruptSpell(InterruptFlag interrupt_reason, string incomingSpellName = null)
    {
        if (!spellRunning)
            return false;

        bool sameSpellCheck = string.IsNullOrEmpty(incomingSpellName)
            || currentSpellName == incomingSpellName;

        if (!sameSpellCheck || !currentSpellData.isInterruptableBy.HasFlag(interrupt_reason))
            return false;
        if (currentSpellData.UnInterruptablePhase.HasFlag(currentPhase))
            return false;
        if (IsDelayedUninterruptible(interrupt_reason))
            return false;

        CancelCurrentSpell(interrupt_reason, SpellInterruptionType.CancelSpell);
        return true;
    }


    public bool Test_TryInterruptSpell2(InterruptFlag interrupt_reason = 0, string incomingSpellName = null)
    {
        if (!spellRunning)
            return false;
        if (!string.IsNullOrEmpty(incomingSpellName) && incomingSpellName == currentSpellName)
            interrupt_reason = InterruptFlag.KeyDown;

        if (!TryGetInterruption(interrupt_reason, currentPhase, out var interrupt_data))
            return false;
        if (TryGetImmunity(interrupt_data.Interrupt, interrupt_data.Phase, out var interruptWindow)
                            && interruptWindow > spellElapsedTime)
            return false;
        if (interrupt_data.Type == SpellInterruptionType.SkipPhase)
            CancelCurrentSpell(interrupt_reason, SpellInterruptionType.SkipPhase);
        else
            CancelCurrentSpell(interrupt_reason, SpellInterruptionType.CancelSpell);
        return true;
    }

    private bool isUninterruptableByDelay(InterruptFlag interrupt_reason)
    {
        var unInterruptable = currentSpellData.hasUnInterruptableWindow.Find(r => r.Flag == interrupt_reason);
        //chek si 
        if (unInterruptable.Phase.HasFlag(currentPhase) && unInterruptable.time_amount > spellElapsedTime)
            return true;
        return false; 
    }
    private bool IsDelayedUninterruptible(InterruptFlag interrupt_reason)
    {
        Debug.Log(currentSpellData.isUninterruptable);
        return currentSpellData.isUninterruptable
            && currentSpellData.unInterrumptableDelayBy.HasFlag(interrupt_reason)
            && currentSpellData.UnInterruptableDelay >= spellElapsedTime;
    }
    //public bool TryInterruptSpell(Spell_data spellData, InterruptFlag interrupt_reason)
    //{
        //if (spellRunning)
        //{
            //if (currentSpellName == spellData.spellName && spellData.isInterruptableBy.HasFlag(interrupt_reason))
            //{
                //CancelCurrentSpell(interrupt_reason);
                //return true;
            //}
        //}
        //return false;
    //}
    //public bool TryInterruptCurrentSpell(InterruptFlag interrupt_reason)
    //{
        //if (spellRunning)
        //{
            //if (currentSpellData.isInterruptableBy.HasFlag(interrupt_reason))
            //{
                //if (currentSpellData.isUninterruptable && currentSpellData.unInterrumptableDelayBy.HasFlag(interrupt_reason) && currentSpellData.UnInterruptableDelay < spellElapsedTime)
                    //CancelCurrentSpell(interrupt_reason);
                //return true;
            //}
        //}
        //return false;
    //}
    private void CancelCurrentSpell(InterruptFlag interrupt_reason, SpellInterruptionType interrupt_type)
    {
        if (currentSpell == null || spellFateToken.IsCanceled || spellFateToken.SkipRequested) return;
        //Debug.Log("Time elapsed = " + spellElapsedTime);
        SpellCancelBy spellCancelBy = interrupt_reason switch
        {
            InterruptFlag.KeyDown => SpellCancelBy.keyDown,
            InterruptFlag.KeyUp => SpellCancelBy.KeyUp,
            InterruptFlag.Movement => SpellCancelBy.MovementKey,
            InterruptFlag.Stun => SpellCancelBy.Stun,
            InterruptFlag.EnnemyHit => SpellCancelBy.EnemyHit,
            _ => SpellCancelBy.None
        };
        if (interrupt_type == SpellInterruptionType.CancelSpell)
            spellFateToken.Cancel(spellCancelBy);
        else
            spellFateToken.RequestSkip(spellCancelBy);
    }
private bool TryGetInterruption(InterruptFlag flag, SpellPhase phase, out SpellInterruption_data interrupt_data)
{
    foreach (var r in currentSpellData.IsInterruptableBy)
    {
        if (r.Interrupt == flag && r.Phase == phase)
        {
            interrupt_data = r;
            return true;
        }
    }
    interrupt_data = default;
    return false;
}
private bool TryGetImmunity(InterruptFlag flag, SpellPhase phase, out float interruptWindow)
{
    foreach (var r in currentSpellData.hasUnInterruptableWindow)
    {
        if (r.Interrupt == flag && r.Phase == phase)
        {
            interruptWindow = r.time_amount;
            return true;
        }
    }
    interruptWindow = 0;
    return false;
}
}