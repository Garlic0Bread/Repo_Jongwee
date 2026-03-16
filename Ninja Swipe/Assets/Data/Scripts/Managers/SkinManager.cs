using UnityEngine;
using System.Collections.Generic;

public class SkinManager : MonoBehaviour
{
    public static SkinManager Instance;

    [SerializeField] private List<SkinData> allSkins;

    private SkinData equippedSkin;

    private void Awake()
    {
        Instance = this;
        LoadSkin();
    }

    void LoadSkin()
    {
        string skinID = PlayerPrefs.GetString("EquippedSkin", "");

        foreach (var skin in allSkins)
        {
            if (skin.skinID == skinID)
            {
                equippedSkin = skin;
                return;
            }
        }

        if (allSkins.Count > 0)
            equippedSkin = allSkins[0];
    }

    public void EquipSkin(SkinData skin)
    {
        equippedSkin = skin;

        PlayerPrefs.SetString("EquippedSkin", skin.skinID);
        PlayerPrefs.Save();

        ApplyEquippedSkin(FindFirstObjectByType<Player_VisualController>());
    }

    public void ApplyEquippedSkin(Player_VisualController player)
    {
        if (equippedSkin != null && player != null)
        {
            player.ApplySkin(equippedSkin);
        }
    }
}
