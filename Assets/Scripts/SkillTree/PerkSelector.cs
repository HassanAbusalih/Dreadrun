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

    void Start()
    {
        perkUIcanvas.SetActive(false);
        //playerStats = player.playerStats;
        player = FindObjectOfType<Player>();
    }

    private void Update()
    {
        if (perkDescriptionPanel.activeSelf)
        {
            perkDescriptionPanel.transform.position = Input.mousePosition;
        }

    }
    public void RandomPerkSelector()
    {
        perkUIcanvas.SetActive(true);
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
            choice.perkName.text = choice.perk.perkName;
            choice.perkSprite.sprite = choice.perk.perkIcon;
            Debug.Log("Selected Perk: " + choice.perk.perkName);
        }
    }

    public void OnClick(int perkIndexSelected)
    {
        perkChoices[perkIndexSelected].perk.ApplyPlayerBuffs(player);
        perkUIcanvas.SetActive(false);
    }

    public void ShowDescription(int index)
    {
        if (index >= 0)
        {
            descriptionText.text = perkChoices[index].perk.perkDescription;
            perkDescriptionPanel.SetActive(true);
        }
    }

    public void HideDescription()
    {
        perkDescriptionPanel.SetActive(false);
    }
}

