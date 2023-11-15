using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class UnlockedPerks : MonoBehaviour
{
    public List<Perk> unlockedPerkpool;
    [SerializeField]
    void Start()
    {
        LoadPerks();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.U))
        {
            SavePerks();
        }
    }

    public void AddNewPerk(Perk newPerk)
    {
        unlockedPerkpool.Add(newPerk);
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