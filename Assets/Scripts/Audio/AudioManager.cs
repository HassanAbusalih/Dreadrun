using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    AudioSource audioSource;
    public void AudioSources(AudioClip audioClip)
    {
        audioSource.PlayOneShot(audioClip);
    }
}
