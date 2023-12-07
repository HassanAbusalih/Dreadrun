using UnityEngine;

[CreateAssetMenu(menuName = "PlayerPerks/Scatter Projectiles")]
public class ScatterProjectiles : Perk
{
    [SerializeField] int projectileCount = 4;
    [SerializeField] float projectileSpeed = 5f;
    [SerializeField] float projectileRange = 5f;
    [SerializeField] float damagePercentage = 0.5f;
    [SerializeField] GameObject projectilePrefab;

    public override INeedUI ApplyPlayerBuffs(Player player)
    {
        player.gameObject.AddComponent<ScatterEffect>().Setup(projectilePrefab, projectileCount, projectileSpeed, projectileRange, damagePercentage);
        if (player.playerWeapon != null)
        {
            player.playerWeapon.UpdateWeaponEffects();
        }
        return null;
    }
}