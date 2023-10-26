using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArtifactStatus2 : MonoBehaviour
{
    GameObject player;
    [SerializeField] float increaseDamage;
    float defaultDamage;
    void Start()
    {
        player = GameObject.FindObjectOfType<Player>().gameObject;
        defaultDamage = player.GetComponent<Player>().playerWeapon.DamageModifier;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject)
        {
            player.GetComponent<Player>().playerWeapon.DamageModifier += increaseDamage;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject)
        {
            player.GetComponent<Player>().playerWeapon.DamageModifier = defaultDamage;
        }
    }
}
