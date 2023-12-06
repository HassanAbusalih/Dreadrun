using System;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class AudioSettingsMenu : MonoBehaviour
{
    [SerializeField] Slider masterVolumeSlider;
    [SerializeField] Slider musicVolumeSlider;
    [SerializeField] Slider sfxVolumeSlider;
    private AudioSettingsData audioSettingsData;
    private AudioManager audioManager;

    private void Start()
    {
        LoadAudioSettings();
    }

    public void OnMasterVolumeChanged(float volume)
    {
        audioManager.SetVolume(volume);
        audioSettingsData.masterVolume = volume;
    }

    public void OnMusicVolumeChanged(float volume)
    {
        audioManager.SetVolume(volume, AudioSourceType.Music);
        audioSettingsData.musicVolume = volume;
    }

    public void OnSFXVolumeChanged(float volume)
    {
        audioManager.SetVolume(volume, AudioSourceType.Player);
        audioSettingsData.sfxVolume = volume;
    }

    void SaveAudioSettings()
    {
        string json = JsonUtility.ToJson(audioSettingsData);
        File.WriteAllText("audioSettings.json", json);
    }

    void LoadAudioSettings()
    {
        if (AudioManager.instance == null)
        {
            audioManager = new GameObject("AudioManager").AddComponent<AudioManager>();
        }
        else
        {
            audioManager = AudioManager.instance;
        }

        if (audioSettingsData == null) { audioSettingsData = new AudioSettingsData(); }

        if (File.Exists("audioSettings.json"))
        {
            string json = File.ReadAllText("audioSettings.json");
            audioSettingsData = JsonUtility.FromJson<AudioSettingsData>(json);

            OnMasterVolumeChanged(audioSettingsData.masterVolume);
            masterVolumeSlider.value = audioSettingsData.masterVolume;
            OnMusicVolumeChanged(audioSettingsData.musicVolume);
            musicVolumeSlider.value = audioSettingsData.musicVolume;
            OnSFXVolumeChanged(audioSettingsData.sfxVolume);
            sfxVolumeSlider.value = audioSettingsData.sfxVolume;
        }
    }

    private void OnApplicationQuit()
    {
        SaveAudioSettings();
    }

    private void OnEnable()
    {
        LoadAudioSettings();
    }

    private void OnDisable()
    {
        SaveAudioSettings();
    }
}

[Serializable]
public class AudioSettingsData
{
    public float masterVolume = 1;
    public float musicVolume = 1;
    public float sfxVolume = 1;
}
