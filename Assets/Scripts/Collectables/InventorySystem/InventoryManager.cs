using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class InventoryManager : MonoBehaviour
{
    public Player player;
    public Inventory inventory;
    public Image[] InventorySprites;
    public GameObject descriptionPanel;
    public TextMeshProUGUI descriptionText;
    public Sprite emptySprite;
    KeyCode[] keys = { KeyCode.Alpha1, KeyCode.Alpha2, KeyCode.Alpha3, KeyCode.Alpha4, KeyCode.Alpha5 };
    [SerializeField] SoundSO pickUpItemSFX;
    AudioSource audioSource;
    private int slotCounter = 0;

    void Start()
    {
        if (descriptionPanel != null)
        {
            descriptionPanel.SetActive(false);
        }
        player = GetComponent<Player>();
        inventory = new Inventory();
        inventory.inventoryList = new ItemBase[inventory.inventorySlots];
    }
    private void Update()
    {
        if (descriptionPanel != null && descriptionPanel.activeSelf)
        {
            descriptionPanel.transform.position = Input.mousePosition;
        }

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

        if (collectable != null && slotCounter < inventory.inventorySlots)
        {
            ItemBase item = collectable.Collect() as ItemBase;
            if (item != null)
            {
                AddItem(item);
                if (pickUpItemSFX != null)
                {
                    pickUpItemSFX.PlaySound(0, AudioSourceType.Player);
                }
                Destroy(other.gameObject);
            }
        }

    }

    public void AddItem(ItemBase item)
    {
        slotCounter++;
        for (int i = 0; i < inventory.inventoryList.Length; i++)
        {
            if (inventory.inventoryList[i] == null)
            {
                inventory.inventoryList[i] = item;
                break;
            }
        }
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

    public void UseItem(int slot)
    {
        if (slot >= 0 && slot < inventory.inventoryList.Length)
        {
            ItemBase item = inventory.inventoryList[slot];

            if (item != null)
            {
                item.UseOnSelf(player);
                inventory.inventoryList[slot] = null;
                UpdateInventoryUISlot(slot, null);
                slotCounter--;
            }
            if(item.itemSound != null)
            {
                item.itemSound.PlaySound(0, AudioSourceType.Player);
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
        if (index >= 0 && InventorySprites[index].sprite != emptySprite)
        {
            descriptionText.text = inventory.inventoryList[index].description;
            descriptionPanel.SetActive(true);
        }
    }

    public void HideDescription()
    {
        descriptionPanel.SetActive(false);
    }
}
