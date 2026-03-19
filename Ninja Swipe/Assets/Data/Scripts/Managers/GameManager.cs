using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("References")]
    public PlayerPermissions unlockedPermissions;
    [SerializeField] private AudioClip gameplayTrack;
    [SerializeField] private TutorialManager tutorialManager;

    [Header("State")]
    public bool canStartGame = false;
    public bool _isTutorialPhase = false;
    public bool hasPlayedBefore;

    private Obstacle_Spawner[] obstacleSpawners;
    private KernelSpawner kernelSpawner;
    private Player_Controller player;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        obstacleSpawners = FindObjectsByType<Obstacle_Spawner>(FindObjectsSortMode.None);
        player = FindFirstObjectByType<Player_Controller>();

        MusicPlayer.Instance.PlayMusic(gameplayTrack);

        //PlayerPrefs.DeleteKey("HasLaunchedBefore"); //enable to test tutorial
        hasPlayedBefore = PlayerPrefs.GetInt("HasLaunchedBefore", 0) == 1;
    }

    public void StopGame()
    {
        if (player != null)
            player.gameObject.SetActive(false);

        canStartGame = false;
        GameManager.Instance.unlockedPermissions = PlayerPermissions.None;

        if (kernelSpawner == null)
        {
            kernelSpawner = FindFirstObjectByType<KernelSpawner>();
            kernelSpawner.enabled = false;
        }
        else
            kernelSpawner.enabled = false;
    }
    public void StartGame()
    {
        if (PlayerPrefs.GetInt("HasLaunchedBefore", 0) == 0)
        {
            PlayerPrefs.SetInt("HasLaunchedBefore", 1);
            PlayerPrefs.Save();

            hasPlayedBefore = true;

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
        UnlockAbility(PlayerPermissions.Flap | PlayerPermissions.Attack | PlayerPermissions.Ability | PlayerPermissions.Gameplay);

        foreach (var spawner in obstacleSpawners)
            spawner.enabled = true;

        if (kernelSpawner == null)
        {
            kernelSpawner = FindFirstObjectByType<KernelSpawner>();
            kernelSpawner.enabled = true;
        }
        else
            kernelSpawner.enabled = true;

        GameProgressManager.Instance.CalculateDistances();
    }

    void StartTutorial()
    {
        canStartGame = false;
        _isTutorialPhase = true;
        unlockedPermissions = PlayerPermissions.None;

        if (tutorialManager != null)
            tutorialManager.StartTutorial();

    }
    public bool HasAbility(PlayerPermissions permission)
    {
        return unlockedPermissions.HasFlag(permission);
    }
    public void UnlockAbility(PlayerPermissions permission)
    {
        unlockedPermissions |= permission;
    }
}
