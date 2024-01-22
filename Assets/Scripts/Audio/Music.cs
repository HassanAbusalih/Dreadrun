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
            musicSFX.PlayTrackkk(musicPlaying, AudioSourceType.Music, true);
            Destroy(this.gameObject);
        }
    }
}
