using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class DataTracker : MonoBehaviour
{
    public ExperienceManager experienceManager;
    public PerkCollectorManager perkCollectorManager;
    public Player player;

    public RandomDungeonCreator randomDungeonCreator;
    public Payload payload;
    public PayloadTimer payloadTimer;

    List<PlayerData> playerDataList = new List<PlayerData>();

    [SerializeField] int dataFileNumber; 

    public void SaveToJson()
    {


        PlayerData data = playerDataList[playerDataList.Count + 1];
        data.totalHealth = player.playerStats.maxHealth;
        data.maximumLevelReached = experienceManager.currentLevel;

        data.roomsSpawned = randomDungeonCreator.totalRoomsSpawned;
        data.damageToPayload = 1000 - payload.health;
        data.totalTimePayload = payloadTimer.elapsedTime;

        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(Application.dataPath + $"/PlayerDataFile{dataFileNumber}.json", json);
    }

}