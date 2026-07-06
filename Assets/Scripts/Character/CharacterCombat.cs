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

    public Collider currentTarget {get; private set;}
    private List<Collider> detectedTargets = new();

    //Others
    private ISpell currentSpell;
    public string currentSpellName {get; private set;}
    public Spell_data currentSpellData {get; private set;}
    private SpellFateToken spellFateToken;
    public bool spellRunning {get; private set;}
    void Start()
    {
    }
    void Update()
    {
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
        currentSpell = null;
        currentSpellName = null;
        spellFateToken.OnSpellCanceled -= spellFateToken_OnSpellCanceled;
        spellFateToken = null;
        spellRunning = false;
    }

    private ISpell InitSpell(Spell_data spellData)
    {
        spellRunning = true;
        spellFateToken = new SpellFateToken();
        currentSpellName = spellData.spellName;
        currentSpellData = spellData;
        spellFateToken.OnSpellCanceled += spellFateToken_OnSpellCanceled;
        return spellData.CreateSpellRuntime(gameObject, null);
    }

    private void spellFateToken_OnSpellCanceled(SpellCancelBy by)
    {
        Debug.Log($"Spell {currentSpellName} canceled by {by}");
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
        yield return StartCoroutine(currentSpell.StartSpell(gameObject));
        currentSpell.SpellEnd();
        OnSpellEnded(spellData);
        ResetSpell();
    }

    public bool TryInterruptSpell(Spell_data spellData)
    {
        if (spellRunning)
        {
            if (currentSpellName == spellData.spellName && spellData.interruptableBy == isInterruptableBy.Recast)
            {
                CancelCurrentSpell(SpellCancelBy.InputSpell);
                return true;
            }
        }
        return false;
    }
    public void CancelCurrentSpell(SpellCancelBy spellCancelBy)
    {
        if (currentSpell != null && !spellFateToken.IsCanceled)
            spellFateToken.Cancel(spellCancelBy);
    }
}