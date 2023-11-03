
using UnityEngine;

[CreateAssetMenu(menuName = "PlayerPerks/MoreBulletSpread")]
public class MoreBulletSpread : Perk
{
    [SerializeField]  float bulletSpreadScaling =5;
    public override void ApplyPlayerBuffs(Player player)
    {
        float increaseAmount = player.playerStats.SpreadMultiplier * (bulletSpreadScaling/ 100f);
        player.playerStats.SpreadMultiplier+= increaseAmount;
        player.ScaleWeapon();
    }

}
