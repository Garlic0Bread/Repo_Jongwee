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
    private int _currentIndex = 0;
    private Dictionary<string, int> bossLevels = new();

    [Header("Boss Rotation")]
    [SerializeField] private List<BossProfile> bossRotation = new();
    [SerializeField] private Transform spawnPoint;

    private void Awake()
    {
        Instance = this;

        // Initialize boss levels
        foreach (var boss in bossRotation)
        {
            bossLevels[boss.bossName] = 0;
        }
    }

    public BossProfile GetCurrentProfile() => bossRotation[_currentIndex];

    public void SpawnBoss()
    {
        BossProfile profile = GetCurrentProfile();

        GameObject bossObj = Instantiate(profile.prefab, spawnPoint.position, Quaternion.identity);

        // Apply difficulty level
        int level = bossLevels[profile.bossName];

        BossDifficulty boss = bossObj.GetComponent<BossDifficulty>();
        boss?.ApplyDifficulty(level);

        _currentIndex = (_currentIndex + 1) % bossRotation.Count;
    }

    public void RegisterBossDefeat(string bossName)
    {
        if (bossLevels.ContainsKey(bossName))
            bossLevels[bossName]++;
    }
}
