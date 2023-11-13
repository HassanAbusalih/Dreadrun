using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerExp : MonoBehaviour
{
    [SerializeField] int currentExp, maxExpToLevelUp, currentLevel, expAmountToLevelUp;

    public PerkSelector perkSelector;
    public ExperienceManager expmanager;
    [SerializeField] SoundSO levelUpSFX;
    private void OnEnable()
    {
        if (expmanager != null)
        {
            expmanager = FindObjectOfType<ExperienceManager>();
            expmanager.OnExperienceChange += HandleExperience;
        }
        perkSelector = FindObjectOfType<PerkSelector>();

    }

    private void HandleExperience(int newExperience)
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

    public void LevelUp()
    {
        perkSelector.RandomPerkSelector();
        currentLevel++;
        currentExp = 0;
        maxExpToLevelUp += expAmountToLevelUp;
        Debug.Log("You are now level " + currentLevel);
    }

    private void OnDisable()
    {
        if (expmanager != null)
        {
            expmanager.OnExperienceChange -= HandleExperience;
        }

    }
}
