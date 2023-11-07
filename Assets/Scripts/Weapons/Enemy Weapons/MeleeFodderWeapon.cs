using UnityEngine;
using System;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class MeleeFodderWeapon : WeaponBase
{
    [SerializeField] float knockbackForce = 50f;
    public Action hit;
    bool swinging;

    public override void Attack()
    {
        Attack(fireRate);
    }

    public void Attack(float duration)
    {
        swinging = true;
        Invoke(nameof(Deactivate), duration);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (swinging && collision.gameObject.TryGetComponent(out IDamagable damagable))
        {
            damagable.TakeDamage(damageModifier);
            collision.gameObject.GetComponent<Rigidbody>().AddForce(transform.parent.forward * knockbackForce * 100);
            hit.Invoke();
            swinging = false;
        }
    }

    void Deactivate()
    {
        swinging = false;
    }
}
#if UNITY_EDITOR
[CustomEditor(typeof(MeleeFodderWeapon))]
public class MeleeFodderWeaponEditor : Editor
{
    public override void OnInspectorGUI()
    {
        MeleeFodderWeapon weapon = (MeleeFodderWeapon)target;
        serializedObject.Update();
        DrawPropertiesExcluding(serializedObject,
            "fireRate",
            "projectileRange",
            "projectileSpeed",
            "spreadAngle",
            "projectilePrefab",
            "m_Script"
        );
        serializedObject.ApplyModifiedProperties();
    }
}
#endif