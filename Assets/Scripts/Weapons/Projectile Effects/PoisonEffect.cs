using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoisonEffect : MonoBehaviour, IProjectileEffect
{
    float duration = 5;
    float tickRate = 0.5f;
    float damagePerTick;
    IDamagable target;
    GameObject targetGameObject;
    private GameObject poisonedVFX;

    public bool ApplyOnCollision { get; } = false;

    public void ApplyEffect(IDamagable damagable, float damage, List<IProjectileEffect> projectileEffects)
    {
        target = damagable;
        targetGameObject = target.gameObject;
        damagePerTick = damage / (duration / tickRate);
        StartCoroutine(PoisonTarget(targetGameObject, target));
    }

    public List<Type> EffectsToRemove()
    {
        return new List<Type> { };
    }

    IEnumerator PoisonTarget(GameObject targetGameObject, IDamagable target)
    {
        float elapsedTime = 0;
        GameObject vfx = Instantiate(poisonedVFX, targetGameObject.transform.position, targetGameObject.transform.rotation, targetGameObject.transform);
        Vector3 parentScale = targetGameObject.transform.localScale;
        vfx.transform.localScale = new Vector3(1 / parentScale.x, 1 / parentScale.y, 1 / parentScale.z);
        while (elapsedTime < duration)
        {
            if (targetGameObject != null)
            {
                float damageThisFrame = damagePerTick * Time.deltaTime;
                target.TakeDamage(damageThisFrame);
            }
            else
            {
                Destroy(vfx);
                yield break;
            }
            elapsedTime += tickRate; 
            yield return new WaitForSeconds(tickRate);
            yield return null;
        }
        Destroy(vfx);
    }

    public void FetchVFX(GameObject vfx)
    {
        poisonedVFX = vfx;
    }

    public void ApplyEffect(Transform transform, float damage, List<IProjectileEffect> effects)
    {
        throw new NotImplementedException();
    }
}
