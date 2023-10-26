using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    [SerializeField]
    public Player player;
    public Inventory inventory;
    public Image[] InventorySprites;
    KeyCode[] keys = { KeyCode.Alpha1, KeyCode.Alpha2, KeyCode.Alpha3, KeyCode.Alpha4, KeyCode.Alpha5 };

    void Start()
    {
        player = GetComponent<Player>();
        inventory = new Inventory();
        inventory.inventoryList = new List<ItemBase>(new ItemBase[inventory.inventorySlots]);
    }

    private void Update()
    {
        for (int i = 0; i < keys.Length; i++)
        {
            if (Input.GetKeyDown(keys[i]))
            {
                UseItem(i);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        ICollectable collectable = other.GetComponent<ICollectable>();

        if (collectable != null)
        {
            AddItem(collectable.Collect());
            Destroy(other.gameObject);
        }
    }

    public void AddItem(ItemBase item)
    {
        inventory.inventoryList.Add(item);
        AddToUI(item);
    }

    public void AddToUI(ItemBase item)
    {
        for (int slot = 0; slot < inventory.inventoryList.Count; slot++)
        {
            if (InventorySprites[slot].sprite != item.icon)
            {
                inventory.inventoryList[slot] = item;
                InventorySprites[slot].sprite = item.icon;
                break;
            }
        }
    }

    void UseItem(int slot)
    {
        if (slot >= 0 && slot < inventory.inventoryList.Count)
        {
            ItemBase item = inventory.inventoryList[slot];

            if (item != null)
            {
                item.UseOnSelf(player);
                inventory.inventoryList[slot] = null;
                UpdateInventoryUISlot(slot, null);
            }
        }

    }
    void UpdateInventoryUISlot(int slot, ItemBase item)
    {
        if (slot >= 0 && slot < InventorySprites.Length)
        {
            InventorySprites[slot].sprite = null;                                     
        }
    }
}
