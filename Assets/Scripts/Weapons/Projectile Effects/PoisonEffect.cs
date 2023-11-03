using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoisonEffect : MonoBehaviour, IProjectileEffect
{

    float duration = 5;
    float damagePerTick;
    IDamagable target;
    GameObject targetGameObject;
    private GameObject poisonedVFX;

    public void ApplyEffect(IDamagable damagable, float damage, List<IProjectileEffect> projectileEffects)
    {
        target = damagable;
        targetGameObject = target.gameObject;
        damagePerTick = (damage / duration) / 2;
        StartCoroutine(PoisonTarget(targetGameObject, target));
        Debug.Log("COOOOOOOOOCK");
    }

    public List<Type> EffectsToRemove()
    {
        return new List<Type> { };
    }

    IEnumerator PoisonTarget(GameObject targetGameObject, IDamagable target)
    {
        float elapsedTime = 0;
        GameObject vfx = Instantiate(poisonedVFX, targetGameObject.transform.position, targetGameObject.transform.rotation);
        vfx.transform.SetParent(targetGameObject.transform, true);
        while (elapsedTime < duration)
        {
            if (targetGameObject != null)
            {
                float damageThisFrame = damagePerTick * Time.deltaTime;
                target.TakeDamage(damageThisFrame);
            }
            else
            {
                elapsedTime = 0;
                Destroy(vfx);
                yield break;
            }
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        elapsedTime = 0;
        Destroy(vfx);
    }

    public  void FetchVFX(GameObject vfx)
    {
        poisonedVFX = vfx;
    }
}
