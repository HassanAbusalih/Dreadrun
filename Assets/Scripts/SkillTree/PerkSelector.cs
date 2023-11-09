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
    private Perk[] perkPool; //assigned in inspector

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

        for (int perkIndex = 0; perkIndex < perkChoices.Length; perkIndex++)
        {
            int randomIndexNum;
            do
            {
                randomIndexNum = UnityEngine.Random.Range(0, perkPool.Length);
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
        perkCollectorManager.AcquirePerk(perkChoices[perkIndexSelected].perk);
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

