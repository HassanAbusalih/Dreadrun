//All player data that needs to be stored is stored here
using System.Collections.Generic;
[System.Serializable]
public class PlayerData
{
    public int maximumLevelReached;
    public int totalExpCollected;
    public float totalHealth;
    public List<Perk> perks = new List<Perk>();
    public string PerkName;
    public int roomsSpawned;
    public float damageToPayload;
    public float totalTimePayload;
}