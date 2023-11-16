using UnityEngine;

public class SettingsMenu : MonoBehaviour
{
    [SerializeField] GameObject controlsSettingsScreen;
    [SerializeField] GameObject displaySettingsScreen;
    [SerializeField] GameObject audioSettingsScreen;

    public void ShowControlsScreen()
    {
        controlsSettingsScreen.SetActive(true);
    }

    public void ShowDisplayScreen()
    {
        displaySettingsScreen.SetActive(true);
    }

    public void ShowAudioScreen()
    {
        audioSettingsScreen.SetActive(true);
    }
}
