using UnityEngine;

public class KernelSpawner : MonoBehaviour
{
    private KernelPattern lastPatterm;

    [Header("References")]
    [SerializeField] private GameObject kernelPrefab;
    [SerializeField] private KernelPattern[] patterns;

    [Header("Spawn Settings")]
    [SerializeField] private float spawnInterval = 2.5f;
    [SerializeField] private float verticalRang = 3f;

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Vector3 up = transform.position + Vector3.up * verticalRang;
        Vector3 down = transform.position + Vector3.down * verticalRang;

        Gizmos.DrawLine(up, down);//line representing yRange

        Gizmos.DrawLine(up + Vector3.left * 0.5f, up + Vector3.right * 0.5f);
        Gizmos.DrawLine(down + Vector3.left * 0.5f, down + Vector3.right * 0.5f);
    }

    private void Awake()
    {
        enabled = false;
    }
    private void Start()
    {
        InvokeRepeating(nameof(SpawnPattern), 1f, spawnInterval);
    }

    private void SpawnPattern()
    {
        KernelPattern pattern = Get_RandomPattern();
        lastPatterm = pattern;

        float baseY = Random.Range(-verticalRang, verticalRang);
        Vector3 basePosition = transform.position + Vector3.up * baseY;

        foreach (Vector2 offset in pattern.localOffset)
        {
            Vector3 spawnPos = basePosition + new Vector3(offset.x, offset.y, 0f);
            VFXPooler.Instance.SpawnFromPool(kernelPrefab, spawnPos, Quaternion.identity, 10);
        }
    }
    private KernelPattern Get_RandomPattern()
    {
        KernelPattern selected;
        do
        {
            selected = patterns[Random.Range(0, patterns.Length)];
        }
        while (selected == lastPatterm);
        return selected;
    }
}
