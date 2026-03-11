using UnityEngine;
using UnityEngine.UI;

public class BossHealth : MonoBehaviour
{
    [SerializeField] private int maxHealth;
    [SerializeField] private string bossName;
    [SerializeField] private Slider healthBar;
    [SerializeField] private AudioClip sfx_Damage;
    [SerializeField] private PlayerVFXProfile vfxProfile;

    private int _currentHealth;

    void Start()
    {
        _currentHealth = maxHealth;

        if (healthBar)
        {
            healthBar.maxValue = maxHealth;
            UpdateUI();
        }
    }

    public void TakeDamage(int amount)
    {
        SFXPooler.Instance.PlaySFX(sfx_Damage, 0.7f);

        _currentHealth -= amount;
        UpdateUI();

        if (_currentHealth <= 0) BossDefeated();
    }
    private void BossDefeated()
    {
        BossSpawner.Instance.RegisterBossDefeat(bossName);

        GameProgressManager.Instance.AddEggs(false, transform.position, true, 30);
        GameProgressManager.Instance.ResetBossProgress();
        GameProgressManager.Instance.ResumeRun();
        Destroy(gameObject);
    }
    private void UpdateUI()
    {
        if (healthBar)
            healthBar.value = _currentHealth;
    }
}
