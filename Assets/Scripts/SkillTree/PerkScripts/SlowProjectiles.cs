using UnityEngine;

[CreateAssetMenu(menuName = "PlayerPerks/Slow Projectiles")]
public class SlowProjectiles : Perk
{
    [SerializeField][Range(0f, 1f)] float slowModifier = 0.5f;
    [SerializeField] float slowDuration = 5f;
    [SerializeField] GameObject slowVFX;
    public override void ApplyPlayerBuffs(Player player)
    {
        player.gameObject.AddComponent<SlowEffect>().Initialize(slowModifier, slowDuration, slowVFX);
        if (player.playerWeapon != null)
        {
            player.playerWeapon.UpdateWeaponEffects();
        }
    }
}