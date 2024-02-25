using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldKiller : MonoBehaviour
{
    PlayerShield shield;
    [SerializeField] GameObject dummy;
    // Start is called before the first frame update
    void Start()
    {
        shield = GetComponent<PlayerShield>();
    }

    // Update is called once per frame
    void Update()
    {
        if(shield.shieldHP <= 0) 
        {
            Destroy(dummy);
        }
    }
}
