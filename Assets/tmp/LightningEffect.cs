using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightningEffect : MonoBehaviour
{
    [SerializeField] private List<LineRenderer> lineRenderer;
    [SerializeField] private int segmentCount = 10;
    [SerializeField] private float jitterAmount = 0.3f;
    [SerializeField] private float updateInterval = 0.02f; // fréquence du "scintillement"
    [SerializeField] private float frameRate = 24f; // frames par seconde de lecture
    [SerializeField] private ParticleSystem sparkParticles;

    public Transform caster;
    public Transform target;
    private float jitterTimer;
    private int[] currentFrames;
    private float[] animationTimers;

    public void Play(Transform caster, Transform target)
    {
        this.caster = caster;
        this.target = target;
        foreach (var line in lineRenderer)
        {
            line.positionCount = segmentCount;
        }
        gameObject.SetActive(true);
    }
    private void Start()
    {
        currentFrames = new int[lineRenderer.Count];
        animationTimers = new float[lineRenderer.Count];

        for (int i = 0; i < lineRenderer.Count; i++)
        {
            currentFrames[i] = Random.Range(0, 16);
            animationTimers[i] = Random.Range(0f, 1f / frameRate);
        }
    }
    private void Update()
    {
        if (caster == null || target == null) return;

        jitterTimer += Time.deltaTime;
        if (jitterTimer >= updateInterval)
        {
            jitterTimer = 0f;
            foreach (var line in lineRenderer)
                UpdateLightningPath(line);
            //UpdateSparkPosition();
        }

        UpdateAnimation(); // indépendant du jitter, tourne à son propre rythme
    }
    private void UpdateAnimation()
    {
        for (int i = 0; i < lineRenderer.Count; i++)
        {
            animationTimers[i] += Time.deltaTime;

            if (animationTimers[i] >= 1f / frameRate)
            {
                animationTimers[i] -= 1f / frameRate;

                currentFrames[i] =
                    (currentFrames[i] + 1) % 16;

                int col = currentFrames[i] % 4;
                int row = currentFrames[i] / 4;

                lineRenderer[i].material.mainTextureOffset =
                    new Vector2(col * 0.25f, row * 0.25f);

                lineRenderer[i].material.mainTextureScale =
                    new Vector2(0.25f, 0.25f);
            }
        }
    }
    private void UpdateLightningPath(LineRenderer lineRenderer)
    {
        Vector3 start = caster.position;
        Vector3 end = target.position;

        for (int i = 0; i < segmentCount; i++)
        {
            float t = i / (float) (segmentCount - 1);
            Vector3 basePoint = Vector3.Lerp(start, end, t);

            // pas de jitter aux extrémités, pour rester ancré sur caster/target
            if (i != 0 && i != segmentCount - 1)
            {
                Vector3 randomOffset = new Vector3(
                    Random.Range(-jitterAmount, jitterAmount),
                    Random.Range(-jitterAmount, jitterAmount),
                    Random.Range(-jitterAmount, jitterAmount)
                );
                basePoint += randomOffset;
            }

            lineRenderer.SetPosition(i, basePoint);
        }
    }
    private void UpdateSparkPosition()
    {
        Vector3 start = caster.position;
        Vector3 end = target.position;
        Vector3 mid = (start + end) / 2f;
        float distance = Vector3.Distance(start, end);

        sparkParticles.transform.position = mid;
        sparkParticles.transform.LookAt(end);

        var shape = sparkParticles.shape;
        shape.radius = distance / 2f; // Edge shape s'étend sur "radius" de chaque côté du pivot
    }
    public void Stop() => gameObject.SetActive(false);
}