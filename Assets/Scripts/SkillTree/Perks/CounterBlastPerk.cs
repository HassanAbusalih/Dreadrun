using UnityEngine;

[CreateAssetMenu(menuName = "PlayerPerks/CounterBlast")]
public class CounterBlastPerk : Perk
{
    [SerializeField] float explosionRadius = 4f;
    [SerializeField] float explosionForce = 4000f;
    [SerializeField] LayerMask layersToIgnore;
    [SerializeField] GameObject vfx;
    [SerializeField] float cooldown = 5f;
    public override INeedUI ApplyPlayerBuffs(Player player)
    {
        CounterBlast cbat = player.gameObject.AddComponent<CounterBlast>();
        cbat.SetCounterBlast(explosionRadius, explosionForce, layersToIgnore, vfx, cooldown);
        return cbat;
    }

    public override float FetchCooldown()
    {
        return cooldown;
    }
}
