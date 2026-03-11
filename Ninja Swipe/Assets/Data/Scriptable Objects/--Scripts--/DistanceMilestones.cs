using UnityEngine;

[CreateAssetMenu(menuName = "Run/Distance Milestone")]

public class DistanceMilestones : ScriptableObject
{
    [Range(0f, 1f)]
    public float[] milestones;
    public float distanceToBoss;
}
