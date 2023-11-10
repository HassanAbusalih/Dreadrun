using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class PerkSelector : MonoBehaviour
{
    public PlayerStats playerStats;
    public Player player;
    public GameObject perkDescriptionPanel;
    public TextMeshProUGUI descriptionText;

    [SerializeField]
    private List<Perk> perkPool; //assigned in inspector

    public GameObject perkUIcanvas;

    [SerializeField]
    private PerkOptions[] perkChoices;
    [SerializeField] bool debugMode;

    private PerkCollectorManager perkCollectorManager;

    void Start()
    {
        if(perkUIcanvas != null)
        {
            perkUIcanvas.SetActive(false);
        }
        //playerStats = player.playerStats;
        perkCollectorManager = FindObjectOfType<PerkCollectorManager>();
        player = FindObjectOfType<Player>();
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
        List<int> selectedIndexes = new List<int>();

        for (int perkIndex = 0; perkIndex < perkPool.Count; perkIndex++)
        {
            if (!perkCollectorManager.AcquireablePerk(perkPool[perkIndex])) //if its not an aquirable perk, just remove it from the pool
            {
                perkPool.RemoveAt(perkIndex);
                perkIndex--;
            }
        }                     

        for (int perkIndex = 0; perkIndex < perkChoices.Length; perkIndex++) //generate a random index depending on the count, use the index number to select a perk in the perkpool
        {
            int randomIndexNum;
            do
            {
                randomIndexNum = UnityEngine.Random.Range(0, perkPool.Count);
            } while (selectedIndexes.Contains(randomIndexNum));

            selectedIndexes.Add(randomIndexNum);
            perkChoices[perkIndex].perk = perkPool[randomIndexNum];
        }

        DisplayPerkDetails();
    }

    public void DisplayPerkDetails()
    {
        foreach (var choice in perkChoices)
        {
            choice.perkName.text = choice.perk.name;
            choice.perkSprite.sprite = choice.perk.icon;
            Debug.Log("Selected Perk: " + choice.perk.name);
        }
    }

    public void OnClick(int perkIndexSelected)
    {
        perkChoices[perkIndexSelected].perk.ApplyPlayerBuffs(player);

        perkUIcanvas.SetActive(false);
        perkCollectorManager.AcquirePerk(perkChoices[perkIndexSelected].perk, ref perkPool);
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
}
