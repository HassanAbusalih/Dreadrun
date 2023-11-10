using UnityEngine;

[CreateAssetMenu(menuName = "PlayerPerks/CounterBlast")]
public class CounterBlastPerk : Perk
{
    [SerializeField] float explosionRadius = 4f;
    [SerializeField] float explosionForce = 4000f;
    [SerializeField] LayerMask layersToIgnore;
    [SerializeField] GameObject vfx;
    [SerializeField] float cooldown = 5f;
    public override void ApplyPlayerBuffs(Player player)
    {    
        player.gameObject.AddComponent<CounterBlast>().SetCounterBlast(explosionRadius, explosionForce, layersToIgnore, vfx, cooldown);
    }

    public override float FetchCooldown()
    {
        return cooldown;
    }
}
