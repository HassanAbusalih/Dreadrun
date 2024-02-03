using UnityEngine;
using UnityEngine.Audio;

[CreateAssetMenu(fileName = "NewSoundEffect", menuName = "Audio/New Sound Effect")]
public class SoundSO : ScriptableObject
{
    [SerializeField] AudioMixerGroup mixerGroup;
    public AudioSource referenceAudioSource;
    public AudioClip[] audioClips;

    public void PlaySound(int index, AudioSourceType audioType, bool isLooping = false)
    {
        referenceAudioSource = AudioManager.instance.Play(GetSound(index), audioType, isLooping, mixerGroup);
    }
    public void PlayTrackkk(int index, AudioSourceType audioType, bool isLooping = false)
    {
        referenceAudioSource = AudioManager.instance.PlayTrack(GetSound(index), audioType, isLooping, mixerGroup);
    }
    public void StopSound(AudioSourceType audioType)
    { 
        AudioManager.instance.Stop(audioType);
    }
    public AudioClip GetSound(int index)
    {
        if (index < audioClips.Length)
        {
            return audioClips[index];
        }
        else
        {
            return null;
        }
    }

}
