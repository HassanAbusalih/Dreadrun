using System;
using System.IO;
using UnityEngine;
using TMPro;

public class DisplaySettingsMenu : MonoBehaviour
{
    [SerializeField] TMP_Dropdown screenMode;
    [SerializeField] TMP_Dropdown resolution;
    [SerializeField] TMP_Dropdown refreshRate;
    DisplaySettingsData data;

    public void OnBackButtonPressed()
    {
        SaveDisplaySettings();
        gameObject.SetActive(false);
    }

    public void ExitDisplaySettings()
    {
        SaveDisplaySettings();
    }

    public void ChangeScreenMode(int value)
    {
        switch (value)
        {
            case 0:
                Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
                break;
            case 1:
                Screen.fullScreenMode = FullScreenMode.ExclusiveFullScreen;
                break;
            case 2:
                Screen.fullScreenMode = FullScreenMode.Windowed;
                break;
            default:
                Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
                break;
        }
        if (data.screenMode != value) { data.screenMode = value; }
    }

    public void ChangeResolution(int value)
    {
        switch (value)
        {
            case 0:
                Screen.SetResolution(1920, 1080, Screen.fullScreenMode);
                break;
            case 1:
                Screen.SetResolution(1280, 720, Screen.fullScreenMode);
                break;
            case 2:
                Screen.SetResolution(2560, 1440, Screen.fullScreenMode);
                break;
                default:
                Screen.SetResolution(1920, 1080, Screen.fullScreenMode);
                break;
        }
        if (data.resolution != value) { data.resolution = value; }
    }

    public void ChangeRefreshRate(int value)
    {
        switch (value)
        {
            case 0:
                Application.targetFrameRate = 60;
                break;
            case 1:
                Application.targetFrameRate = 120;
                break;
            case 2:
                Application.targetFrameRate = 144;
                break;
            default:
                Application.targetFrameRate = 60;
                break;
        }
        if (data.refreshRate != value) { data.refreshRate = value; }
    }

    private void LoadDisplaySettings()
    {
        if (data == null) { data = new DisplaySettingsData(); }
        string filePath = "settings.json";

        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            data = JsonUtility.FromJson<DisplaySettingsData>(json);
        }
        ChangeScreenMode(data.screenMode);
        screenMode.value = data.screenMode;
        ChangeResolution(data.resolution);
        resolution.value = data.resolution;
        ChangeRefreshRate(data.refreshRate);
        refreshRate.value = data.refreshRate;
    }

    private void SaveDisplaySettings()
    {
        data = new DisplaySettingsData
        {
            screenMode = screenMode.value,
            resolution = resolution.value,
            refreshRate = refreshRate.value
        };
        string json = JsonUtility.ToJson(data);
        File.WriteAllText("settings.json", json);
    }

    private void OnApplicationQuit()
    {
        SaveDisplaySettings();
    }

    private void OnEnable()
    {
        LoadDisplaySettings();
    }

    private void OnDisable()
    {
        SaveDisplaySettings();
    }
}

[Serializable]
public class DisplaySettingsData
{
    public int screenMode = 0;
    public int resolution = 0;
    public int refreshRate = 0;
}
