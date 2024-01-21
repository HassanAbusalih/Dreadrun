using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRM;

public class AnimationTrigger : MonoBehaviour
{
    public enum playType { playOnTriggerEnter, playOnTriggerExit, PlayAfterEnteringAndExitingTriggerTwice };

    [SerializeField] playType animationPlayType;
    [SerializeField] AnimationClip animationClip;
    [SerializeField] int animationLayer;
    [SerializeField] Animator animator;
    int timesEntered;


    private void OnTriggerEnter(Collider other)
    {
        timesEntered++;
        if (animationPlayType == playType.playOnTriggerEnter)
        {
            animator.Play(animationClip.name, animationLayer);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (animationPlayType == playType.playOnTriggerExit)
        {
            animator.Play(animationClip.name, animationLayer);
        }
        else if (animationPlayType == playType.PlayAfterEnteringAndExitingTriggerTwice)
        {
            if (timesEntered >= 2)
            animator.Play(animationClip.name, animationLayer);
        }
    }




}
