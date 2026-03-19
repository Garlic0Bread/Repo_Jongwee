using TMPro;
using UnityEngine;

public class ShopManager : MonoBehaviour
{
    public static ShopManager Instance;

    [SerializeField] private int totalKernelsAvailable;
    [SerializeField] private TextMeshProUGUI totalKernelsAvailable_txt;

    private void Awake()
    {
        Instance = this;
        totalKernelsAvailable = PlayerPrefs.GetInt("TotalKernels", 0);

        Update_UI();
    }

    public void BuyItem(ShopItemData itemData)
    {
        if (PlayerInventory.Instance.IsOwned(itemData.itemID))
            return;

        if (CurrencyManager.Instance._kernels < itemData.price)
            return;

        CurrencyManager.Instance.SpendKernels(itemData.price);
        PlayerInventory.Instance.AddItem(itemData.itemID);

        Update_UI();
    }
    public void EquipItem(ShopItemData item)
    {
        if (!PlayerInventory.Instance.IsOwned(item.itemID))
            return;

        EquipmentManager.Instance.Equip(item);
    }

    void Update_UI()
    {
        totalKernelsAvailable = PlayerPrefs.GetInt("TotalKernels", 0);
        totalKernelsAvailable_txt.text = totalKernelsAvailable.ToString();
    }
}
