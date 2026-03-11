using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerVFXProfile", menuName = "NinjaSwipe/PlayerVFXProfile")]
public class PlayerVFXProfile : ScriptableObject
{
    public List<VFXData> selectedVFX;

    public GameObject GetVFX(VFXType type)
    {
        foreach (var vfx_Data in selectedVFX)
        {
            if (vfx_Data.type == type)
                return vfx_Data.vfxPrefab;
        }
        return null;
    }

    public AudioClip Get_SFX(VFXType type)
    {
        foreach (var vfx_Data in selectedVFX)
        {
            if (vfx_Data.type == type)
                return vfx_Data.vfxSound;
        }
        return null;
    }
}