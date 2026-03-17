using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ShopLaneController : MonoBehaviour
{
    private int currentIndex;

    [Header("UI")]
    [SerializeField] private TMP_Text buttonText;

    [SerializeField] private List<ShopItemData> items;
    [SerializeField] private SnapCarousel carousel;

    void Start()
    {
        carousel.OnItemSelected += SetIndex;
    }
    void UpdateUI()
    {
        var item = items[currentIndex];

        bool owned = ShopManager.IsOwned(item.itemID);
        bool equipped = IsEquipped(item);

        if (!owned)
            buttonText.text = $"Purchase ({item.price})";
        else if (!equipped)
            buttonText.text = "Equip";
        else
            buttonText.text = "Equipped";
    }

    public void OnButtonPressed()
    {
        var item = items[currentIndex];

        if (!ShopManager.IsOwned(item.itemID))
        {
            Buy(item);
        }
        else
        {
            Equip(item);
        }

        UpdateUI();
    }
    public void SetIndex(int index)
    {
        currentIndex = index;
        UpdateUI();
    }

    void Buy(ShopItemData item)
    {
        if (CurrencyManager.Instance._kernels < item.price) return;

        CurrencyManager.Instance.SpendKernels(item.price);
        ShopManager.SetOwned(item.itemID);
    }
    void Equip(ShopItemData item)
    {
        if (item.type == ShopItemType.Skin)
            SkinManager.Instance.EquipSkin(item);

        if (item.type == ShopItemType.Environment)
            EnvironmentManager.Instance.SetEnvironment(item.environmentRoot);
    }
    bool IsEquipped(ShopItemData item)
    {
        if (item.type == ShopItemType.Skin)
            return PlayerPrefs.GetString("EquippedSkin") == item.itemID;

        if (item.type == ShopItemType.Environment)
            return PlayerPrefs.GetString("EquippedEnvironment") == item.itemID;

        return false;
    }
}
