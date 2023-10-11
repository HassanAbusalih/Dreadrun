using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

    [System.Serializable]
    public class PerkOptions 
{
    public Button perkButton;
    public TextMeshProUGUI perkName;

    [HideInInspector]
    public Perk perk;
}
