using UnityEngine;

[CreateAssetMenu(menuName = "PlayerPerks/AttackSpeed")]
public class AttackSpeed : Perk
{
    [SerializeField]
    private float attackSpeedScaling = 20f;

    public override INeedUI ApplyPlayerBuffs(Player player)
    {
        if (player.playerWeapon != null)
        {
            player.playerWeapon.FireRate /= player.playerStats.attackSpeed;
        }
        float increaseAmount = player.playerStats.attackSpeed * (attackSpeedScaling / 100f);
        player.playerStats.attackSpeed += increaseAmount;

        if (player.playerWeapon != null)
        {
            player.playerWeapon.FireRate *= player.playerStats.attackSpeed;
        }
        return null;
    }
}