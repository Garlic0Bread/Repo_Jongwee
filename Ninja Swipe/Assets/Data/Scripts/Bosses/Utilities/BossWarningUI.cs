using DG.Tweening;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;


public class BossWarningUI : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private CanvasGroup warningGroup;  
    [SerializeField] private TextMeshProUGUI warningText;
    [SerializeField] private AudioSource sfxSource;
    [SerializeField] private AudioClip sirenClip;

    [Header("Settings")]
    [SerializeField] private float warningDuration = 2f;

    public void PlayWarning(UnityEvent onComplete)
    {
        gameObject.SetActive(true);
        
        sfxSource.pitch = 0.8f;
        sfxSource.PlayOneShot(sirenClip);
        sfxSource.DOPitch(1.2f, warningDuration);//rising pitch sfx

        Sequence warningSequence = DOTween.Sequence();
        warningSequence.SetLoops(3); //blink x times

        warningSequence.Append(warningText.transform.DOPunchScale(Vector3.one * 0.5f, 0.5f));
        warningSequence.Append(warningGroup.DOFade(0f, 0.3f));

        //use onComplete event to spawn boss
        warningSequence.OnComplete(() =>
        {
            gameObject.SetActive(false);
            warningGroup.alpha = 1f;
            onComplete?.Invoke();
        });
    }
}
