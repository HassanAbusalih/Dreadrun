using UnityEngine;

[CreateAssetMenu(menuName = "PlayerPerks/Berserk")]
public class BerserkPerk : Perk
{
    [SerializeField] float cooldown = 15f;
    [SerializeField] float duration = 5f;
    [SerializeField] float percentDamageIncrease = 20f;
    [SerializeField] float percentDefenseIncrease = 50f;
    [SerializeField] float percentSpeedIncrease = 20f;
    [SerializeField] float percentHealthDrain = 30f;
    [SerializeField] GameObject berserkEffect;

    public override INeedUI ApplyPlayerBuffs(Player player)
    {
        Berserk berserk = player.gameObject.AddComponent<Berserk>();
        berserk.Initialize(player, duration, percentDamageIncrease, percentDefenseIncrease, percentSpeedIncrease, percentHealthDrain, cooldown, berserkEffect);
        return berserk;
    }

    public override float FetchCooldown()
    {
        return cooldown;
    }
}