using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class MainMenu : MonoBehaviour
{
    [SerializeField] GameObject settingsMenu;
    [SerializeField] GameObject mainMenu;
    [SerializeField] string GameScene;

    public void PlayGame()
    {
        SceneManager.LoadScene(GameScene);
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
