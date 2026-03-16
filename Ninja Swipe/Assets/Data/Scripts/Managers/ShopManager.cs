using UnityEngine;

public class ShopManager : MonoBehaviour
{
    public static ShopManager Instance;

    private void Awake()
    {
        Instance = this;
    }

    public bool IsUnlocked(ShopItemData itemData)
    {
        return PlayerPrefs.GetInt(itemData.itemID, 0) == 1;
    }

    public void UnlockItem(ShopItemData itemData)
    {
        PlayerPrefs.SetInt(itemData.itemID, 1);
        PlayerPrefs.Save();
    }
    public bool TryPurchasing(ShopItemData itemData)
    {
        if(CurrencyManager.Instance._kernels < itemData.price)
            return false;

        CurrencyManager.Instance.SpendKernels(itemData.price);
        UnlockItem(itemData);
        return true;
    }
}
