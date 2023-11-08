using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemAssetHolder : MonoBehaviour, ICollectable
{
    public ItemBase item;
    public Collectable Collect()
    {
        return item;

    }
}

