using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArtifactStatus2 : MonoBehaviour
{
    Player player;
    [SerializeField] float increaseDamage;
    float defaultDamage;
    void Start()
    {
        player = FindObjectOfType<Player>();
        if (player.playerWeapon != null)
        {
            defaultDamage = player.playerWeapon.DamageModifier;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject)
        {
            player.playerWeapon.DamageModifier += increaseDamage;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject)
        {
            player.playerWeapon.DamageModifier = defaultDamage;
        }
    }
}
