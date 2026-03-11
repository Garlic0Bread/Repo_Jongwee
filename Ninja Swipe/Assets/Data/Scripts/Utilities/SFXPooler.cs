using UnityEngine;
using System.Collections.Generic;

public class SFXPooler : MonoBehaviour
{
    private int index;
    private List<AudioSource> pool = new ();

    public static SFXPooler Instance;
    [SerializeField] private int poolSize = 10;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            CreatePool();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void CreatePool()
    {
        for (int i = 0; i < poolSize; i++)
        {
            GameObject obj = new ("SFX_" + i);
            obj.transform.parent = transform;

            AudioSource source = obj.AddComponent<AudioSource>();

            source.playOnAwake = false;
            source.loop = false;
            source.spatialBlend = 0f; 

            pool.Add(source);
        }
    }

    public void PlaySFX(AudioClip clip, float volume = 1f, float pitch = 1f)
    {
        AudioSource source = pool[index];

        source.clip = clip;
        source.volume = volume;
        source.pitch = pitch;

        source.Play();

        index++;
        if (index >= pool.Count)
            index = 0;
    }
}

