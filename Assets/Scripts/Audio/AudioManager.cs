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
        for (int i = 0; i < enumCount; i++)
        {
            GameObject audioObject = new GameObject();
            audioSources[i] = audioObject.AddComponent<AudioSource>();
            audioObject.transform.SetParent(transform, true);
            AudioSourceType audioSourceType = (AudioSourceType)i;
            audioObject.name = audioSourceType.ToString();
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
}

public enum AudioSourceType
{
    Player,
    Enemy,
    Environment,
    Weapons
}
