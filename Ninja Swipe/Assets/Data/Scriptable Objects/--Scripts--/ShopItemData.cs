using UnityEngine;

public enum ShopItemType
{
    Skin,
    Environment
}

[CreateAssetMenu(menuName = "Shop/Item")]
public class ShopItemData : ScriptableObject
{
    public int price;
    public string itemID;
    public bool isDefault;
    public ShopItemType itemType;

    [Header("Visual")]
    public Sprite previewIcon;

    [Header("Skin")]
    public Sprite flapSprite;
    public Sprite slashSprite;
    public Sprite fallingSprite;

    [Header("Environment")]
    public GameObject environmentRoot;
}
