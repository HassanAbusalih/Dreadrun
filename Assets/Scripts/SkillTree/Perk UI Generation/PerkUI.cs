using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;

public class PerkUI : MonoBehaviour
{
    Perk perk;
    PerkSelector perkSelector;
    [SerializeField] Image iconUI;
    [SerializeField] TextMeshProUGUI perkText;
    int amountAcquired = 1;

    public void SetPerk(Perk receivedPerk,PerkSelector perkSelector)
    {
        perk = receivedPerk;
        iconUI.sprite = receivedPerk.icon;
        perkText.text = amountAcquired.ToString();
        this.perkSelector = perkSelector;
        if (this.perkSelector != null) perkSelector.UpdateEquippedPerkUi += UpdatePerkUI;
    }

    void UpdatePerkUI(Perk receivedPerk, Color borderColor)
    {
        if (perk == receivedPerk)
        amountAcquired++;
        perkText.text = amountAcquired.ToString();
    }

    private void OnDisable()
    {
        if (perkSelector != null) perkSelector.UpdateEquippedPerkUi -= UpdatePerkUI;
    }
}
