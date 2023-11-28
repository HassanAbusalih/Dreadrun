using System;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class AudioSettingsMenu : MonoBehaviour
{
    [SerializeField] Slider masterVolumeSlider;
    [SerializeField] Slider musicVolumeSlider;
    [SerializeField] Slider ambianceVolumeSlider;
    [SerializeField] Slider sfxVolumeSlider;
    [SerializeField] GameObject Return;
    [SerializeField] GameObject mainSettings;

    private AudioSettingsData audioSettingsData;
    private AudioManager audioManager;

    private void Start()
    {
        audioManager = AudioManager.instance;

        LoadAudioSettings();

        masterVolumeSlider.onValueChanged.AddListener(OnMasterVolumeChanged);
        musicVolumeSlider.onValueChanged.AddListener(OnMusicVolumeChanged);
        ambianceVolumeSlider.onValueChanged.AddListener(OnAmbianceVolumeChanged);
        sfxVolumeSlider.onValueChanged.AddListener(OnSFXVolumeChanged);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SaveAudioSettings();
            gameObject.SetActive(false);
        }
    }

    public void OnBackButtonPressed()
    {
        SaveAudioSettings();
        gameObject.SetActive(false);
    }

    void OnMasterVolumeChanged(float volume)
    {
        audioManager.SetVolume(volume);
    }

    void OnMusicVolumeChanged(float volume)
    {
        audioManager.SetVolume(volume, AudioSourceType.Music);
    }

    void OnAmbianceVolumeChanged(float volume)
    {
        audioManager.SetVolume(volume, AudioSourceType.Environment);
    }

    void OnSFXVolumeChanged(float volume)
    {
        foreach (AudioSourceType sourceType in Enum.GetValues(typeof(AudioSourceType)))
        {
            switch (sourceType)
            {
                case AudioSourceType.Environment:
                case AudioSourceType.Music:
                    continue;
                default:
                    audioManager.SetVolume(volume, sourceType);
                    break;
            }
        }
    }

    void SaveAudioSettings()
    {
        audioSettingsData = new AudioSettingsData
        {
            masterVolume = masterVolumeSlider.value,
            musicVolume = musicVolumeSlider.value,
            ambianceVolume = ambianceVolumeSlider.value,
            sfxVolume = sfxVolumeSlider.value
        };

        string json = JsonUtility.ToJson(audioSettingsData);
        File.WriteAllText(Application.persistentDataPath + "/audioSettings.json", json);
    }

    void LoadAudioSettings()
    {
        if (File.Exists(Application.persistentDataPath + "/audioSettings.json"))
        {
            string json = File.ReadAllText(Application.persistentDataPath + "/audioSettings.json");
            audioSettingsData = JsonUtility.FromJson<AudioSettingsData>(json);

            masterVolumeSlider.value = audioSettingsData.masterVolume;
            musicVolumeSlider.value = audioSettingsData.musicVolume;
            ambianceVolumeSlider.value = audioSettingsData.ambianceVolume;
            sfxVolumeSlider.value = audioSettingsData.sfxVolume;
        }
    }

    public void ExitAudioSettings()
    {
        Return.SetActive(false);
        mainSettings.SetActive(true);
    }
}

[Serializable]
public struct AudioSettingsData
{
    public float masterVolume;
    public float musicVolume;
    public float ambianceVolume;
    public float sfxVolume;
}
