using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AllWeaponRarities", menuName = "ItemRarities/AllWeaponRarities")]
public class AllWeaponRarities : ScriptableObject
{
    public List<ItemRarityListClass> AllWeaponTypes;
}
