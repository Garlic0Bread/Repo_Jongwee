using UnityEngine;
using UnityEngine.UI;

public class AbilityCooldownUI : MonoBehaviour
{
    private float cooldownTimer;
    private float cooldownDuration;

    [SerializeField] private Image cooldownImage;
    public bool IsCoolingDown => cooldownTimer > 0f;

    public void StartCooldown(float duration)
    {
        cooldownDuration = duration;
        cooldownTimer = duration;
        cooldownImage.fillAmount = 1f;
    }

    void Update()
    {
        if (cooldownTimer <= 0f)
            return;

        cooldownTimer -= Time.deltaTime;
        cooldownImage.fillAmount = cooldownTimer / cooldownDuration;
    }
}
