public class Perk : Collectable
{
    public bool unique;

    public virtual INeedUI ApplyPlayerBuffs(Player player)
    {
       return null;
    }

    public virtual float FetchCooldown()
    {
        return float.MinValue;
    }
}