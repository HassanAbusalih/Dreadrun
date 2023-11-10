using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemBase : Collectable
{
    public bool throwable;
    public bool hasBuffedItem = false;
    public int itemID;
    public SoundSO itemSound;

    public virtual void UseOnSelf(Player player)
    {

    }

  

}
