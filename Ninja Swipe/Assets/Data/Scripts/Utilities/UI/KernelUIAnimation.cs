using DG.Tweening;
using UnityEngine;

public class KernelUIAnimation : MonoBehaviour
{
    public void Play(Vector3 startWorldPos, Vector3 targetScreenPos, System.Action onComplete)
    {
        Vector3 screenPos = Camera.main.WorldToScreenPoint(startWorldPos);
        transform.position = screenPos;
        transform.localScale = Vector3.zero;

        Sequence seq = DOTween.Sequence();//animation sequence
        seq.Append(transform.DOScale(1.2f, 0.3f).SetEase(Ease.OutBack));// Pop out and scale up

        //random burst effect
        Vector3 randomBurst = new(Random.Range(-50f, 50f), Random.Range(-50f, 50f), 0);
        seq.Join(transform.DOMove(screenPos + randomBurst, 0.4f).SetEase(Ease.OutQuad));

        //Fly to UI Target
        seq.AppendInterval(0.1f); //pause briefly to look at the kernel
        seq.Append(transform.DOMove(targetScreenPos, 0.6f).SetEase(Ease.InBack));
        seq.Join(transform.DOScale(0.5f, 0.6f));

        seq.OnComplete(() =>
        {
            onComplete?.Invoke();
            gameObject.SetActive(false); //return kernel to pool
        });
    }
}
