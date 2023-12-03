using System;
using UnityEngine;
using UnityEngine.UI;

public class ExperienceManager : MonoBehaviour
{
    [SerializeField] int expIncreaseAddon, expToLevelUp;
    [SerializeField] SoundSO levelUpSFX;
    [SerializeField] Image expBar;
    PerkSelector perkSelector;
    int currentLevel;
    int currentExp;

    public static bool CollectExp { get; private set; } = true;

    private void Awake()
    {
        CollectExp = true;
        UpdateExpBarUI();
    }
    void HandleExperience(int newExperience)
    {
        currentExp += newExperience;
        UpdateExpBarUI();

        if (currentExp >= expToLevelUp)
        {
            LevelUp();
            if (levelUpSFX != null)
            {
                levelUpSFX.PlaySound(0, AudioSourceType.Player);
            }
        }
    }

    void UpdateExpBarUI()=> expBar.fillAmount = (float)currentExp / expToLevelUp;

    void LevelUp()
    {
        perkSelector.RandomPerkSelector();
        CollectExp = false;
        currentLevel++;
        currentExp = 0;
        expToLevelUp += expIncreaseAddon;
        UpdateExpBarUI();
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