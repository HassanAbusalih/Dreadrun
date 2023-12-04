using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PerkSelector : MonoBehaviour
{
    public PlayerStats playerStats;
    public GameObject perkDescriptionPanel;
    public TextMeshProUGUI descriptionText;

    [SerializeField]
    private List<Perk> perkPool; //assigned in inspector

    public GameObject perkUIcanvas;

    [SerializeField]
    private PerkOptions[] perkChoices;
    [SerializeField] bool debugMode;

    private PerkCollectorManager perkCollectorManager;

    public static Action OnPerkSelection;
    public Action<Perk> UpdateEquippedPerkUi;

    [SerializeField] [Range(0f, 1f)] private float commonChance = 0.5f;
    [SerializeField] [Range(0f, 1f)] private float rareChance = 0.3f;
    [SerializeField] [Range(0f, 1f)] private float legendaryChance = 0.1f;

    void Start()
    {
        if(perkUIcanvas != null)
        {
            perkUIcanvas.SetActive(false);
        }
        perkCollectorManager = FindObjectOfType<PerkCollectorManager>();
    }

    private void Update()
    {
        if (perkDescriptionPanel != null && perkDescriptionPanel.activeSelf)
        {
            perkDescriptionPanel.transform.position = Input.mousePosition;
        }

        if(debugMode)
        {
            if (Input.GetKeyDown(KeyCode.P))
            {
                RandomPerkSelector();
            }
        }
    }

    public void RandomPerkSelector()
    {
        perkUIcanvas.SetActive(true);
        perkDescriptionPanel.SetActive(false);
        List<Perk> selectedPerks = new List<Perk>();

        for (int perkIndex = 0; perkIndex < perkPool.Count; perkIndex++)
        {
            if (!perkCollectorManager.AcquireablePerk(perkPool[perkIndex])) //if its not an aquirable perk, just remove it from the pool
            {
                perkPool.RemoveAt(perkIndex);
                perkIndex--;
            }
        }

        List<Perk> common = new List<Perk>();
        List<Perk> rare = new List<Perk>();
        List<Perk> legendary = new List<Perk>();

        for (int perkIndex = 0; perkIndex < perkChoices.Length; perkIndex++) //generate a random index depending on the count, use the index number to select a perk in the perkpool
        {
            float randomValue = UnityEngine.Random.value;
            foreach (var perk in perkPool)
            {
                switch (perk.perkRarity)
                {
                    case PerkRarity.Common:
                        common.Add(perk);
                        break;
                    case PerkRarity.Rare:
                        rare.Add(perk);
                        break;
                    case PerkRarity.Legendary:
                        legendary.Add(perk);
                        break;
                }
            }

            List<Perk> selectedRarity = new();
            if(randomValue < legendaryChance && legendary.Count > 0)
            {
                selectedRarity = legendary;
                Debug.Log("LEGENDARY HAMPER");
            }
            else if(randomValue < rareChance && rare.Count > 0)
            {
                selectedRarity = rare;
                Debug.Log("RARE HAMPER");
            }
            else
            {
                selectedRarity = common;
                Debug.Log("COMMON HAMPER");
            }

            Perk randomPerk;
            do
            {
                int randomIndexNum;
                randomIndexNum = UnityEngine.Random.Range(0, selectedRarity.Count);
                randomPerk = selectedRarity[randomIndexNum];
            } while (selectedPerks.Contains(randomPerk));

            selectedPerks.Add(randomPerk);
            perkChoices[perkIndex].perk = randomPerk;
        }

        DisplayPerkDetails();
    }

    public void DisplayPerkDetails()
    {
        foreach (var choice in perkChoices)
        {
            choice.perkName.text = choice.perk.name;
            choice.perkSprite.sprite = choice.perk.icon;
            //Debug.Log("Selected Perk: " + choice.perk.name);
        }
    }

    public void OnClick(int perkIndexSelected)
    {
        perkUIcanvas.SetActive(false);
        perkCollectorManager.AcquirePerk(perkChoices[perkIndexSelected].perk);
        OnPerkSelection?.Invoke();
        UpdateEquippedPerkUi?.Invoke(perkChoices[perkIndexSelected].perk);
    }

    public void ShowDescription(int index)
    {
        if (index >= 0)
        {
            descriptionText.text = perkChoices[index].perk.description;
            perkDescriptionPanel.SetActive(true);
        }
    }

    public void HideDescription()
    {
        perkDescriptionPanel.SetActive(false);
    }

    public void AddToPool(Perk newPerk)
    {
        perkPool.Add(newPerk);
        //Debug.Log("added");
    }
}