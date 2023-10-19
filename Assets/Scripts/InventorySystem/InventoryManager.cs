using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    [SerializeField]
    public Player player;
    public Inventory inventory;

    // Start is called before the first frame update
    void Start()
    {
        player = GetComponent<Player>();
        inventory = new Inventory();
        inventory.inventoryList = new List<ItemBase>();
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
    }
}
