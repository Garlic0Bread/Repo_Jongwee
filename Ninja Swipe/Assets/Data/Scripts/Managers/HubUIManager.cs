using UnityEngine;

public class HubUIManager : MonoBehaviour
{
    public static HubUIManager Instance;

    public enum HubState
    {
        Home,
        Store,
        Pause,
        Gameplay,
        Settings,
    }
    private HubState currentState;

    [Header("Panels")]
    [SerializeField] private GameObject homePanel;
    [SerializeField] private GameObject storePanel;
    [SerializeField] private GameObject pausePanel;
    [SerializeField] private GameObject gameplayPanel;
    [SerializeField] private GameObject settingsPanel;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public void SwitchState(HubState newState)
    {
        currentState = newState;

        homePanel.SetActive(false);
        storePanel.SetActive(false);
        pausePanel.SetActive(false);
        gameplayPanel.SetActive(false);
        settingsPanel.SetActive(false);

        switch (currentState)
        {
            case HubState.Home:
                homePanel.SetActive(true);
                Time.timeScale = 1f;
                break;

            case HubState.Gameplay:
                gameplayPanel.SetActive(true);
                Time.timeScale = 1f;
                break;

            case HubState.Store:
                storePanel.SetActive(true);
                break;

            case HubState.Settings:
                settingsPanel.SetActive(true);
                break;

            case HubState.Pause:
                pausePanel.SetActive(true);
                Time.timeScale = 0;
                break;
        }
    }
}

