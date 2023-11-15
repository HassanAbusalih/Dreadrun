using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class mainMenu : MonoBehaviour
{
    // Start is called before the first frame update
    public void PlayGame()
    {
        SceneManager.LoadScene("");
    }

    // Update is called once per frame
    public void Settings()
    {
        SceneManager.LoadScene("SettingsMenu");
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void BackToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void AudioSettings()
    {
        SceneManager.LoadScene("Audio");
    }

    public void DisplaySettings()
    {
        SceneManager.LoadScene("");
    }

    public void ControlSettings()
    {
        SceneManager.LoadScene("Controls");
    }
}
