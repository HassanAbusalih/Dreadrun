using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

[CreateAssetMenu(fileName = "NewSoundEffect", menuName = "Audio/New Sound Effect")]
public class SoundSO : ScriptableObject
{
    [SerializeField] AudioMixerGroup mixerGroup;
    public AudioClip clips;
    [HideInInspector]
    public AudioSource audioSource;

    public AudioSource Play(bool isLooping = false)
    {

        AudioSource source = null;
        if (source == null)
        {
            GameObject obj = new GameObject("Sound", typeof(AudioSource));
            source = obj.GetComponent<AudioSource>();
            audioSource = source;
            source.outputAudioMixerGroup = mixerGroup;
        }

        source.clip = clips;
        source.playOnAwake = false;
        source.loop = isLooping;
        source.Play();

        if (source.loop == true)
        {
            return source;
        }
        audioSource = null;
        Destroy(source.gameObject, source.clip.length / source.pitch);

        return source;
    }
    public AudioSource Stop()
    {
        if (audioSource != null)
        {
            audioSource.Stop();
        }
        return audioSource;
    }
}
