using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
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
        GameObject audioObject = new GameObject("AudioObject");
        audioObject.transform.SetParent(transform, true);

        for (int i = 0; i < enumCount; i++)
        {
            audioSources[i] = audioObject.AddComponent<AudioSource>();
            AudioSourceType audioSourceType = (AudioSourceType)i;
            audioSources[i].name = audioSourceType.ToString();
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
        if(audioType != AudioSourceType.Master)
            audioSources[(int)audioType].volume = volume;
        else
        {
            for (int i = 0; i < audioSources.Length; i++)
            {
                audioSources[i].volume *= volume;
            }
        }
    }
}

public enum AudioSourceType
{
    Master = 0,
    Player,
    Enemy,
    Environment,
    Weapons,
    Music
}
