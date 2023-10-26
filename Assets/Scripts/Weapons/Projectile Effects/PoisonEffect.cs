using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoisonEffect : MonoBehaviour, IProjectileEffect
{
    float elapsedTime;
    float duration = 5;
    float damagePerTick;
    IDamagable target;

    public void ApplyEffect(IDamagable damagable, float damage, List<IProjectileEffect> projectileEffects)
    {
        target = damagable;
        damagePerTick = (damage / duration) * 2;
        StartCoroutine(PoisonTarget());
    }

    public List<Type> EffectsToRemove()
    {
        return new List<Type> { };
    }

    IEnumerator PoisonTarget()
    {
        while (elapsedTime < duration)
        {
            if (target != null)
            {
                target.TakeDamage(damagePerTick);
            }
            else
            {
                yield break;
            }
            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }
}
