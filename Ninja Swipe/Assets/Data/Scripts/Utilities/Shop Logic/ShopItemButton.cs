using TMPro;
using UnityEngine;

public class ShopItemButton : MonoBehaviour
{
    private int itemPrice;

    [SerializeField] private ShopItemData item;
    [SerializeField] private GameObject priceObj;
    [SerializeField] private TextMeshProUGUI btnText;
    [SerializeField] private TextMeshProUGUI priceText;

    private void Awake()
    {
        itemPrice = item.price;

        if (PlayerInventory.Instance.IsOwned(item.itemID))
        {
            priceObj.SetActive(false);
            btnText.SetText("Equip");
        }

        else
        {
            priceObj.SetActive(true);
            btnText.SetText("Buy");
            priceText.SetText($"{itemPrice}");
        }
    }

    public void OnItemClicked(ShopItemData item)
    {
        if (PlayerInventory.Instance.IsOwned(item.itemID))
        {
            ShopManager.Instance.EquipItem(item);
        }

        else
        {
            ShopManager.Instance.BuyItem(item);

            priceObj.SetActive(false);
            btnText.SetText("Equip");
        }
    }
}
