public class Perk : Collectable
{
    public bool unique;
    public PerkRarity perkRarity = PerkRarity.Common;

    public virtual INeedUI ApplyPlayerBuffs(Player player)
    {
       return null;
    }

    public virtual float FetchCooldown()
    {
        return float.MinValue;
    }
}

public enum PerkRarity
{
    Common,
    Rare,
    Legendary
}