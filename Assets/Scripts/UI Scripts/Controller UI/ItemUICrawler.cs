using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemUICrawler : MonoBehaviour
{

    int last;
    int first;
    [SerializeField] int current;
    [SerializeField] KeyCode rightbumper;
    [SerializeField] KeyCode leftbumper;
    [SerializeField] KeyCode useItem;
    InventoryManager inventory;
    // Start is called before the first frame update
    void Start()
    {
        inventory = FindObjectOfType<InventoryManager>();
        last = 4;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(rightbumper))
        {
            if(last != current)
            {
                current++;
            }
            else
            {
                current = first;
            }
        }

        if (Input.GetKeyDown(leftbumper))
        {
            if (first != current)
            {
                current--;
            }
            else
            {
                current = last;
            }
        }

        if (Input.GetKeyDown(useItem))
        {
            inventory.UseItem(current);
            Debug.Log(current);
        }
    }
}