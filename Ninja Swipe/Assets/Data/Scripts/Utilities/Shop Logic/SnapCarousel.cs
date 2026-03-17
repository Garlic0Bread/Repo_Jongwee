using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SnapCarousel : MonoBehaviour
{
    private int targetIndex = 0;
    private bool isDragging = false;

    public System.Action<int> OnItemSelected;

    [Header("References")]
    [SerializeField] private ScrollRect scrollRect;
    [SerializeField] private RectTransform content;
    [SerializeField] private List<RectTransform> items;

    [Header("Settings")]
    [SerializeField] private float snapSpeed = 10f;
    [SerializeField] private float minScale = 0.7f;
    [SerializeField] private float scaleMultiplier = 0.3f;


    void Update()
    {
        UpdateScaling();

        if (!isDragging)
        {
            SnapToTarget();
        }
    }

    public void OnBeginDrag()
    {
        isDragging = true;
    }
    public void OnEndDrag()
    {
        isDragging = false;
        targetIndex = GetClosestItemIndex();
        OnItemSelected?.Invoke(targetIndex);
    }

    void UpdateScaling()
    {
        for (int i = 0; i < items.Count; i++)
        {
            float distance = Mathf.Abs(
                content.InverseTransformPoint(items[i].position).x
            );

            float normalized = Mathf.Clamp01(distance / 300f);

            float scale = Mathf.Lerp(1f, minScale, normalized);
            items[i].localScale = Vector3.one * scale;
        }
    }

    int GetClosestItemIndex()
    {
        float closestDistance = float.MaxValue;
        int closestIndex = 0;

        for (int i = 0; i < items.Count; i++)
        {
            float distance = Mathf.Abs(
                content.InverseTransformPoint(items[i].position).x
            );

            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestIndex = i;
            }
        }

        return closestIndex;
    }

    void SnapToTarget()
    {
        Vector2 targetPos = GetTargetPosition(targetIndex);
        content.anchoredPosition = Vector2.Lerp(
            content.anchoredPosition,
            targetPos,
            Time.deltaTime * snapSpeed
        );
    }

    Vector2 GetTargetPosition(int index)
    {
        float contentWidth = content.rect.width;
        float viewportWidth = scrollRect.viewport.rect.width;

        float itemPos = -items[index].anchoredPosition.x;

        float centeredPos = itemPos - (viewportWidth / 2) + (items[index].rect.width / 2);

        return new Vector2(centeredPos, content.anchoredPosition.y);
    }
}
