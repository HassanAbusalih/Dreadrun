using UnityEngine;

[CreateAssetMenu(menuName = "PlayerPerks/ShieldPerk")]
public class ShieldPerk : Perk
{
    [SerializeField] private GameObject shield;
    [SerializeField] private float shieldCD;
    public override void ApplyPlayerBuffs(Player player)
    {
        GameObject newShield = Instantiate(shield, player.transform.position, player.transform.rotation);
        ShieldManager shieldManager = player.gameObject.AddComponent<ShieldManager>();
        shieldManager.GimmieShield(newShield, shieldCD, player);
        //repush
    }
}
