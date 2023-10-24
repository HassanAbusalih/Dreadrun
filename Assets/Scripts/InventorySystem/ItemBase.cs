using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemBase : ScriptableObject
{
    public new string name;
    public Sprite icon;
    public bool throwable;
    public string description;
    public bool hasBuffedItem = false;
    public float timer;
    public int itemID;


    public virtual void UseOnSelf(Player player)
    {

    }

}
