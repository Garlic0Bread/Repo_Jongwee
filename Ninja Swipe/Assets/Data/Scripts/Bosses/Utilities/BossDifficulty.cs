using UnityEngine;

public class BossDifficulty : MonoBehaviour
{
    private IBossScalable scalableBoss;

    private void Awake()
    {
        scalableBoss = GetComponent<IBossScalable>();
    }

    public void ApplyDifficulty(int level)
    {
        if (scalableBoss != null)
        {
            scalableBoss.ApplyDifficulty(level);
        }
    }
}
