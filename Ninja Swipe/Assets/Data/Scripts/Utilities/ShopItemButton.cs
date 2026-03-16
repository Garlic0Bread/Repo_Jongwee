using TMPro;
using UnityEngine;

public class ShopItemButton : MonoBehaviour
{
    public ShopItemData item;
    [SerializeField] private TextMeshProUGUI itemPrice;

    private int _kernels;
    [SerializeField] private TextMeshProUGUI availableKernels;

    private void Awake()
    {
        itemPrice.text = item.price.ToString();

        _kernels = PlayerPrefs.GetInt("TotalKernels", 0);
        availableKernels.text = _kernels.ToString();
    }
    public void OnClick()
    {
        if (!ShopManager.Instance.IsUnlocked(item))
        {
            bool bought = ShopManager.Instance.TryPurchasing(item);

            if (!bought)
            {
                Debug.Log("Not enough coins");
                return;
            }
        }

        EquipItem();
    }

    void EquipItem()
    {
        if (item is SkinData skin)
        {
            SkinManager.Instance.EquipSkin(skin);
        }
        else if (item is EnvironmentData env)
        {
            EnvironmentManager.Instance.ActivateEnvironment(env);
        }
    }
}
