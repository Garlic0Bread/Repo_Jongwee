using UnityEngine;

public enum TutorialTrigger
{
    ObstacleCleared,
    FlapInitiated,
    KernelCollected,
    EggCollected
}

[CreateAssetMenu(menuName = "Tutorial/Step")]
public class TutorialStepData : ScriptableObject
{
    public float spawnDelay = 1f;
    public TutorialTrigger trigger;
    public GameObject prefabToSpawn;
    [TextArea] public string tutorialText;

    [Header("Abilities Unlocked On Start")]
    public PlayerPermissions abilitiesUnlocked;
}
