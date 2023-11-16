using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PerkLootboxes : MonoBehaviour
{
    [SerializeField] List<Perk> unlockablePerkPool;
    [SerializeField] UnlockedPerks unlocked;
    [SerializeField] bool opened;
    [SerializeField] bool entered;
    [Header("Perk UI")]
    [SerializeField] GameObject perkUIcanvas;
    [SerializeField] List<PerkOptions> perkChoices;
    void Start()
    {
        unlocked = FindObjectOfType<UnlockedPerks>();
        RemoveCommonPerks(unlockablePerkPool, unlocked.unlockedPerkpool);
        perkUIcanvas.SetActive(false);
    }

    void Update()
    {
        if (entered)
        {
            OpenLootBox();
        }
    }
    void RemoveCommonPerks(List<Perk> unlockable, List<Perk> unlocked)
    {
        unlockable.RemoveAll(poolA => unlocked.Contains(poolA));
        Debug.Log("deleted");
        UpdateOptions();
        if (unlockable.Count == 0)
        {
            Destroy(this.gameObject);
        }
    }

    void UpdateOptions()
    {
 
        while (unlockablePerkPool.Count < perkChoices.Count)
        {
            perkChoices.RemoveAt(perkChoices.Count - 1);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<UnlockedPerks>())
        {
            entered = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        entered = false;
    }

    void OpenLootBox()
    {
        if (Input.GetKeyDown(KeyCode.E) && !opened)
        {
            RandomPerkSelector();
            perkUIcanvas.SetActive(true);
            opened = true;
        }
    }

    public void RandomPerkSelector()
    {
        List<int> selectedIndexes = new List<int>();

        for (int perkIndex = 0; perkIndex < perkChoices.Count; perkIndex++)
        {
            int randomIndexNum;
            do
            {
                randomIndexNum = UnityEngine.Random.Range(0, unlockablePerkPool.Count);
            } while (selectedIndexes.Contains(randomIndexNum));

            selectedIndexes.Add(randomIndexNum);
            perkChoices[perkIndex].perk = unlockablePerkPool[randomIndexNum];
        }

        DisplayPerkDetails();
    }

    public void DisplayPerkDetails()
    {
        foreach (var choice in perkChoices)
        {
            choice.perkName.text = choice.perk.name;
            choice.perkSprite.sprite = choice.perk.icon;
           
        }
    }

    public void OnClick(int perkIndexSelected)
    {
        perkUIcanvas.SetActive(false);
        unlocked.AddNewPerk(perkChoices[perkIndexSelected].perk);
    }

}