using DG.Tweening;
using TMPro;
using UnityEngine;

public enum TutorialStep { None, Obstacle_1, Obstacle_2, Kernel, Egg, Complete }

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Tutorial Prefabs")]
    [SerializeField] private GameObject tutorialObstacle_1;
    [SerializeField] private GameObject tutorialObstacle_2;
    [SerializeField] private GameObject tutorialKernel;
    [SerializeField] private GameObject tutorialEgg;

    [Header("References")]
    [SerializeField] private TextMeshProUGUI tutorialHintText;
    [SerializeField] private AudioClip gameplayTrack;

    [Header("State")]
    public TutorialStep currentStep = TutorialStep.None;
    public bool canStartGame = false;
    public bool hasPlayedBefore;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        DontDestroyOnLoad(gameObject);
        MusicPlayer.Instance.PlayMusic(gameplayTrack);

        //PlayerPrefs.DeleteKey("HasLaunchedBefore"); //enable to test tutorial phase
        hasPlayedBefore = PlayerPrefs.GetInt("HasLaunchedBefore", 0) == 1;
    }

    public void StopGame()
    {
        Player_Controller player = FindFirstObjectByType<Player_Controller>();
        player.gameObject.SetActive(false);
        canStartGame = false;
    }
    public void StartGame()
    {
        if (!hasPlayedBefore)
        {
            PlayerPrefs.SetInt("HasLaunchedBefore", 1);
            PlayerPrefs.Save();
            StartTutorial();
        }
        else
        {
            UnlockGameplay();
        }
    }
    public void UnlockGameplay()
    {
        canStartGame = true;
        currentStep = TutorialStep.Complete;

        foreach (var spawner in FindObjectsByType<Obstacle_Spawner>(FindObjectsSortMode.None))
            spawner.enabled = true;

        var kernelSpawner = FindFirstObjectByType<KernelSpawner>();
        if (kernelSpawner) kernelSpawner.enabled = true;

        GameProgressManager.Instance.CalculateDistances();
    }

    #region Tutorial Logic
    private void SpawnTutorialObject(GameObject prefab, string hint)
    {
        tutorialHintText.gameObject.SetActive(true);

        tutorialHintText.text = hint;
        tutorialHintText.transform.localScale = Vector3.zero;
        tutorialHintText.transform.DOScale(1f, 0.5f).SetEase(Ease.OutBack);

        Obstacle_Spawner spawner = FindFirstObjectByType<Obstacle_Spawner>();
        if (spawner != null)
        {
            Instantiate(prefab, spawner.transform.position, Quaternion.identity);
        }
    }
    public void StartTutorial()
    {
        canStartGame = false;
        currentStep = TutorialStep.Obstacle_1;
        SpawnTutorialObject(tutorialObstacle_1, "SWIPE TO ATTACK!");
    }

    public void OnObstacleCleared()
    {
        if (currentStep != TutorialStep.Obstacle_1) return;

        currentStep = TutorialStep.Obstacle_2;
        DOVirtual.DelayedCall(1f, () =>//basically a 'wait for X seconds'
            SpawnTutorialObject(tutorialObstacle_2, "SWIPE FOR ABILITY"));
    }
    public void OnObstacleCleared_2()
    {
        if (currentStep != TutorialStep.Obstacle_2) return;

        currentStep = TutorialStep.Kernel;
        DOVirtual.DelayedCall(1f, () =>
            SpawnTutorialObject(tutorialKernel, "COLLIDE TO COLLECT KERNELS!"));
    }
    public void OnKernelCollected()
    {
        if (currentStep != TutorialStep.Kernel) return;

        currentStep = TutorialStep.Egg;
        DOVirtual.DelayedCall(1f, () =>
            SpawnTutorialObject(tutorialEgg, "COLLECT EGGS FOR BONUSES!"));
    }
    public void OnEggCollected()
    {
        if (currentStep != TutorialStep.Egg) return;

        currentStep = TutorialStep.Complete;
        tutorialHintText.text = "GOOD LUCK!";
        tutorialHintText.transform.DOPunchScale(Vector3.one * 0.2f, 0.5f);

        DOVirtual.DelayedCall(1.5f, () =>
        {
            tutorialHintText.gameObject.SetActive(false);
            UnlockGameplay();
        });
    }
    #endregion
}
