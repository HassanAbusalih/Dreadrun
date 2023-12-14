using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class PerkUI : MonoBehaviour
{
    Perk perk;
    PerkCollectorManager perkCollector;
    [SerializeField] Image iconUI;
    [SerializeField] TextMeshProUGUI perkText;
    GameObject perkDescription;
    TextMeshProUGUI perkDescriptionText;
    int amountAcquired = 1;

    private void Update()
    {
        if (perkDescription != null && perkDescription.activeSelf)
        {
            perkDescription.transform.position = Input.mousePosition;
        }
    }

    public void SetPerk(Perk receivedPerk, PerkCollectorManager perkCollector, GameObject perkDescription, TextMeshProUGUI perkDescriptionText)
    {
        perk = receivedPerk;
        iconUI.sprite = receivedPerk.icon;
        perkText.text = amountAcquired.ToString();
        this.perkCollector = perkCollector;
        this.perkDescription = perkDescription;
        this.perkDescriptionText = perkDescriptionText;
        if (this.perkCollector != null) perkCollector.UpdateEquippedPerkUi += UpdatePerkUI;

        EventTrigger trigger = gameObject.AddComponent<EventTrigger>();
        EventTrigger.Entry entryPointerEnter = new EventTrigger.Entry();
        entryPointerEnter.eventID = EventTriggerType.PointerEnter;
        entryPointerEnter.callback.AddListener((data) => { OnPointerEnter((PointerEventData)data); });
        trigger.triggers.Add(entryPointerEnter);

        EventTrigger.Entry entryPointerExit = new EventTrigger.Entry();
        entryPointerExit.eventID = EventTriggerType.PointerExit;
        entryPointerExit.callback.AddListener((data) => { OnPointerExit((PointerEventData)data); });
        trigger.triggers.Add(entryPointerExit);
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

    public void OnPointerEnter(PointerEventData data)
    {
        perkDescription.SetActive(true);
        perkDescriptionText.text = perk.description;
    }

    public void OnPointerExit(PointerEventData data)
    {
        perkDescription.SetActive(false);
    }
}
