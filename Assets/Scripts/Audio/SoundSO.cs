using UnityEngine;
using UnityEngine.Audio;

[CreateAssetMenu(fileName = "NewSoundEffect", menuName = "Audio/New Sound Effect")]
public class SoundSO : ScriptableObject
{
    [SerializeField] AudioMixerGroup mixerGroup;
    public AudioClip[] audioClips;

    private AudioSource Play(int index, AudioSource newSource = null, bool isLooping = false, bool setAsChild = true)
    {
        AudioSource source = newSource;
        if (source == null)
        {
            GameObject obj = new GameObject("Sound", typeof(AudioSource));
            source = obj.GetComponent<AudioSource>();
            source.outputAudioMixerGroup = mixerGroup;
        }
        source.playOnAwake = false;
        source.loop = isLooping;
        source.PlayOneShot(GetSound(index));
        if (!setAsChild)
        {
            Destroy(source.gameObject, 1);
        }
        return source;
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

    public void PlaySound(ref AudioSource source, int index, GameObject gameObject, bool isLooping = false, bool setAsChild = true)
    {
        if (source == null)
        {
            source = Play(index, null, isLooping, setAsChild);
            if (setAsChild)
            {
                source.gameObject.transform.parent = gameObject.transform;
            }
        }
        else
        {
            source.PlayOneShot(GetSound(index));
        }
    }
}
