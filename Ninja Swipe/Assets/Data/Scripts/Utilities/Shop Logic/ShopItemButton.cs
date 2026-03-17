using UnityEngine;
using UnityEngine.UI;

public class ShopItemButton : MonoBehaviour
{
    public Image icon;
    public GameObject lockIcon;

    public void Setup(ShopItemData data)
    {
        icon.sprite = data.previewIcon;

        bool owned = ShopManager.IsOwned(data.itemID);
        lockIcon.SetActive(!owned);
    }
}
