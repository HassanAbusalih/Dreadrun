using UnityEngine;

public class SettingsMenu : MonoBehaviour
{
    [SerializeField] GameObject controlsSettingsScreen;
    [SerializeField] GameObject displaySettingsScreen;
    [SerializeField] GameObject audioSettingsScreen;
    [SerializeField] GameObject settingsMenu;
    [SerializeField] GameObject mainMenu;


    public void ShowControlsScreen()
    {
        controlsSettingsScreen.SetActive(true);
        displaySettingsScreen.SetActive(false);
        audioSettingsScreen.SetActive(false);
    }

    public void ShowDisplayScreen()
    {
        displaySettingsScreen.SetActive(true);
        audioSettingsScreen.SetActive(false);
        controlsSettingsScreen.SetActive(false);
    }

    public void ShowAudioScreen()
    {
        audioSettingsScreen.SetActive(true);
        controlsSettingsScreen.SetActive(false);
        displaySettingsScreen.SetActive(false);
    }

    public void BackToMain()
    {
        settingsMenu.SetActive(false);
        displaySettingsScreen.SetActive(false);
        controlsSettingsScreen.SetActive(false);
        audioSettingsScreen.SetActive(false);
        mainMenu.SetActive(true);
    }
    
    public void OpenSettings()
    {
        settingsMenu.SetActive(true);
        audioSettingsScreen.SetActive(true);
        mainMenu.SetActive(false);
    }
}
