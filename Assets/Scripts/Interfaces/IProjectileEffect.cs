using System.Collections.Generic;

public interface IProjectileEffect
{
    public void ApplyEffect(IDamagable damagable, float damage, List<IProjectileEffect> projectileEffects);

    List<System.Type> EffectsToRemove();
}