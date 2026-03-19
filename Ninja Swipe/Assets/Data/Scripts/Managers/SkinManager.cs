using UnityEngine;

public class SkinManager : MonoBehaviour
{
    public static SkinManager Instance;

    public ShopItemData[] allItems;

    private void Awake()
    {
        Instance = this;
    }

    public void ApplyAll()
    {
        var playerViz = FindFirstObjectByType<Player_VisualController>();
        if (playerViz != null)
            ApplyEquippedSkin(playerViz);

        Apply_Environment();
    }
    void Apply_Environment()
    {
        string id = EquipmentManager.Instance.equippedEnvironment_ID;

        foreach (var item in allItems)
        {
            if (item.itemID == id && item.environmentRoot != null)
            {
                Instantiate(item.environmentRoot);
                return;
            }
        }
    }
    public void ApplyEquippedSkin(Player_VisualController player)
    {
        var skin = GetEquippedSkin();
        if (skin != null)
            player.ApplySkin(skin);
    }

    ShopItemData GetEquippedSkin()
    {
        string id = EquipmentManager.Instance.equippedSkin_ID;

        foreach (var item in allItems)
        {
            if (item.itemID == id)
                return item;
        }
        return GetDefault(ShopItemType.Skin);
    }
    public ShopItemData GetDefault(ShopItemType type)
    {
        foreach (var item in allItems)
        {
            if (item.itemType == type && item.isDefault)
                return item;
        }

        Debug.LogError("No default set for " + type);
        return null;
    }
}
