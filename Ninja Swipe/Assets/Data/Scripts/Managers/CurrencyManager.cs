using TMPro;
using UnityEngine;

public class CurrencyManager : MonoBehaviour
{
    public static CurrencyManager Instance;

    [SerializeField] private TextMeshProUGUI bankKernelsTxt;
    public int _kernels;

    private void Awake()
    {
        Instance = this;
       _kernels = PlayerPrefs.GetInt("TotalKernels", 0);

        UIUpdate_TotalKernels();
    }

    public void SpendKernels(int kernel)
    {
        _kernels -= kernel;

        UIUpdate_TotalKernels();
    }

    public void UIUpdate_TotalKernels(int totalEarnedThisRun = 0)
    {
        PlayerPrefs.SetInt("TotalKernels", _kernels + totalEarnedThisRun);
        PlayerPrefs.Save();

        if (bankKernelsTxt) bankKernelsTxt.text = _kernels.ToString();//updates total kernels available
    }
}
