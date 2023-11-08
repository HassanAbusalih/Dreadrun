using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AllConsumableRarities", menuName = "ItemRarities/AllConsumableRarities")]
public class AllConsumableRarities : ScriptableObject
{
    public ItemRarityListClass CommonConsumables;
    public ItemRarityListClass RareConsumables;
    public ItemRarityListClass LegendaryConsumables;
}
