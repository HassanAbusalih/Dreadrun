using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ambiance : MonoBehaviour
{
    [SerializeField] SoundSO backgroundSFX;
    void Awake()
    {
        backgroundSFX.PlaySound(0, AudioSourceType.Music, true);
    }
}
