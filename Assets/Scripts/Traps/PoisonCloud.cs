using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoisonCloud : Trap
{
    [SerializeField] float expansionTime = 3f;
    GameObject childVFX;
    Vector3 targetScale = Vector3.one;
    Vector3 currentScale = Vector3.zero;

    private void Start()
    {
        currentScale = transform.localScale;
        childVFX = GetComponentInChildren<ParticleSystem>().gameObject;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.V))
        {
            childVFX.transform.localScale = Vector3.zero;
        }
        if (playerCount > 0)
        {
            timer += Time.deltaTime;
            if (timer > trapDelay)
            {
                childVFX.transform.LeanScale(targetScale, expansionTime);
            }
        }
        else
        {
            timer = 0;
        }
    }

    IEnumerator ExpandCloud()
    {
        float time = 0;
        while (time < expansionTime)
        {
            
            yield return null;
        }
    }
}