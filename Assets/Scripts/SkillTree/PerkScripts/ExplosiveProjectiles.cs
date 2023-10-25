using UnityEngine;

[CreateAssetMenu(menuName = "PlayerPerks/Explosive Projectiles")]
public class ExplosiveProjectiles : Perk
{
    public override void ApplyPlayerBuffs(Player player)
    {
        player.gameObject.AddComponent<ExplosiveEffect>();
        if (player.playerWeapon != null)
        {
            player.playerWeapon.UpdateWeaponEffects();
        }
    }
}