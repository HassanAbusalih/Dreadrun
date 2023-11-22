using System.Collections.Generic;
using UnityEngine;

public interface IProjectileEffect
{
    public bool ApplyOnCollision { get; }

    public void ApplyEffect(IDamagable damagable, float damage, List<IProjectileEffect> projectileEffects);
    void ApplyEffect(Transform transform, float damage, List<IProjectileEffect> effects);
    List<System.Type> EffectsToRemove();
}