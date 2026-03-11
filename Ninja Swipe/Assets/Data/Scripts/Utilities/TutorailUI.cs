using UnityEngine;

public class TutorialUI : MonoBehaviour
{
    [SerializeField] private TMPro.TextMeshProUGUI messageText;

    public void ShowMessage(string msg)
    {
        messageText.text = msg;
        messageText.gameObject.SetActive(true);
    }

    public void HideMessage()
    {
        messageText.gameObject.SetActive(false);
    }
}
