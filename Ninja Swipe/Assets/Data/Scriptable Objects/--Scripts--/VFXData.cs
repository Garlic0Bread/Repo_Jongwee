using UnityEngine;

[CreateAssetMenu(fileName = "NewVFXData", menuName = "NinjaSwipe/VFXData")]
public class VFXData : ScriptableObject
{
    public VFXType type;
    public AudioClip vfxSound;
    public GameObject vfxPrefab;
}
