using UnityEngine;

[CreateAssetMenu(menuName = "PlayerPerks/Explosive Projectiles")]
public class ExplosiveProjectiles : Perk
{
    [SerializeField] float explosionRadius = 5;
    [SerializeField] float explosionForce = 700;
    [SerializeField] LayerMask layersToIgnore;
    public override void ApplyPlayerBuffs(Player player)
    {
        player.gameObject.AddComponent<ExplosiveEffect>().Setup(explosionRadius, explosionForce, layersToIgnore);
        if (player.playerWeapon != null)
        {
            player.playerWeapon.UpdateWeaponEffects();
        }
    }
}