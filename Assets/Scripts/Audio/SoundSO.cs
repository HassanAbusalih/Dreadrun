using UnityEngine;
using UnityEngine.Audio;

[CreateAssetMenu(fileName = "NewSoundEffect", menuName = "Audio/New Sound Effect")]
public class SoundSO : ScriptableObject
{
    [SerializeField] AudioMixerGroup mixerGroup;
    public AudioClip clips;

    public AudioSource Play(AudioSource newSource = null, bool isLooping = false)
    {
        AudioSource source = newSource;
        if (source == null)
        {
            GameObject obj = new GameObject("Sound", typeof(AudioSource));
            source = obj.GetComponent<AudioSource>();
            source.outputAudioMixerGroup = mixerGroup;
        }

        source.clip = clips;
        source.playOnAwake = false;
        source.loop = isLooping;
        source.PlayOneShot(clips);

        return source;
    }
}
