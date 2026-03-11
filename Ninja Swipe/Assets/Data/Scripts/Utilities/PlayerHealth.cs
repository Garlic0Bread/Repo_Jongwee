using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    [Header("Life Settings")]
    [SerializeField] private AudioClip chickinCluck_sfx;
    [SerializeField] private int maxLives = 3;
    private bool _isDead = false;
    private int _currentLives;

    [Header("UI Visuals")]
    [SerializeField] private GameObject[] heartIcons;
    [SerializeField] private Color lostHeartColor = Color.gray;

    private void Start()
    {
        _currentLives = maxLives;
        UpdateHeartsUI();
    }

    private void Die()
    {
        _isDead = true;
        GameProgressManager.Instance.FinalizeRun();
    }
    private void UpdateHeartsUI()
    {
        for (int i = 0; i < heartIcons.Length; i++)
        {
            if (i < _currentLives)
            {
                heartIcons[i].GetComponent<Image>().color = Color.white; 
            }
            else
            {
                heartIcons[i].GetComponent<Image>().color = lostHeartColor;
            }
        }
    }
    public void TakeDamage(int amount)
    {
        if (_isDead) return;

        SFXPooler.Instance.PlaySFX(chickinCluck_sfx, 0.7f);


        _currentLives -= amount;
        _currentLives = Mathf.Clamp(_currentLives, 0, maxLives);
        Camera.main.transform.DOShakePosition(0.2f, 0.4f);//screen shake & flash

        SpriteRenderer sr = GetComponentInChildren<SpriteRenderer>();
        if (sr != null)
        {
            sr.DOColor(Color.red, 0.1f).SetLoops(2, LoopType.Yoyo);
        }

        UpdateHeartsUI();

        if (_currentLives <= 0)
        {
            Die();
        }
    }
}
