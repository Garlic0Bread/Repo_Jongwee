using UnityEngine;

public class HomePanel_BtnManager : MonoBehaviour
{
    [SerializeField] private AudioClip roosterCrow_sfx;

    public void Quit()
    {
        Application.Quit();
    }
    public void Home_Panel()
    {
        HubUIManager.Instance.SwitchState(HubUIManager.HubState.Home);
    }
    public void Store_Panel()
    {
        HubUIManager.Instance.SwitchState(HubUIManager.HubState.Store);
    }
    public void Pause_Panel()
    {
        HubUIManager.Instance.SwitchState(HubUIManager.HubState.Pause);
    }
    public void Gameplay_Panel()
    {
        HubUIManager.Instance.SwitchState(HubUIManager.HubState.Gameplay);
    }
    public void Settings_Panel()
    {
        HubUIManager.Instance.SwitchState(HubUIManager.HubState.Settings);
    }

    public void StartGame()
    {
        GameManager.Instance.StartGame();
        SFXPooler.Instance.PlaySFX(roosterCrow_sfx, 0.7f);
    }
}
