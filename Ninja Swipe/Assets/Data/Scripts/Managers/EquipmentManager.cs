using UnityEngine;

public class EquipmentManager : MonoBehaviour
{
    public static EquipmentManager Instance;

    public string equippedSkin_ID;
    public string equippedEnvironment_ID;

    private void Awake()
    {
        Instance = this;
        Load();
    }

    public void Equip(ShopItemData itemData)
    {
        if(itemData.itemType == ShopItemType.Skin)
            equippedSkin_ID = itemData.itemID;

        if (itemData.itemType == ShopItemType.Environment)
            equippedEnvironment_ID = itemData.itemID;

        Save();
        SkinManager.Instance.ApplyAll();
    }
    private void EnsureDefaultsEquipped()
    {
        if (string.IsNullOrEmpty(equippedSkin_ID))
        {
            var defaultSkin = SkinManager.Instance.GetDefault(ShopItemType.Skin);
            equippedSkin_ID = defaultSkin.itemID;
        }

        if (string.IsNullOrEmpty(equippedEnvironment_ID))
        {
            var defaultEnv = SkinManager.Instance.GetDefault(ShopItemType.Environment);
            equippedEnvironment_ID = defaultEnv.itemID;
        }
        Save();
    }

    private void Save()
    {
        PlayerPrefs.SetString("EQUIPPED_SKIN", equippedSkin_ID);
        PlayerPrefs.SetString("EQUIPPED_ENVIRONMENT", equippedEnvironment_ID);
    }
    private void Load()
    {
        equippedSkin_ID = PlayerPrefs.GetString("EQUIPPED_SKIN", "");
        equippedEnvironment_ID = PlayerPrefs.GetString("EQUIPPED_ENVIRONMENT", "");

        EnsureDefaultsEquipped();
    }
}
