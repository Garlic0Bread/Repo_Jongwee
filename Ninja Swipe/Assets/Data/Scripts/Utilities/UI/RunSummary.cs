using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RunSummary : MonoBehaviour
{
    [Header("UI Text References")]
    [SerializeField] private TextMeshProUGUI eggBonusText;
    [SerializeField] private TextMeshProUGUI milesValueText;
    [SerializeField] private TextMeshProUGUI kernelsValueText;
    [SerializeField] private TextMeshProUGUI totalEarnedText;

    [Header("Animation Settings")]
    [SerializeField] private float countDuration = 1.5f;

    public void RestartGame()
    {
        GameManager.Instance.StopGame();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        Time.timeScale = 1f;
    }

    public void ShowSummary(int miles, int kernels, int bonus, int total)
    {
        gameObject.SetActive(true);
        StartCoroutine(AnimateSummary(miles, kernels, bonus, total));
    }
    private IEnumerator AnimateSummary(int miles, int kernels, int bonus, int total)
    {
        float elapsed = 0;

        while (elapsed < countDuration)
        {
            elapsed += Time.deltaTime;
            float progress = elapsed / countDuration;

            //ADD KERNEL COUNT SOUND HERE

            int currentMiles = (int)Mathf.Lerp(0, miles, progress);
            int currentBonus = (int)Mathf.Lerp(0, bonus, progress);
            int currentTotal = (int)Mathf.Lerp(0, total, progress);
            int currentKernels = (int)Mathf.Lerp(0, kernels, progress);


            eggBonusText.SetText($"+{currentBonus}");
            milesValueText.SetText($"{currentMiles}m");
            totalEarnedText.SetText(currentTotal.ToString());
            kernelsValueText.SetText(currentKernels.ToString());

            yield return null;
        }

        //this is to make sure we end on the exact final numbers
        milesValueText.SetText($"{miles}m");
        eggBonusText.SetText($"+{bonus} bonus");
        totalEarnedText.SetText(total.ToString());
        kernelsValueText.SetText(kernels.ToString());
    }
}
