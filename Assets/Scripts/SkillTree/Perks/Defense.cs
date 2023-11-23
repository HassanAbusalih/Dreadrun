using UnityEngine;

[CreateAssetMenu(menuName = "PlayerPerks/Defense")]
public class Defense : Perk
{
    [SerializeField] float defenseIncrement = 5f;

    public override INeedUI ApplyPlayerBuffs(Player player)
    {
        player.playerStats.defense += defenseIncrement;
        return null;
    }
}