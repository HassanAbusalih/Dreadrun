using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemAssetHolder : MonoBehaviour, ICollectable
{
    public ItemBase item;
    public Player player;

 
    public ItemBase Collect()
    {
        return item;
    }
}

