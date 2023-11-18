using System;
using UnityEngine;

public class ExperienceManager : MonoBehaviour
{
    [SerializeField] int maxExpToLevelUp, requiredExpToLevelUp;
    [SerializeField] SoundSO levelUpSFX;
    PerkSelector perkSelector;
    int currentLevel;
    int currentExp;
    public static bool CollectExp { get; private set; } = true;

    void HandleExperience(int newExperience)
    {
        currentExp += newExperience;
        if (currentExp >= maxExpToLevelUp)
        {
            LevelUp();
            if (levelUpSFX != null)
            {
                levelUpSFX.PlaySound(0, AudioSourceType.Player);
            }
        }
    }

    void LevelUp()
    {
        perkSelector.RandomPerkSelector();
        CollectExp = false;
        currentLevel++;
        currentExp = 0;
        maxExpToLevelUp += requiredExpToLevelUp;
        Debug.Log("You are now level " + currentLevel);
    }

    void AllowExpCollection() => CollectExp = true;

    private void OnEnable()
    {
        ExpOrb.OnExpOrbCollected += HandleExperience;
        PerkSelector.OnPerkSelection += AllowExpCollection;
        perkSelector = FindObjectOfType<PerkSelector>();
    }

    private void OnDisable()
    {
        ExpOrb.OnExpOrbCollected -= HandleExperience;
        PerkSelector.OnPerkSelection -= AllowExpCollection;
    }
}