using UnityEngine;

[CreateAssetMenu(menuName = "Shop/Skin")]
public class SkinData : ShopItemData
{
    public string skinID;

    [Header("Sprites")]
    public Sprite fallingSprite;
    public Sprite slashSprite;
    public Sprite flapSprite;
}
