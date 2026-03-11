using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct BossProfile
{
    public string bossName;
    public GameObject prefab;
    public DistanceMilestones milestoneData;
}

public class BossSpawner : MonoBehaviour
{
    public static BossSpawner Instance;

    [Header("Boss Rotation")]
    [SerializeField] private List<BossProfile> bossRotation = new();
    [SerializeField] private Transform spawnPoint;
    private int _currentIndex = 0;

    private void Awake() => Instance = this;

    public BossProfile GetCurrentProfile() => bossRotation[_currentIndex];

    public void SpawnBoss()
    {
        Instantiate(GetCurrentProfile().prefab, spawnPoint.position, Quaternion.identity);

        // Cycle to next boss for the next segment
        _currentIndex = (_currentIndex + 1) % bossRotation.Count;
    }
}
