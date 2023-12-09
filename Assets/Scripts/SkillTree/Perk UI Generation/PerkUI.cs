using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PerkUI : MonoBehaviour
{
    Perk perk;
    PerkCollectorManager perkCollector;
    [SerializeField] Image iconUI;
    [SerializeField] TextMeshProUGUI perkText;
    int amountAcquired = 1;

    public void SetPerk(Perk receivedPerk, PerkCollectorManager perkCollector)
    {
        perk = receivedPerk;
        iconUI.sprite = receivedPerk.icon;
        perkText.text = amountAcquired.ToString();
        this.perkCollector = perkCollector;
        if (this.perkCollector != null) perkCollector.UpdateEquippedPerkUi += UpdatePerkUI;
    }

    void UpdatePerkUI(Perk receivedPerk, Color borderColor)
    {
        if (perk == receivedPerk)
        amountAcquired++;
        perkText.text = amountAcquired.ToString();
    }

    private void OnDisable()
    {
        if (perkCollector != null) perkCollector.UpdateEquippedPerkUi -= UpdatePerkUI;
    }
}
