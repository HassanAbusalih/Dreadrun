using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PerkSelector : MonoBehaviour
{
    public Player player;
    [SerializeField]
    private Perk[] perkPool; //assigned in inspector

    public GameObject perkUIcanvas;

    [SerializeField]
    private PerkOptions[] perkChoices;

    void Start()
    {
        perkUIcanvas.SetActive(false);
        player = FindObjectOfType<Player>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void RandomPerkSelector()
    {
        //Perk[] selectedPerks = new Perk[perkSelection];
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
}
