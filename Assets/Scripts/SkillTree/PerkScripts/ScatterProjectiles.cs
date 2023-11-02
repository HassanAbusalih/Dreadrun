using UnityEngine;

[CreateAssetMenu(menuName = "PlayerPerks/Scatter Projectiles")]
public class ScatterProjectiles : Perk
{
    [SerializeField] int projectileCount = 4;
    [SerializeField] float projectileSpeed = 5f;
    [SerializeField] float projectileRange = 5f;
    [SerializeField] GameObject projectilePrefab;

    public override void ApplyPlayerBuffs(Player player)
    {
        player.gameObject.AddComponent<ScatterEffect>().Setup(projectilePrefab, projectileCount, projectileSpeed, projectileRange);
        if (player.playerWeapon != null)
        {
            player.playerWeapon.UpdateWeaponEffects();
        }
    }
}