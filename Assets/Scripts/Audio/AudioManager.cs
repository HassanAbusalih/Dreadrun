using System;
using System.IO;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    float[] volumeLevels;
    [SerializeField] AudioSource[] audioSources;
    AudioMixerGroup mixerGroup;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        int enumCount = Enum.GetNames(typeof(AudioSourceType)).Length;
        audioSources = new AudioSource[enumCount - 1];
        volumeLevels = new float[enumCount];

        for (int i = 0; i < audioSources.Length; i++)
        {
            GameObject audioObject = new GameObject("AudioObject");
            audioObject.transform.SetParent(transform, true);
            audioSources[i] = audioObject.AddComponent<AudioSource>();
            AudioSourceType audioSourceType = (AudioSourceType)(i + 1);
            audioSources[i].name = audioSourceType.ToString();
        }
        if (File.Exists("audioSettings.json"))
        {
            string json = File.ReadAllText("audioSettings.json");
            AudioSettingsData audioSettingsData = JsonUtility.FromJson<AudioSettingsData>(json);
            SetVolume(audioSettingsData.masterVolume);
            SetVolume(audioSettingsData.musicVolume, AudioSourceType.Music);
            SetVolume(audioSettingsData.sfxVolume, AudioSourceType.Player);
        }
    }

    public void Play(AudioClip audioClip, AudioSourceType audioType, bool isLooping = false, AudioMixerGroup mixerGroup = null)
    {
        audioSources[(int)audioType].loop = isLooping;
        if (mixerGroup != null)
        {
            audioSources[(int)audioType].outputAudioMixerGroup = mixerGroup;
        }
        audioSources[(int)audioType].PlayOneShot(audioClip);

    }

    public void Stop(AudioSourceType audioType)
    {
        audioSources[(int)audioType].Stop();
    }

    public void SetVolume(float volume, AudioSourceType audioType = 0)
    {
        volumeLevels[(int)audioType] = volume;
        for (int i = 1; i < audioSources.Length; i++)
        {
            audioSources[i].volume = volumeLevels[i] * volumeLevels[0];
        }
    }
}

public enum AudioSourceType
{
    Master = 0,
    Player,
    Enemy,
    Weapons,
    Music
}
