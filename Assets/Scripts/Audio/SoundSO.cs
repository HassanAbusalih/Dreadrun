using UnityEngine;
using UnityEngine.Audio;

[CreateAssetMenu(fileName = "NewSoundEffect", menuName = "Audio/New Sound Effect")]
public class SoundSO : ScriptableObject
{
    [SerializeField] AudioMixerGroup mixerGroup;
    public AudioSource referenceAudioSource;
    public AudioClip[] audioClips;

    bool playActivated = false;

    public void PlaySound(int index, AudioSourceType audioType, bool isLooping = false, bool muteOnStart = false)
    {
        playActivated = true;
        referenceAudioSource = AudioManager.instance.Play(GetSound(index), audioType, isLooping, mixerGroup);
        referenceAudioSource.volume = muteOnStart ? 0 : 1;
    }
    public void PlayTrackkk(int index, AudioSourceType audioType, bool isLooping = false, bool muteOnStart = false)
    {
        playActivated = true;
        referenceAudioSource = AudioManager.instance.PlayTrack(GetSound(index), audioType, isLooping, mixerGroup);
        referenceAudioSource.volume = muteOnStart ? 0 : 1;

    }
    public void FadeInVolume(float fadeSpeed=0.15f)
    {
        if (referenceAudioSource == null) return;
        float targetVolume = playActivated ? 1f : 0f;
        referenceAudioSource.volume = Mathf.Lerp(referenceAudioSource.volume, targetVolume, Time.unscaledDeltaTime * fadeSpeed);
    }
    public void StopSound(AudioSourceType audioType)
    { 
        playActivated = false;
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
