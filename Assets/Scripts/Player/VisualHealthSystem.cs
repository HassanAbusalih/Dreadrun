using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.PostProcessing;

public class VisualHealthSystem : DynamicPostProcessing
{

    [Header("Player Reference")]
    [SerializeField] Player player;

    [Header("Post Processing Volume References")]
    [SerializeField] Volume mediumHealthVolume;
    [SerializeField] Volume lowHealthVolume;

    [Header("Audio Source References")]
    [SerializeField] AudioSource mediumHealthSound;
    [SerializeField] AudioSource lowHealthSound;

    [Header("Visual Mode")]
    [SerializeField] VisualModeType visualModeType = VisualModeType.ShowBoth;

    [Header("Lerp Settings")]
    [Range(0,15)][SerializeField] float lerpInDuration;
    [Range(0,15)][SerializeField] float lerpOutDuration;
    [Range(0, 10)][SerializeField] float amplitude;
    [Range(0, 500)][SerializeField] float frequency;

    [Header("Health Thresholds To Fade Volumes")]
    [SerializeField] float mediumHealthThreshold;
    [SerializeField] float lowHealthThreshold;

    [Header("Audio Values")]
    [SerializeField] float mediumHealthSoundVolume;
    [SerializeField] float lowHealthSoundVolume;
    [SerializeField] float lowHealthPitch;

    bool meduimHealthSoundPlayed;
    bool lowHealthSoundPlayed;


    private void Start()
    {
        player = player ?? GetComponent<Player>();
        if(mediumHealthVolume!= null) mediumHealthVolume.weight = 0;
        if(lowHealthVolume!= null) lowHealthVolume.weight = 0;

    }

    private void Update()
    {
        TransitionToLowHealthVolume();
        TransitionToMeduimHealthVolume();
    }

    void TransitionToMeduimHealthVolume()
    {
        if(visualModeType == VisualModeType.ShowOnlyLowHealth) { return; }
        bool CanTransitionToMeduimhealth = player.playerStats.health <= mediumHealthThreshold && player.playerStats.health > lowHealthThreshold;

        if (CanTransitionToMeduimhealth)
        {
            LerpBetweenTwoWeightsForOneVolume(mediumHealthVolume, 1, lerpInDuration);
            PlayAnAudioSource(mediumHealthSound, ref meduimHealthSoundPlayed);
            FadeAnAudioSourceVolumeAndPitch(mediumHealthSound, mediumHealthSoundVolume, 1, lerpInDuration);
            return;
        }
        LerpBetweenTwoWeightsForOneVolume(mediumHealthVolume, 0, lerpInDuration);
        FadeOutAnAudioSourceAndStopPlaying(mediumHealthSound,1, ref meduimHealthSoundPlayed);
    }

    private void TransitionToLowHealthVolume()
    {
        if (visualModeType == VisualModeType.ShowOnlyMediumHealth) { return; }
        bool CanTransitionToLowHealth = player.playerStats.health <= lowHealthThreshold;

        if (CanTransitionToLowHealth)
        {
            if(lowHealthVolume.weight > 0.45f)
            { LerpToAndfroBetweenTwoWeightsForAVolume(lowHealthVolume, 0.5f, 1f, amplitude, frequency); }
            else { LerpBetweenTwoWeightsForOneVolume(lowHealthVolume, 0.5f, lerpInDuration); }
            
            PlayAnAudioSource(lowHealthSound, ref lowHealthSoundPlayed);
            FadeAnAudioSourceVolumeAndPitch(lowHealthSound, lowHealthSoundVolume, lowHealthPitch, lerpInDuration);
            return;
        }
        LerpBetweenTwoWeightsForOneVolume(lowHealthVolume, 0, lerpInDuration);
        FadeOutAnAudioSourceAndStopPlaying(lowHealthSound,1, ref lowHealthSoundPlayed);
    }

}

public enum VisualModeType
{
    ShowOnlyLowHealth,
    ShowOnlyMediumHealth,
    ShowBoth
}
