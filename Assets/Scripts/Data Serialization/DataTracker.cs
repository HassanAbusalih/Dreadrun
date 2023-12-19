using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class DataTracker : MonoBehaviour
{
    public ExperienceManager experienceManager;
    public PerkCollectorManager perkCollectorManager;
    public Player player;
    public Collectable collectable;

    public RandomDungeonCreator randomDungeonCreator;
    public Payload payload;
    public PayloadTimer payloadTimer;

    List<PlayerData> playerDataList = new List<PlayerData>();
    List<string> instanceIDstring = new List<string>();

    public void Start()
    {
        collectable = FindObjectOfType<Collectable>();
    }
    public void SaveToJson()
    {


        PlayerData data = new PlayerData();
        data.totalHealth = player.playerStats.maxHealth;
        data.maximumLevelReached = experienceManager.currentLevel;

        data.roomsSpawned = randomDungeonCreator.totalRoomsSpawned;
        data.damageToPayload = 1000 - payload.health;
        data.totalTimePayload = payloadTimer.elapsedTime;
        data.perks = perkCollectorManager.acquiredPerks;

        foreach(var item in perkCollectorManager.acquiredPerks)
        {
            string IDstring = item.ToString();
            instanceIDstring.Add(IDstring);
        }

        string IDname = string.Join(",", instanceIDstring);
        data.PerkName = IDname;
        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(Application.dataPath + "/PlayerDataFile.json", json);
    }

}