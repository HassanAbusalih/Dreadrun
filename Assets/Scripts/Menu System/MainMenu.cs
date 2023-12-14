using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class MainMenu : MonoBehaviour
{
    [SerializeField] GameObject settingsMenu;
    [SerializeField] GameObject mainMenu;
    [SerializeField] string GameScene;
    [SerializeField] string tutorialScene;
    [SerializeField] bool hasCompletedTutorial = false;

    private void Start()
    {
        LoadDataFromJson();
        Time.timeScale = 1;
    }

    private void LoadDataFromJson()
    {
        string filePath = "interactionData.json";
        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            InteractData data = JsonUtility.FromJson<InteractData>(json);
            hasCompletedTutorial = data.tutorialstate;
        }
        else
        {
            Debug.LogWarning("No saved data file found.");
        }
    }

    public void PlayGame()
    {
        if (hasCompletedTutorial == true)
        {
            SceneManager.LoadScene(GameScene);
        }
        else
        {
            SceneManager.LoadScene(tutorialScene);
        }
        
    }

    public void OpenSettingsMenu()
    {
        settingsMenu.SetActive(true);
        mainMenu.SetActive(false);
    }

    public void CloseSettingsMenu()
    {
        settingsMenu.SetActive(false);
        mainMenu.SetActive(true);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
