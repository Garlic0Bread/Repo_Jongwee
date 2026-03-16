using UnityEngine;

public enum ShopItemType
{
    Skin,
    Environment
}

public abstract class ShopItemData: ScriptableObject
{
    public int price;
    public string itemID;
    public string itemName;
    public ShopItemType itemType;
}
