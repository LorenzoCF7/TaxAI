using UnityEngine;
using System.Collections.Generic;

public class InventoryController : MonoBehaviour
{
    public static InventoryController Instance { get; private set; }
    
    private HashSet<Item> items = new HashSet<Item>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void AddItem(Item item)
    {
        if (item != null)
        {
            items.Add(item);
            Debug.Log($"Item añadido: {item.itemName}");
        }
    }

    public void RemoveItem(Item item)
    {
        if (item != null && items.Contains(item))
        {
            items.Remove(item);
            Debug.Log($"Item eliminado: {item.itemName}");
        }
    }

    public bool HasItem(Item item)
    {
        return item != null && items.Contains(item);
    }

    public void ClearInventory()
    {
        items.Clear();
    }

    public List<Item> GetAllItems()
    {
        return new List<Item>(items);
    }
}
