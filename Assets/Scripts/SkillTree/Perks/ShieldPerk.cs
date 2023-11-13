using UnityEngine;

[CreateAssetMenu(menuName = "PlayerPerks/ShieldPerk")]
public class ShieldPerk : Perk
{
    [SerializeField] private GameObject shield;
    [SerializeField] private float shieldCD;
    [SerializeField] float regenDelay = 1;
    [SerializeField] float regenDuration = 5;
    public override INeedUI ApplyPlayerBuffs(Player player)
    {
        GameObject newShield = Instantiate(shield, player.transform.position, player.transform.rotation);
        ShieldManager manager = player.gameObject.AddComponent<ShieldManager>();
        manager.GimmieShield(newShield, shieldCD, regenDelay, regenDuration, player);
        return manager;
    }

    public override float FetchCooldown()
    {
        return shieldCD;
    }
}