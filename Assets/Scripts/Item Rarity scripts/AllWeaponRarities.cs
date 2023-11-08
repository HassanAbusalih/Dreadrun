using UnityEngine;

[CreateAssetMenu(fileName = "AllWeaponRarities", menuName = "ItemRarities/AllWeaponRarities")]
public class AllWeaponRarities : ScriptableObject
{
    public ItemRarityListClass CommonWeapons;
    public ItemRarityListClass RareWeapons;
    public ItemRarityListClass LegendaryWeapons;
}
