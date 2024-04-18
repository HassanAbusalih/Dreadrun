using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using static UnityEngine.ParticleSystem;
public class RotationAnimator : MonoBehaviour
{
    [SerializeField] MinMaxCurve rotationCurve;
    [SerializeField]float rotationSpeed;
    public bool Disabled;


    private void Update()
    {
        if(Disabled) return;
        float time = rotationCurve.Evaluate(Time.deltaTime);
        transform.localRotation *= Quaternion.Euler(0, rotationSpeed * time,0);
    }
}
