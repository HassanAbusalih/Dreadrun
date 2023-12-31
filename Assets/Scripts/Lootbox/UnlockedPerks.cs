using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class UnlockedPerks : MonoBehaviour
{
    public List<Perk> unlockedPerkpool;
    [SerializeField] PerkSelector selector;
    [SerializeField] bool debugMode;

    void Awake()
    {
        selector = FindObjectOfType<PerkSelector>();
        LoadPerks();
        AddUnlockedPerks();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.U) && debugMode)
        {
            SavePerks();
        }
    }

    public void AddNewPerk(Perk newPerk)
    {
        unlockedPerkpool.Add(newPerk);
    }

    void AddUnlockedPerks()
    {
        foreach (Perk unlockedPerk in unlockedPerkpool)
        {
            selector.AddToPool(unlockedPerk);
        }
    }

    public void SavePerks()
    {
        string jsonData = JsonUtility.ToJson(new PerkData(unlockedPerkpool));
        File.WriteAllText("perks.json", jsonData);
        Debug.Log("Perks saved to JSON.");
    }

    private void LoadPerks()
    {
        if (File.Exists("perks.json"))
        {
            string jsonData = File.ReadAllText("perks.json");
            PerkData perkData = JsonUtility.FromJson<PerkData>(jsonData);
            unlockedPerkpool = perkData.unlockedPerks;
            AddUnlockedPerks();
            Debug.Log("Perks read to JSON.");
        }
    }

    [System.Serializable]
    private class PerkData
    {
        public List<Perk> unlockedPerks;

        public PerkData(List<Perk> perks)
        {
            unlockedPerks = perks;
        }
    }
}