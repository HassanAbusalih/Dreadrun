using System.Collections;
using UnityEngine;

public class ExpandingTrap : Trap
{
    [SerializeField] float expansionTime = 3f;
    [SerializeField] float shrinkDelay = 10f;
    ParticleSystem childVFX;

    private void Start()
    {
        childVFX = GetComponentInChildren<ParticleSystem>();
        float scaleAverage = (transform.localScale.x + transform.localScale.y + transform.localScale.z) / 3;
        ParticleSystem.MainModule main = childVFX.main;
        main.startSizeMultiplier *= scaleAverage;
        ParticleSystem.ShapeModule shape = childVFX.shape;
        shape.scale *= scaleAverage;
        triggered = false;
    }

    private void Update()
    {
        if (playerCount > 0 && !triggered)
        {
            timer += Time.deltaTime;
            if (timer > trapDelay)
            {
                triggered = true;
                StartCoroutine(LerpVector3(childVFX.transform, Vector3.one, expansionTime));
                Invoke(nameof(Shrink), shrinkDelay);
            }
        }
        else
        {
            timer = 0;
        }
    }

    void Shrink()
    {
        if (playerCount > 0)
        {
            Invoke(nameof(Shrink), shrinkDelay);
            return;
        }
        StartCoroutine(LerpVector3(childVFX.transform, Vector3.zero, expansionTime));
        triggered = false;
    }

    IEnumerator LerpVector3(Transform transform, Vector3 target, float time)
    {
        float lerpTime = 0;
        Vector3 initialScale = transform.localScale;
        while (lerpTime < time)
        {
            transform.localScale = Vector3.Lerp(initialScale, target, lerpTime / time);
            lerpTime += Time.deltaTime;
            yield return null;
        }
        transform.localScale = target;
    }
}