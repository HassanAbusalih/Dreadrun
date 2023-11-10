using UnityEngine;

[CreateAssetMenu(menuName = "PlayerPerks/Poison Projectiles")]
public class PoisonProjectiles : Perk
{
    [SerializeField] GameObject vfx;
    public override void ApplyPlayerBuffs(Player player)
    {
        player.gameObject.AddComponent<PoisonEffect>().FetchVFX(vfx);
        if (player.playerWeapon != null)
        {
            player.playerWeapon.UpdateWeaponEffects();
        }
    }
}