using UnityEngine;

public class SelfDelete : MonoBehaviour
{
    void Start()
    {
        Destroy(gameObject, 5);
    }

    private void OnDestroy()
    {
        TutorialManager.Instance.TriggerEvent(TutorialTrigger.FlapInitiated);
    }
}
