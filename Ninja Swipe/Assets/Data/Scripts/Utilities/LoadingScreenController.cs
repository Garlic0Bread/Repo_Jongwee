using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingScreenController : MonoBehaviour
{
    private float timer = 0f;
    private bool isLoading = true;

    [Header("UI References")]
    [SerializeField] private Slider loadingBar;
    [SerializeField] private TMP_Text loadingText;

    [Header("Timing")]
    [SerializeField] private float loadDuration = 3f;

    void Start()
    {
        loadingBar.value = 0f;
    }

    void Update()
    {
        if (!isLoading) return;

        timer += Time.deltaTime;
        float progress = Mathf.Clamp01(timer / loadDuration);
        loadingBar.value = progress;

        if (loadingText != null)
            loadingText.text = $"Loading... {Mathf.RoundToInt(progress * 100)}%";

        if (progress >= 1f)
        {
            FinishLoading();
        }
    }

    void FinishLoading()
    {
        isLoading = false;
        SceneManager.LoadScene(1);
    }
}

