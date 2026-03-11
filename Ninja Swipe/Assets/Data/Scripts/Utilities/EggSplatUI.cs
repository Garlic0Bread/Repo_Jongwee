using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class EggSplatUI : MonoBehaviour
{
    private Coroutine splatRoutine;
    public static EggSplatUI Instance;
    [SerializeField] private Image splatImage;
    [SerializeField] private float duration = 0.75f;
    [SerializeField] private float fadeTime = 0.25f;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        splatImage.gameObject.SetActive(false);
    }

    public void ShowSplat()
    {
        if(splatRoutine != null)
        {
            StopCoroutine(splatRoutine);
        }

        splatRoutine = StartCoroutine(SplatRoutine());
    }

    IEnumerator SplatRoutine()
    {
        splatImage.color = Color.white;
        splatImage.gameObject.SetActive(true);
        yield return new WaitForSeconds(duration);

        float t = 0f;
        while(t < fadeTime)
        {
            t += Time.deltaTime;
            splatImage.color = new Color(1,1,1,1 -(t / fadeTime));
            yield return null;
        }
    }
}
