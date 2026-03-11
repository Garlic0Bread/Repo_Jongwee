using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SpawnableItem
{
    public string name;
    public float cooldown;//prevents this specific item from spawning too soon
    public GameObject prefab;
    [Range(0, 100)] public float weight; //higher, more common the object
    [HideInInspector] public float lastSpawnTime;
}

public class Obstacle_Spawner : MonoBehaviour
{
    [Header("Spawn Configuration")]
    [SerializeField] private float yRange = 4.5f;
    [SerializeField] private List<SpawnableItem> spawnTable = new ();
    
    [Header("Pacing (The Rhythm)")]
    [SerializeField] private float minSpawnDelay = 1.5f;
    [SerializeField] private float maxSpawnDelay = 2.5f;
    [SerializeField] private float intensityMultiplier = 1f;

    private float _lastY;
    private float _nextSpawnTime;
    private float _currentSpawnRate;
    private bool _spawningEnabled = true;

    private void Start()
    {
        _currentSpawnRate = maxSpawnDelay;
        SetNextSpawnTime();

        if (GameProgressManager.Instance != null)
        {//subscribe to a event StopSpawning via bool onBossReached

            //advantage of addlistener is that we dont have to run this every frame, the spanwer just waits for the signa. cool
            GameProgressManager.Instance.onBossReached.AddListener(StopSpawningForBoss);//when boss is here
            GameProgressManager.Instance.onBossDefeated.AddListener(StartSpawningAgain);//when boss dies
        }
    }
    private void Update()
    {
        if (!_spawningEnabled || !GameManager.Instance.canStartGame || !GameProgressManager.Instance._isRunning)
            return;

        if (Time.time >= _nextSpawnTime)
        {
            SpawnFromTable();
            SetNextSpawnTime();
        }
    }
    private void OnDestroy()
    {
        if (GameProgressManager.Instance != null)
        {//unsubscribe when destroyed to prevent memory errors
            GameProgressManager.Instance.onBossReached.RemoveListener(StopSpawningForBoss);
            GameProgressManager.Instance.onBossDefeated.RemoveListener(StartSpawningAgain);
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawLine(transform.position + Vector3.up * yRange, transform.position + Vector3.down * yRange);
    }

    private void SpawnFromTable()
    {
        SpawnableItem itemToSpawn = GetWeightedItem();

        if (itemToSpawn != null && itemToSpawn.prefab != null)
        {
            float spawnY = CalculateY_Position();
            Vector3 spawnPos = transform.position + new Vector3(0f, spawnY, 0f);

            VFXPooler.Instance.SpawnFromPool(itemToSpawn.prefab, spawnPos, Quaternion.identity, 10);

            itemToSpawn.lastSpawnTime = Time.time;
            _lastY = spawnY;
        }
    }
    private float CalculateY_Position()
    {
        float newY;
        int attempts = 0;
        do
        {
            newY = Random.Range(-yRange, yRange);
            attempts++;
        } 

        //Ensure new object is a few units away from the last one vertically
        while (Mathf.Abs(newY - _lastY) < 1.5f && attempts < 10);

        return newY;
    }
    private void SetNextSpawnTime()
    {
        //increase difficulty by reducing the delay
        _currentSpawnRate = Mathf.Max(minSpawnDelay, _currentSpawnRate * intensityMultiplier);
        
        //avoid a readable/robotic beat
        float jitter = Random.Range(-0.2f, 0.2f);
        _nextSpawnTime = Time.time + _currentSpawnRate + jitter;
    }

    private void StopSpawningForBoss()
    {
        _spawningEnabled = false;
    }
    private void StartSpawningAgain()
    {
        _spawningEnabled = true;
    }

    private SpawnableItem GetWeightedItem()
    {
        float totalWeight = 0;
        List<SpawnableItem> availableItems = new List<SpawnableItem>();

        foreach (var item in spawnTable)
        {
            if (Time.time >= item.lastSpawnTime + item.cooldown)
            {//Filter out items on cooldown
                availableItems.Add(item);
                totalWeight += item.weight;
            }
        }

        float randomValue = Random.Range(0, totalWeight);
        float cursor = 0;

        foreach (var item in availableItems)
        {
            cursor += item.weight;
            if (randomValue <= cursor) return item;
        }

        return null;
    }
}
