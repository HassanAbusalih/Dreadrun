using UnityEngine;
using TMPro;
using UnityEngine.UI;

[System.Serializable]
public class PerkOptions 
{
    [HideInInspector] public Perk perk;
    public Button perkButton;
    public TextMeshProUGUI perkName;
    public Image perkSprite;
    public Image perkRarity;
}