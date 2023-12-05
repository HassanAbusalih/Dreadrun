using System;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DisplaySettingsMenu : MonoBehaviour
{
    [SerializeField] TMP_Dropdown screenMode;
    [SerializeField] TMP_Dropdown resolution;
    [SerializeField] TMP_Dropdown refreshRate;
    [SerializeField] GameObject Return;
    DisplaySettingsData data;

    private void Start()
    {
        LoadDisplaySettings();
    }

    public void OnBackButtonPressed()
    {
        SaveDisplaySettings();
        gameObject.SetActive(false);
    }

    public void ExitDisplaySettings()
    {
        Return.SetActive(false);
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
                Application.targetFrameRate = 140;
                break;
        }
        if (data.refreshRate != value) { data.refreshRate = value; }
    }

    private void LoadDisplaySettings()
    {
        string filePath = Application.persistentDataPath + "/settings.json";

        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            data = JsonUtility.FromJson<DisplaySettingsData>(json);

            ChangeScreenMode(data.screenMode);
            ChangeResolution(data.resolution);
            ChangeRefreshRate(data.refreshRate);
        }
    }

    private void SaveDisplaySettings()
    {
        string json = JsonUtility.ToJson(data);
        string filePath = Application.persistentDataPath + "/settings.json";
        File.WriteAllText(filePath, json);
    }

    private void OnApplicationQuit()
    {
        SaveDisplaySettings();
    }
}

    [Serializable]
    public struct DisplaySettingsData
    {
        public int screenMode;
        public int resolution;
        public int refreshRate;
    }
