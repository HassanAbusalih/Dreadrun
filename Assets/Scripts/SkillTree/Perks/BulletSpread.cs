
using UnityEngine;

[CreateAssetMenu(menuName = "PlayerPerks/BulletSpread")]
public class BulletSpread : Perk
{
    [SerializeField]  float bulletSpreadScaling =0.1f;
    public override INeedUI ApplyPlayerBuffs(Player player)
    {
        if(player.playerWeapon!= null)
        {
            player.playerWeapon.SpreadAngle /= player.playerStats.Spread;
        }
     
        float increaseAmount = player.playerStats.Spread * (bulletSpreadScaling/ 100f);
        player.playerStats.Spread += increaseAmount;

        if (player.playerWeapon != null)
        {
            player.playerWeapon.SpreadAngle *= player.playerStats.Spread;
        }
        return null;
    }
}
