public class Perk : Collectable
{
    public bool unique;
    public bool needsUI;

    public virtual INeedUI ApplyPlayerBuffs(Player player)
    {
       return null;
    }

    public virtual float FetchCooldown()
    {
        return float.MinValue;
    }

}