using UnityEngine;
using System;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class MeleeFodderWeapon : WeaponBase
{
    [SerializeField] float knockbackForce = 50f;
    Player player;
    public Action hit;
    bool swinging;

    private void Update()
    {
        if (player != null)
        {
            if (swinging)
            {
                player.TakeDamage(damageModifier * 0.7f);
                hit.Invoke();
                player.GetComponent<Rigidbody>().AddForce(transform.parent.forward * knockbackForce * 100);
                swinging = false;
            }
        }
    }

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
        if (collision.gameObject.TryGetComponent(out Player player))
        {
            this.player = player;
            player.TakeDamage(damageModifier * 0.3f);
            player.GetComponent<Rigidbody>().AddForce(transform.parent.forward * knockbackForce * 100);
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.TryGetComponent(out Player player))
        {
            if (this.player == player)
            {
                this.player = null;
            }
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