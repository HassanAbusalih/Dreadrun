using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Inventory 
{
    public int inventorySlots = 5;
    public ItemBase[] inventoryList;
}
