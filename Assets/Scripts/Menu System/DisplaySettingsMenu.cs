using System;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class DisplaySettingsMenu : MonoBehaviour
{
    [SerializeField] Dropdown fullscreenDropdown;
    [SerializeField] Dropdown qualityDropdown;
    [SerializeField] GameObject Return;

    private void Start()
    {
        /*fullscreenDropdown.onValueChanged.AddListener(delegate { OnFullscreenDropdownChange(); });
        qualityDropdown.onValueChanged.AddListener(delegate { OnQualityDropdownChange(); });

        LoadDisplaySettings();*/
    }

    public void OnBackButtonPressed()
    {
        SaveDisplaySettings();
        gameObject.SetActive(false);
    }

    public void ExitDisplaySettings()
    {
        Return.SetActive(false);
    }

    private void OnFullscreenDropdownChange()
    {
        switch (fullscreenDropdown.value)
        {
            case 0:
                Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
                break;
            case 1:
                Screen.fullScreenMode = FullScreenMode.MaximizedWindow;
                break;
            case 2:
                Screen.fullScreenMode = FullScreenMode.Windowed;
                break;
        }
    }

    private void OnQualityDropdownChange()
    {
        QualitySettings.SetQualityLevel(qualityDropdown.value);
    }

    private void LoadDisplaySettings()
    {
        string filePath = Application.persistentDataPath + "/settings.json";

        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            SettingsData data = JsonUtility.FromJson<SettingsData>(json);

            fullscreenDropdown.value = data.fullscreenDropdownValue;
            qualityDropdown.value = data.qualityDropdownValue;
        }
    }

    private void SaveDisplaySettings()
    {
        SettingsData data = new SettingsData();
        data.fullscreenDropdownValue = fullscreenDropdown.value;
        data.qualityDropdownValue = qualityDropdown.value;

        string json = JsonUtility.ToJson(data);
        string filePath = Application.persistentDataPath + "/settings.json";
        File.WriteAllText(filePath, json);
    }

    private void OnApplicationQuit()
    {
        SaveDisplaySettings();
    }

    [System.Serializable]
    private class SettingsData
    {
        public int fullscreenDropdownValue;
        public int qualityDropdownValue;
    }
}

    [Serializable]
    public struct DisplaySettingsData
    {
        public bool fullscreen;
        public bool borderlessWindowed;
        public bool windowed;
        public bool ultraGraphics;
        public bool highGraphics;
        public bool mediumGraphics;
        public bool lowGraphics;
    }
