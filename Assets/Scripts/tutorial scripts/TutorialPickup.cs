using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialPickup : MonoBehaviour
{
    [SerializeField] RotationAnimator rotationAnimator;
    [SerializeField] GameObject wall;

    void Update()
    {
        if (rotationAnimator.Disabled == true)
        {
            wall.SetActive(false);
        }
    }
}
