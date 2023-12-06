using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Music : MonoBehaviour
{
    [SerializeField] SoundSO musicSFX;
    [SerializeField] int musicPlaying;

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Player player))
        {
            musicSFX.StopSound(AudioSourceType.Music);
            musicPlaying = musicSFX.audioClips.Length;
            musicSFX.PlaySound(Random.Range(0, musicPlaying), AudioSourceType.Music);
            Destroy(this.gameObject);
        }
    }
}
