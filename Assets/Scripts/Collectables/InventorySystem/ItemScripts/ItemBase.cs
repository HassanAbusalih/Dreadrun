using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemBase : Collectable
{
    public bool throwable;
    public bool hasBuffedItem = false;
    public int itemID;
    public SoundSO itemSound;
    [SerializeField] ParticleSystem ParticleSystem;

    public virtual void UseOnSelf(Player player)
    {
        
    }
    protected void PlayVfx(Player player, float duration)
    {
        ParticleSystem vfx = Instantiate(ParticleSystem, player.transform.position, Quaternion.identity);
        vfx.transform.SetParent(player.transform);
        Destroy(vfx.gameObject, duration);
    }

  

}
