using DG.Tweening;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class GameProgressManager : MonoBehaviour
{
    public static GameProgressManager Instance { get; private set; }

    [Header("Things To Disable")]
    [SerializeField] private GameObject tapInput;
    [SerializeField] private GameObject swipeInput;
    [SerializeField] private GameObject abilityInput;

    [Header("Juice & V/SFX")]
    [SerializeField] private AudioSource sfxSource;
    [SerializeField] private PlayerVFXProfile vfxProfile;
    [SerializeField] private RectTransform kernelUITarget;
    [SerializeField] private GameObject animatedKernelPrefab;

    [Header("Distance Milestone Refs")]
    [SerializeField] private GameObject tickPrefab;
    [SerializeField] DistanceMilestones _currentMilestoneData;
    [SerializeField] private Color activeColor = Color.white;
    [SerializeField] private Color inactiveColor = Color.gray;
    [SerializeField] private RectTransform milestoneContainer;
    private List<Image> tickImages = new();

    [Header("Distance & Miles")]
    [SerializeField] private float milesPerSecond = 1f;
    [SerializeField] private TextMeshProUGUI milesText;
    private float _mileTimer;
    private int _totalMiles;

    [Header("Boss Setups")]
    [SerializeField] private BossWarningUI bossWarning;
    [SerializeField] private Slider progressSlider;
    public UnityEvent onBossDefeated;
    public UnityEvent onBossReached;
    public bool _isRunning = true;
    private float _currentBossDistance;
    private bool _bossTriggered = false;

    [Header("Currency (This Run)")]
    public int kernelsPerEgg = 5;
    public int kernelsPerGoldenEgg = 15;
    [SerializeField] private RunSummary summaryUI;
    [SerializeField] private TextMeshProUGUI kernelText;
    [SerializeField] private TextMeshProUGUI eggsText;
    private int _eggsCollected;
    private int _kernelsToBurst;
    private int _kernelsCollected;
    private int _goldenEggsCollected;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        SetupNextSegment();
    }
    private void Update()
    {
        if (!GameManager.Instance.canStartGame || !_isRunning || _bossTriggered)
            return;
        CalculateDistances();
    }

    #region Distance Calculations
    public void CalculateDistances()
    {
        _currentBossDistance += Time.deltaTime;

        if (progressSlider != null)
            progressSlider.value = Mathf.Clamp01(_currentBossDistance / _currentMilestoneData.distanceToBoss);

        _mileTimer += Time.deltaTime;
        if (_mileTimer >= 1f / milesPerSecond)
        {
            _totalMiles++;
            _mileTimer = 0f;
            UpdateMilesUI();
        }

        UpdateTicks();

        if (_currentBossDistance >= _currentMilestoneData.distanceToBoss)
        {
            _bossTriggered = true;
            bossWarning.PlayWarning(onBossReached);
        }
    }
    public void SetupNextSegment()
    {
        BossProfile currentBoss = BossSpawner.Instance.GetCurrentProfile();
        _currentMilestoneData = currentBoss.milestoneData;

        _currentBossDistance = 0f;
        _bossTriggered = false;

        ClearTicks();
        CreateTicks();
    }
    public float GetProgress()
    {
        return Mathf.Clamp01(_currentBossDistance / _currentMilestoneData.distanceToBoss);
    }

    private void ClearTicks()
    {
        foreach (var img in tickImages)
        {
            if (img != null) Destroy(img.gameObject);
        }
        tickImages.Clear();
    }
    private void CreateTicks()
    {
        foreach (float milestone in _currentMilestoneData.milestones)
        {
            GameObject tick = Instantiate(tickPrefab, milestoneContainer);
            RectTransform rectTransform = tick.GetComponent<RectTransform>();

            rectTransform.anchorMin = new Vector2(milestone, 0.5f);
            rectTransform.anchorMax = new Vector2(milestone, 0.5f);
            rectTransform.anchoredPosition = Vector2.zero;

            Image img = tick.GetComponent<Image>();
            img.color = inactiveColor;
            tickImages.Add(img);
        }
    }
    private void UpdateTicks()
    {
        float progress = GetProgress();
        for (int i = 0; i < tickImages.Count; i++)
        {
            if (progress >= _currentMilestoneData.milestones[i])
                tickImages[i].color = activeColor;
        }
    }
    #endregion

    #region Currency Logic
    public void AddKernels(int amount)
    {
        if (GameManager.Instance._isTutorialPhase)
            TutorialManager.Instance.TriggerEvent(TutorialTrigger.KernelCollected);

        _kernelsCollected += amount;

        UpdateCurrencyUI();
    }
    public void AddEggs(bool isGolden, Vector3 worldPos, bool isBossDeath = false, int bossKernels = 0)
    {
        if (GameManager.Instance._isTutorialPhase)
            TutorialManager.Instance.TriggerEvent(TutorialTrigger.EggCollected);

        if (isGolden) _goldenEggsCollected++;//decidng whether this is a golden egg or not
        else _eggsCollected++;

        if (isGolden) _kernelsToBurst = kernelsPerGoldenEgg;
        else if (!isGolden && !isBossDeath) _kernelsToBurst = kernelsPerEgg;

        if (!isGolden && isBossDeath)
        {//if we just defeated a boss, burst some kernels where they died
            _kernelsToBurst = bossKernels;
        }

        AudioClip vfxSoundEffect = vfxProfile.Get_SFX(VFXType.Collisions);

        for (int i = 0; i < _kernelsToBurst; i++)
        {
            float delay = i * 0.05f;

            DOVirtual.DelayedCall(delay, () =>
            {
                SpawnFlyingKernel(worldPos);

                if (vfxSoundEffect != null && sfxSource != null)
                {
                    sfxSource.pitch = Random.Range(0.9f, 1.2f);//random pitch shift for variety
                    sfxSource.pitch = 0.8f + (i * 0.05f);//rising pitch effect
                    sfxSource.PlayOneShot(vfxSoundEffect, 0.5f);
                }
            });
        }

        float totalDuration = _kernelsToBurst * 0.05f;
        DOVirtual.DelayedCall(totalDuration + 0.1f, () => sfxSource.pitch = 1f);//reset pitch back to normal after the loop finishes

        UpdateCurrencyUI();
    }
    private void SpawnFlyingKernel(Vector3 worldPos)
    {
        GameObject burstKernel = VFXPooler.Instance.SpawnFromPool(animatedKernelPrefab, Vector3.zero, Quaternion.identity, 2f);

        burstKernel.transform.SetParent(kernelUITarget.parent, false);//set as child of object under canvas so its visible

        burstKernel.GetComponent<KernelUIAnimation>().Play(worldPos, kernelUITarget.position, () =>
        {
            _kernelsCollected++;
            UpdateCurrencyUI();

            //ui punch
            //kernelUITarget.DOPunchScale(new Vector3(0.2f, 0.2f, 0.2f), 0.2f);
        });
    }

    public void FinalizeRun()
    {
        _isRunning = false;

        //this is formula: Total = Kernels + (Normal Eggs * 5) + (Golden Eggs * 15)
        int eggBonus = (_eggsCollected * kernelsPerEgg) + (_goldenEggsCollected * kernelsPerGoldenEgg);
        int totalEarnedThisRun = _kernelsCollected + eggBonus;

        CurrencyManager.Instance.UIUpdate_TotalKernels(totalEarnedThisRun);

        if (summaryUI != null)
        {
            summaryUI.ShowSummary(_totalMiles, _kernelsCollected, eggBonus, totalEarnedThisRun);
        }

        GameManager.Instance.StopGame();
        tapInput.SetActive(false);
        swipeInput.SetActive(false);
        abilityInput.SetActive(false);
    }
    #endregion

    #region UI Updates
    private void UpdateMilesUI()
    {
        if (milesText) milesText.text = $"{_totalMiles}m";
    }
    private void UpdateCurrencyUI()
    {
        CurrencyManager.Instance.UIUpdate_TotalKernels();

        if (kernelText) kernelText.SetText(_kernelsCollected.ToString());
        if (eggsText) eggsText.SetText($"{_eggsCollected}          ({_goldenEggsCollected})");//shows "egg(G eggs)" e.g., 5(1)
    }
    #endregion

    public void ResumeRun()
    {
        _isRunning = true;
        _bossTriggered = false;
        _currentBossDistance = 0f;
    }
    public void ResetBossProgress()
    {
        SetupNextSegment();
        _currentBossDistance = 0f;
        _bossTriggered = false;
        onBossDefeated?.Invoke();

        for (int i = 0; i < tickImages.Count; i++)
        {
            tickImages[i].color = inactiveColor;
        }
    }
}
