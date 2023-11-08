using UnityEngine;

public enum ItemRarityTypes
{
    shit =-1,
    Common =0,
    Rare =5,
    Legendary =10
}

[System.Serializable]
public class ItemRarityClass 
{
    public ItemRarityTypes RarityType;
    public GameObject Item;  
}
