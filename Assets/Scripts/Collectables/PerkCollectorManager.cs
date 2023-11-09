using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerkCollectorManager : MonoBehaviour
{
    [SerializeField]
    List<Perk> perks = new List<Perk>();
    private Player player;
    private void Start()
    {
        player = GetComponent<Player>();
    }
    public void AcquirePerk(Perk perk)
    {
        perks.Add(perk);
    }
    private void OnTriggerEnter(Collider other)
    {
        ICollectable collectable = other.GetComponent<ICollectable>();

        if (collectable != null)
        {
            Perk perk = collectable.Collect() as Perk;
            if (perk != null)
            {
                AcquirePerk(perk);
                perk.ApplyPlayerBuffs(player);
                Destroy(other.gameObject);
            }
        }
    }
}