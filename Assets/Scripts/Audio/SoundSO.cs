using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

[CreateAssetMenu(fileName = "NewSoundEffect", menuName = "Audio/New Sound Effect")]
public class SoundSO : ScriptableObject
{
    [SerializeField] AudioMixerGroup mixerGroup;
    public AudioClip clips;

    public AudioSource Play(AudioSource audioSource = null)
    {
        AudioSource source = audioSource;
        if (source == null)
        {
            GameObject obj = new GameObject("Sound", typeof(AudioSource));
            source = obj.GetComponent<AudioSource>();
            source.outputAudioMixerGroup = mixerGroup;
        }

        source.clip = clips;

        source.Play();
        Destroy(source.gameObject, source.clip.length / source.pitch);
        return source;
    }   
}
