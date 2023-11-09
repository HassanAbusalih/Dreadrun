using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AllConsumableRarities", menuName = "ItemRarities/AllConsumableRarities")]
public class AllConsumableRarities : ScriptableObject
{
    public List<ItemRarityListClass> AllConsumableTypes;
}

