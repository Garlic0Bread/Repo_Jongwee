using UnityEngine;
using System.Collections;

public class MusicPlayer : MonoBehaviour
{
    public static MusicPlayer Instance;
    private AudioSource musicSource;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            musicSource = gameObject.AddComponent<AudioSource>();
            musicSource.loop = true;
            musicSource.playOnAwake = false;
            musicSource.spatialBlend = 0f;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void StopMusic()
    {
        musicSource.Stop();
    }
    public void PlayMusic(AudioClip clip, float volume = 0.15f)
    {
        if (musicSource.clip == clip)
            return;

        musicSource.clip = clip;
        musicSource.volume = volume;
        musicSource.Play();
    }

    public void FadeTo(AudioClip newClip, float duration = 1f)
    {
        StartCoroutine(FadeMusic(newClip, duration));
    }
    IEnumerator FadeMusic(AudioClip newClip, float duration)
    {
        float startVolume = musicSource.volume;

        // Fade out
        while (musicSource.volume > 0)
        {
            musicSource.volume -= startVolume * Time.deltaTime / duration;
            yield return null;
        }

        musicSource.Stop();
        musicSource.clip = newClip;
        musicSource.Play();

        // Fade in
        while (musicSource.volume < startVolume)
        {
            musicSource.volume += startVolume * Time.deltaTime / duration;
            yield return null;
        }
    }
}

