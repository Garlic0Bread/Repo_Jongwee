using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public partial class VFXPooler : MonoBehaviour
{
    public static VFXPooler Instance;
    private Dictionary<string, Queue<GameObject>> pools = new();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            SceneManager.sceneLoaded += OnSceneLoaded;//+= subscribes to the sceneLoaded event
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;//-= unsubscribes to avoid memory leaks
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)//run every time scene restarts or changes
    {
        pools.Clear(); //empty the dictionary so we don't try to use dead objects
    }

    public GameObject SpawnFromPool(GameObject prefab, Vector3 position, Quaternion rotation, float lifetime)
    {
        string key = prefab.name;

        if (!pools.ContainsKey(key))//if item doesnt exits in current pool, add it
            pools[key] = new Queue<GameObject>();

        GameObject obj = null;

        //checking if we have an object AND ensure it hasn't been destroyed  
        while (pools[key].Count > 0 && obj == null)
        {
            obj = pools[key].Dequeue();
        }

        if (obj != null)
        {
            obj.SetActive(true);
        }
        else
        {
            obj = Instantiate(prefab);
            obj.name = key;
        }

        obj.transform.SetPositionAndRotation(position, rotation);
        StartCoroutine(DespawnAfterTime(obj, lifetime, key));

        return obj;
    }

    private IEnumerator DespawnAfterTime(GameObject obj, float time, string key)
    {
        yield return new WaitForSeconds(time);

        if (obj != null)
        {
            obj.SetActive(false);
            pools[key].Enqueue(obj);
        }
    }
}

