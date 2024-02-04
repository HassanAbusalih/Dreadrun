using System;
using System.IO;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioSettingsMenu : MonoBehaviour
{
    [SerializeField] Slider masterVolumeSlider;
    [SerializeField] Slider musicVolumeSlider;
    [SerializeField] Slider sfxVolumeSlider;
    private AudioSettingsData audioSettingsData;
    private AudioManager audioManager;
    [SerializeField] AudioMixerGroup masterMixer;
    [SerializeField] AudioMixerGroup musicMixer;
    [SerializeField] AudioMixerGroup sfxMixer;

    private void Start()
    {
        LoadAudioSettings();
    }

    public void OnMasterVolumeChanged(float volume)
    {
        audioSettingsData.masterVolume = volume;
        audioManager.SetVolume(volume);
        float mixerVolume = Mathf.Log10(audioSettingsData.masterVolume) * 20;
        masterMixer.audioMixer.SetFloat("Master",mixerVolume);
    }

    public void OnMusicVolumeChanged(float volume)
    {
        audioSettingsData.musicVolume = volume;
        audioManager.SetVolume(volume,AudioSourceType.Player);
        float mixerVolume = Mathf.Log10(audioSettingsData.musicVolume) * 20;
        musicMixer.audioMixer.SetFloat("Music",mixerVolume);
    }

    public void OnSFXVolumeChanged(float volume)
    {
        audioSettingsData.sfxVolume = volume;
        audioManager.SetVolume(volume,AudioSourceType.Player);
        float mixerVolume = Mathf.Log10(audioSettingsData.sfxVolume) * 20;
        sfxMixer.audioMixer.SetFloat("SFX",mixerVolume);
    }

    void SaveAudioSettings()
    {
        if(File.Exists("audioSettings.json"))
        {
            File.Delete("audioSettings.json");
        }
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
    public float masterVolume;
    public float musicVolume;
    public float sfxVolume;
}
