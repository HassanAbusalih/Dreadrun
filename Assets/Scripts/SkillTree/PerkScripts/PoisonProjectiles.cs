using UnityEngine;

[CreateAssetMenu(menuName = "PlayerPerks/Poison Projectiles")]
public class PoisonProjectiles : Perk
{
    public override void ApplyPlayerBuffs(Player player)
    {
        player.gameObject.AddComponent<PoisonEffect>();
        if (player.playerWeapon != null)
        {
            player.playerWeapon.UpdateWeaponEffects();
        }
    }
}