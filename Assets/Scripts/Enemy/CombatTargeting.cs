using UnityEngine;

public class CombatTargeting
{
    private readonly CharacterCombat combat;
    private readonly Camera cam;
    private readonly Color highlightColor;
    private readonly LayerMask targetableLayers;
    private readonly MaterialPropertyBlock propBlock;

    private GameObject hoveredObject;
    private Renderer hoveredRenderer;
    private Color originalColor;

    public CombatTargeting(CharacterCombat combat, Camera cam, Color highlightColor, LayerMask targetableLayers)
    {
        this.combat = combat;
        this.cam = cam;
        this.highlightColor = highlightColor;
        this.targetableLayers = targetableLayers;
        propBlock = new MaterialPropertyBlock();
    }

    public void Tick()
    {
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        Debug.DrawRay(ray.origin, ray.direction * 1000f, Color.red);
        if (Physics.Raycast(ray, out RaycastHit hit, 1000f, targetableLayers)
            && ComponentUtils.TryGetDamageable(hit.collider, out IDamageable damageable))
        {
            GameObject hitObject = hit.collider.gameObject;

            if (hitObject != hoveredObject)
            {
                ClearHighlight();
                SetHighlight(hitObject);
                combat.SetTarget(hit.collider);
            }
        }
        else
        {
            ClearHighlight();
        }
    }

    private void SetHighlight(GameObject target)
    {
        Renderer rend = target.GetComponent<Renderer>();
        if (rend == null) return;
        hoveredObject = target;
        hoveredRenderer = rend;

        rend.GetPropertyBlock(propBlock);
        originalColor = rend.sharedMaterial.GetColor("_Color");
        propBlock.SetColor("_Color", highlightColor);
        rend.SetPropertyBlock(propBlock);
    }

    private void ClearHighlight()
    {
        if (hoveredRenderer != null)
        {
            hoveredRenderer.GetPropertyBlock(propBlock);
            propBlock.SetColor("_Color", originalColor);
            hoveredRenderer.SetPropertyBlock(propBlock);
        }

        hoveredObject = null;
        hoveredRenderer = null;
        combat.SetTarget(null);
    }

    // à appeler si le CharacterCombat est détruit/désactivé pendant qu'un objet est highlighté
    public void ClearHighlightExternally() => ClearHighlight();
}