using System.Collections.Generic;
using UnityEngine;

public class DamageOverTime : MonoBehaviour
{
    [SerializeField] float tickRate = 0.5f;
    [SerializeField] float minDamagePerSecond = 5f;
    [SerializeField] float maxDamagePerSecond = 10f;
    [SerializeField] int ticksToMaxDamage = 3;
    Dictionary<GameObject, (IDamagable damagable, float timer, int damageTicks)> damageableDictionary = new();

    private void Update()
    {
        ApplyDamage(damageableDictionary);
    }

    protected void ApplyDamage(Dictionary<GameObject, (IDamagable damagable, float timer, int damageTicks)> damageableDictionary)
    {
        List<GameObject> keysToUpdate = new(damageableDictionary.Keys);
        foreach (GameObject target in keysToUpdate)
        {
            (IDamagable damagable, float timer, int damageTicks) = damageableDictionary[target];
            timer -= Time.deltaTime;
            if (timer <= 0)
            {
                if (target != null)
                {
                    float damage = Mathf.Lerp(minDamagePerSecond, maxDamagePerSecond, (float)damageTicks / ticksToMaxDamage);
                    damagable.TakeDamage(damage * tickRate);
                    if (damageTicks < ticksToMaxDamage) { damageTicks++; }
                }
                else
                {
                    damageableDictionary.Remove(target);
                    continue;
                }
                timer = tickRate;
            }
            damageableDictionary[target] = (damagable, timer, damageTicks);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out IDamagable damagable) && !damageableDictionary.ContainsKey(other.gameObject))
        {
            damageableDictionary.Add(other.gameObject, (damagable, 0, 0));
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out IDamagable _))
        {
            damageableDictionary.Remove(other.gameObject);
        }
    }
}