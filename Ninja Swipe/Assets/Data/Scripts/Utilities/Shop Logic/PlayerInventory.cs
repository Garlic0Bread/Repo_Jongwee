using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    private HashSet<string> ownedItems = new();
    public static PlayerInventory Instance;

    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        Load();
    }

    public bool IsOwned(string id) => ownedItems.Contains(id);
    public void AddItem(string id)
    {
        ownedItems.Add(id);
        Save();
    }

    void Save()
    {
        PlayerPrefs.SetString("OWNED_ITEMS", string.Join(",", ownedItems));
    }
    void Load()
    {
        string data = PlayerPrefs.GetString("OWNED_ITEMS", "");

        if (!string.IsNullOrEmpty(data))
        {
            foreach (var id in data.Split(','))
                ownedItems.Add(id);
        }

        EnsureDeafult_IsOwned();
    }

    private void EnsureDeafult_IsOwned()
    {
        foreach (var item in SkinManager.Instance.allItems)
        {
            if (item.isDefault)
                ownedItems.Add(item.itemID);
        }
    }
}
