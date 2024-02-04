using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using System;
using Random = UnityEngine.Random;


public class DynamicPostProcessing : MonoBehaviour
{
    //post processing volume  related fcuntions 
    protected virtual  void LerpBetweenTwoWeightsForOneVolume(Volume volume, float targetWeight, float lerpduration)
    {
        float lerpValue = Time.deltaTime / lerpduration;
        volume.weight = Mathf.Lerp(volume.weight, targetWeight, lerpValue);
    }
    protected virtual void LerpToAndfroBetweenTwoWeightsForAVolume(Volume volume,float startweight, float endweight, float amplitude, float frequency)
    {
        float lerpValue = amplitude * Mathf.Sin(frequency * Time.time);
        volume.weight = Mathf.Lerp(startweight, endweight, lerpValue);
    }
    protected virtual void FlickerBetweenTwoWeightsForAVolumeContinously(Volume volume, float startWeight, float EndWeight, float amplitude, float frequency)
    {
        float flickerangevalue = amplitude * Random.Range(0f, 1f);
        float clampedFlickerValue = Mathf.Clamp(flickerangevalue, startWeight, EndWeight);
        volume.weight = Mathf.Lerp(volume.weight,clampedFlickerValue , Time.deltaTime * frequency);
    }
    // audio related fucntions, need to put this in a seperate audio helper class
    protected void PlayAnAudioSource(AudioSource source,ref bool audioPlayed)
    {
        if (source == null || audioPlayed) return;
        audioPlayed = true;
        source.Play();
    }
    protected void StopAnAudioSource(AudioSource source, ref bool audioPlayed)
    {
        if (source == null || !audioPlayed) return;
        audioPlayed=false;
        source.Stop();
    }
    protected void FadeAnAudioSourceVolume(AudioSource source,float targetVolume,float lerpDuration)
    {
        if(source==null) return;
        float lerpValue = Time.deltaTime/lerpDuration;
        source.volume = Mathf.Lerp(source.volume, targetVolume,lerpValue);
    }
    protected void FadeAnAudioSourcePitch(AudioSource source, float targetPitch, float lerpDuration)
    {
        if(source==null) return;
        float lerpValue = Time.deltaTime/lerpDuration;
        source.pitch = Mathf.Lerp(source.pitch, targetPitch, lerpValue);
    }
    protected void FadeAnAudioSourceVolumeAndPitch(AudioSource source, float targetVolume, float targetPitch, float lerpDuration)
    {
        if(source==null) return;
        float lerpValue = Time.deltaTime/lerpDuration;
        FadeAnAudioSourceVolume(source, targetVolume, lerpDuration);
        source.pitch = Mathf.Lerp(source.pitch, targetPitch, lerpValue);
    }

    protected void FadeOutAnAudioSourceAndStopPlaying(AudioSource source,float lerpDuration,ref bool soundPlayed)
    {
        if(source==null) return;
        FadeAnAudioSourceVolume(source, 0, lerpDuration);
        soundPlayed = false;
        if (source.volume <= 0.01f && source.isPlaying )
        {
            source.Stop(); 
        }
    }
}
