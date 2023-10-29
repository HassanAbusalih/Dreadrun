
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.UI;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using static UnityEditor.Progress;
using TMPro;
using UnityEditor;

public class InventoryManager : MonoBehaviour
{
    public Player player;
    public Inventory inventory;
    public Image[] InventorySprites;
    public TextMeshProUGUI descriptionText;
    public Sprite emptySprite;
    public GameObject descriptionPanel;

    // Start is called before the first frame update
    void Start()
    {
        descriptionPanel.SetActive(false);
        player = GetComponent<Player>();
        inventory = new Inventory();
        inventory.inventoryList = new List<ItemBase>(new ItemBase[inventory.inventorySlots]);
    }
    #region Inputs
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            UseItem(0);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            UseItem(1);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            UseItem(2);
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            UseItem(3);
        }
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            UseItem(4);
        }
    }
    #endregion

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
        for (int slot = 0; slot < InventorySprites.Length; slot++)
        {    
            if (InventorySprites[slot].sprite == emptySprite)
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
            InventorySprites[slot].sprite = emptySprite;                                        
        }
    }

    public void ShowDescription(int index)
    {
        descriptionText.text = inventory.inventoryList[index].description; 
        descriptionPanel.SetActive(true);
    }

    public void HideDescription()
    {
        descriptionPanel.SetActive(false);
    }
}
