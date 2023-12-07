using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

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
    public Action<Perk, Color> UpdateEquippedPerkUi;

    [SerializeField] [Range(0f, 1f)] private float rareChance = 0.3f;
    [SerializeField] [Range(0f, 1f)] private float legendaryChance = 0.1f;
    [SerializeField] Color rareColor;
    [SerializeField] Color legendaryColor;
    [SerializeField] float fadeDuration = 5f;

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

            List<Perk> selectedRarity = DecidePerkRarity(selectedPerks, common, rare, legendary, randomValue);
            Perk randomPerk;
            int numberOfTries = 0;
            do
            {
                numberOfTries++;
                int randomIndexNum;
                randomIndexNum = UnityEngine.Random.Range(0, selectedRarity.Count);
                randomPerk = selectedRarity[randomIndexNum];
            }
            while (selectedPerks.Contains(randomPerk) && numberOfTries < 50);

            selectedPerks.Add(randomPerk);
            perkChoices[perkIndex].perk = randomPerk;
        }

        DisplayPerkDetails();
    }

    private List<Perk> DecidePerkRarity(List<Perk> selectedPerks, List<Perk> common, List<Perk> rare, List<Perk> legendary, float randomValue)
    {
        List<Perk> selectedRarity = new();
        if (randomValue < legendaryChance && legendary.Count > 0)
        {
            selectedRarity = legendary;
        }
        else if (randomValue < rareChance && rare.Count > 0)
        {
            selectedRarity = rare;
        }
        else
        {
            selectedRarity = common;
        }
        for (int i = 0; i < selectedRarity.Count; i++)
        {
            if (selectedPerks.Contains(selectedRarity[i]))
            {
                selectedRarity.RemoveAt(i);
                i--;
            }
        }
        if (selectedRarity.Count == 0)
        {
            selectedRarity = common;
        }

        return selectedRarity;
    }

    public void DisplayPerkDetails()
    {
        foreach (var choice in perkChoices)
        {
            choice.perkName.text = choice.perk.name;
            choice.perkSprite.sprite = choice.perk.icon;
            choice.perkRarity.color = choice.perk.perkRarity == PerkRarity.Legendary ? legendaryColor : new(1, 1, 1, 0);
            if (choice.perk.perkRarity != PerkRarity.Common)
            {
                choice.perkRarity.gameObject.SetActive(true);
                StartCoroutine(LerpTransparency(choice.perkRarity, fadeDuration));
            }
            else
            {
                choice.perkRarity.gameObject.SetActive(false);
            }
        }
    }

    public void OnClick(int perkIndexSelected)
    {
        perkUIcanvas.SetActive(false);
        perkCollectorManager.AcquirePerk(perkChoices[perkIndexSelected].perk);
        OnPerkSelection?.Invoke();
        Color perkColor;
        switch (perkChoices[perkIndexSelected].perk.perkRarity)
        {
            case PerkRarity.Legendary:
                perkColor = legendaryColor;
                break;
            case PerkRarity.Rare:
                perkColor = rareColor;
                break;
            default:
                perkColor = Color.white;
                break;
        }
        UpdateEquippedPerkUi?.Invoke(perkChoices[perkIndexSelected].perk, perkColor);
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
    }

    IEnumerator LerpTransparency(Image image, float duration)
    {
        Animator animator = image.GetComponent<Animator>();
        if (animator != null)
        {
            animator.enabled = false;
        }
        float elapsedTime = 0;
        float startAlpha = image.color.a;
        image.color = new Color(image.color.r, image.color.g, image.color.b, 0);
        while (elapsedTime < duration)
        {
            image.color = new Color(image.color.r, image.color.g, image.color.b, Mathf.Lerp(0, startAlpha, elapsedTime / duration));
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        image.color = new Color(image.color.r, image.color.g, image.color.b, startAlpha);
        if (animator != null)
        {
            animator.enabled = true;
        }
    }
}