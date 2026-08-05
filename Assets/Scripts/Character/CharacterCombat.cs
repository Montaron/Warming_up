using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEditor.SearchService;
using UnityEngine;
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
    public event Action<CharacterStateType> OnSpellPhaseChanged; 

    public Collider currentTarget {get; private set;}
    private List<Collider> detectedTargets = new();

    //Others
    public string currentSpellName {get; private set;}
    public Spell_data currentSpellData {get; private set;}
    public bool spellRunning {get; private set;}
    public float spellElapsedTime {get; private set;}

    private ISpell currentSpell;
    private SpellFateToken spellFateToken;

    void Start()
    {

    }
    void Update()
    {
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

        currentTarget      = null;
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
        ((BaseSpellRuntime)currentSpell).OnSpellPhaseReached -= HandleSpellPhaseChange;
        spellElapsedTime = 0f;
        currentSpell = null;
        currentSpellName = null;
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
        BaseSpellRuntime spell = (BaseSpellRuntime)spellData.CreateSpellRuntime(gameObject, null);
        spell.OnSpellPhaseReached += HandleSpellPhaseChange;
        return spell;
    }

    void HandleSpellPhaseChange(BaseSpellRuntime.SpellPhase phase)
    {
        Debug.Log("PHASE CHANGED" + phase);
        if (phase == BaseSpellRuntime.SpellPhase.OnPhaseLoop_Enter)
        {
            if (currentSpellData.SpellCastType == SpellCastType.Channeled)
            {
                OnSpellPhaseChanged?.Invoke(CharacterStateType.Channeling);
            }
        }
        if (phase == BaseSpellRuntime.SpellPhase.OnPhaseLoop_End)
        {
            OnSPellPhaseChanged?.Invoke(CharacterStateType.Attacking)     
        }
    }
    private void spellFateToken_OnSpellCanceled(SpellCancelBy by)
    {
        // Debug.Log($"Spell {currentSpellName} canceled by {by}");
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
    public bool Test_TryInterruptSpell(string incomingSpellName, InterruptFlag interrupt_reason)
    {
        if (spellRunning)
        {
            if (!string.isNullOrEmpty(incomingSpellName))
            {
                if (currentSpellName == incomingSpellName && spellData.interruptableBy.HasFlag(interrupt_reason))
                {
                    if (currentSpellData.isUninterruptable && currentSpellData.unInterrumptableDelayBy.HasFlag(interrupt_reason) && currentSpellData.UnInterruptableDelay < spellElapsedTime)
                    CancelCurrentSpell(interrupt_reason);
                    return true;
                }
            }
            else
            {
                if (currentSpellData.interruptableBy.HasFlag(interrupt_reason))
                {
                    if (currentSpellData.isUninterruptable && currentSpellData.unInterrumptableDelayBy.HasFlag(interrupt_reason) && currentSpellData.UnInterruptableDelay < spellElapsedTime)
                    CancelCurrentSpell(interrupt_reason);
                    return true;
                }
            }
        }
    }
    public bool TryInterruptSpell(Spell_data spellData, InterruptFlag interrupt_reason)
    {
        if (spellRunning)
        {
            if (currentSpellName == spellData.spellName && spellData.interruptableBy.HasFlag(interrupt_reason))
            {
                CancelCurrentSpell(interrupt_reason);
                return true;
            }
        }
        return false;
    }
    public bool TryInterruptCurrentSpell(InterruptFlag interrupt_reason)
    {
        if (spellRunning)
        {
            if (currentSpellData.interruptableBy.HasFlag(interrupt_reason))
            {
                if (currentSpellData.isUninterruptable && currentSpellData.unInterrumptableDelayBy.HasFlag(interrupt_reason) && currentSpellData.UnInterruptableDelay < spellElapsedTime)
                CancelCurrentSpell(interrupt_reason);
                return true;
            }
        }
        return false;
    }
    private void CancelCurrentSpell(InterruptFlag interrupt_reason)
    {
        if (currentSpell == null || spellFateToken.IsCanceled) return;

        SpellCancelBy spellCancelBy = interrupt_reason switch
        {
            InterruptFlag.KeyDown => SpellCancelBy.keyDown,
            InterruptFlag.KeyUp => SpellCancelBy.KeyUp,
            InterruptFlag.Movement => SpellCancelBy.MovementKey,
            InterruptFlag.Stun => SpellCancelBy.Stun,
            InterruptFlag.EnnemyHit => SpellCancelBy.EnemyHit,
            _ => SpellCancelBy.None
        };
        spellFateToken.Cancel(spellCancelBy);
    }
}