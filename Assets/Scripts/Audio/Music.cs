using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Music : MonoBehaviour
{
    [SerializeField] SoundSO musicSFX;
    [SerializeField] int musicPlaying;
    bool isMusicPlaying = false;


    private void Update()
    {
        musicSFX.FadeInVolume();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Player player)&& !isMusicPlaying)
        {
            musicSFX.StopSound(AudioSourceType.Music);
            musicSFX.PlayTrackkk(musicPlaying, AudioSourceType.Music, true,true);
            isMusicPlaying = true;
            Destroy(gameObject,4f);
        }
    }
}
