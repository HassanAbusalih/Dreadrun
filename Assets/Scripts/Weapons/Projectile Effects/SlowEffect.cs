using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowEffect : MonoBehaviour, IProjectileEffect
{
    float slowModifier;
    float slowDuration;
    GameObject slowVFX;

    public bool ApplyOnCollision { get; } = false;
    public void Initialize(float slowModifier, float slowDuration, GameObject slowVFX)
    { 
        this.slowModifier = slowModifier; 
        this.slowDuration = slowDuration;
        this.slowVFX = slowVFX;
    }

    public void ApplyEffect(IDamagable damagable, float damage, List<IProjectileEffect> projectileEffects)
    {
        StartCoroutine(Slow(damagable.gameObject));
    }

    public List<Type> EffectsToRemove()
    {
        throw new NotImplementedException();
    }

    IEnumerator Slow(GameObject gameObject)
    {
        if (gameObject.TryGetComponent(out ISlowable slowable) && !slowable.slowed)
        {
            slowable.ApplySlow(slowModifier);
            GameObject vfx = Instantiate(slowVFX, gameObject.transform.position, gameObject.transform.rotation, gameObject.transform);
            Vector3 parentScale = gameObject.transform.localScale;
            vfx.transform.localScale = new Vector3(1 / parentScale.x, 1 / parentScale.y, 1 / parentScale.z);
            yield return new WaitForSeconds(slowDuration);
            if (gameObject != null)
            {
                slowable.RemoveSlow(slowModifier);
                Destroy(vfx);
            }
        }
        else { yield return null;}
    }

    public void ApplyEffect(Transform transform, float damage, List<IProjectileEffect> effects)
    {
        throw new NotImplementedException();
    }
}