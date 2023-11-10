using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerkCollectorManager : MonoBehaviour
{
    [SerializeField]
    List<Perk> acquiredPerks = new List<Perk>();
    private Player player;
    private PerkSelector perkSelector;
    private void Start()
    {
        player = GetComponent<Player>();
        perkSelector = FindObjectOfType<PerkSelector>();
    }

    public void AcquirePerk(Perk perk, ref List<Perk> perkPool)
    {
        acquiredPerks.Add(perk);
    }

    public bool AcquireablePerk(Perk perk)
    {
        return (perk.unique && !acquiredPerks.Contains(perk)) || !perk.unique;
    }

    public void AcquirePerk(Perk perk)
    {
        acquiredPerks.Add(perk);
    }
    private void OnTriggerEnter(Collider other)
    {
        ICollectable collectable = other.GetComponent<ICollectable>();
        if (collectable != null)
        {
            Perk perk = collectable.Collect() as Perk;
            if (perk != null && AcquireablePerk(perk)) 
            {
                AcquirePerk(perk);
                perk.ApplyPlayerBuffs(player);
                Destroy(other.gameObject);
            }
        }
    }
}