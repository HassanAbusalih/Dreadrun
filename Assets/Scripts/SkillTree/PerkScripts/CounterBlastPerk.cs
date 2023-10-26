using UnityEngine;

[CreateAssetMenu(menuName = "PlayerPerks/CounterBlast")]
public class CounterBlastPerk : Perk
{
    [SerializeField]float explosionRadius = 4f;
    [SerializeField] float explosionForce = 4000f;
    public override void ApplyPlayerBuffs(Player player)
    {    
        player.gameObject.AddComponent<CounterBlast>().SetCounterBlast(explosionRadius, explosionForce);     
    }
}
