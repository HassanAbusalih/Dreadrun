using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerExp : MonoBehaviour
{
    [SerializeField] int currentExp, maxExpToLevelUp, currentLevel, expAmountToLevelUp;

    private void OnEnable()
    {
        ExperienceManager.Instance.OnExperienceChange += HandleExperience;
    }

    private void HandleExperience(int newExperience)
    {
        currentExp += newExperience;
        if(currentExp >= maxExpToLevelUp)
        {
            LevelUp();
        }
    }

    void LevelUp()
    {
        currentLevel++;
        currentExp = 0;
        maxExpToLevelUp += expAmountToLevelUp;
        Debug.Log("You are now level " + currentLevel);
    }

    private void OnDisable()
    {
        ExperienceManager.Instance.OnExperienceChange -= HandleExperience;
    }
}
