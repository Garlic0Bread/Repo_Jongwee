using DG.Tweening;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    public static TutorialManager Instance;

    [SerializeField] private Transform spawnPoint;
    [SerializeField] private TextMeshProUGUI hintText;
    [SerializeField] private List<TutorialStepData> tutorialSteps;

    private int currentStepIndex = 0;
    private Transform hintTransform;

    void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);

        hintTransform = hintText.transform;
    }
    public void StartTutorial()
    {
        currentStepIndex = 0;
        SpawnCurrentStep();
    }

    public void TriggerEvent(TutorialTrigger trigger)
    {
        if (currentStepIndex >= tutorialSteps.Count) return;

        var step = tutorialSteps[currentStepIndex];

        if (step.trigger != trigger) return;

        currentStepIndex++;

        SpawnCurrentStep();
    }

    void SpawnCurrentStep()
    {
        if (currentStepIndex >= tutorialSteps.Count)
        {
            CompleteTutorial();
            return;
        }

        var step = tutorialSteps[currentStepIndex];

        DOVirtual.DelayedCall(step.spawnDelay, () =>
        {
            hintText.gameObject.SetActive(true);
            hintText.text = step.tutorialText;

            hintTransform.localScale = Vector3.zero;
            hintTransform.DOScale(1f, 0.5f).SetEase(Ease.OutBack);

            Instantiate(step.prefabToSpawn, spawnPoint.transform.position, Quaternion.identity);

            // Unlock abilities defined by this step
            if (step.abilitiesUnlocked != PlayerPermissions.None)
            {
                GameManager.Instance.UnlockAbility(step.abilitiesUnlocked);
            }
        });
    }
    void CompleteTutorial()
    {
        hintText.text = "GOOD LUCK!";
        hintTransform.DOPunchScale(Vector3.one * 0.2f, 0.5f);

        GameManager.Instance._isTutorialPhase = false;

        DOVirtual.DelayedCall(1.5f, () =>
        {
            hintText.gameObject.SetActive(false);
            GameManager.Instance.UnlockGameplay();
        });
    }
}
