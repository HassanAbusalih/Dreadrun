using System;
using System.IO;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    [SerializeField] float[] volumeLevels;
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
        audioSources = new AudioSource[enumCount];
        volumeLevels = new float[3];
        for (int i = 0; i < volumeLevels.Length; i++)
        {
            volumeLevels[i] = 1f;
        }
        for (int i = 1; i < audioSources.Length; i++)
        {
            GameObject audioObject = new GameObject("AudioObject");
            audioObject.transform.SetParent(transform, true);
            audioSources[i] = audioObject.AddComponent<AudioSource>();
            AudioSourceType audioSourceType = (AudioSourceType)(i);
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

    public AudioSource Play(AudioClip audioClip, AudioSourceType audioType, bool isLooping = false, AudioMixerGroup mixerGroup = null)
    {
        AudioSource source = audioSources[(int)audioType];
        source.loop = isLooping;
        if (mixerGroup != null)
        {
            audioSources[(int)audioType].outputAudioMixerGroup = mixerGroup;
        }
        audioSources[(int)audioType].PlayOneShot(audioClip);
        return source;
    }
    public AudioSource PlayTrack(AudioClip audioClip, AudioSourceType audioType, bool isLooping = false, AudioMixerGroup mixerGroup = null)
    {
        AudioSource source = audioSources[(int)audioType];
        source.loop = isLooping;
        if (mixerGroup != null)
        {
            audioSources[(int)audioType].outputAudioMixerGroup = mixerGroup;
        }
        audioSources[(int)audioType].clip = audioClip;
        audioSources[(int)audioType].Play();
        return source;
    }

    public void Stop(AudioSourceType audioType)
    {
        audioSources[(int)audioType].Stop();
    }

    public void SetVolume(float volume, AudioSourceType audioType = 0)
    {
        int index = Mathf.Clamp((int)audioType, 0, volumeLevels.Length - 1);
        volumeLevels[index] = volume;
        for (int i = 1; i < audioSources.Length; i++)
        {
            int volumeIndex = Mathf.Clamp(i, 1, volumeLevels.Length - 1);
            audioSources[i].volume = volumeLevels[volumeIndex] * volumeLevels[0];
        }
    }

    public void SetMixerVolume(float volume, AudioSourceType audioType = 0)
    {
        volumeLevels[(int)audioType] = volume;
        audioSources[(int)audioType].volume = volumeLevels[(int)audioType] * volumeLevels[(int)AudioSourceType.Master];
    }
}


public enum AudioSourceType
{
    Master = 0,
    Music = 1,
    Player,
    PayloadAmbiance,
    Enemy,
    EnemyShoot,
    Weapons,
    PayloadSounds
}
