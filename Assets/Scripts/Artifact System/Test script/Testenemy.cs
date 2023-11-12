using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Testenemy : MonoBehaviour, IDamagable
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void IDamagable.TakeDamage(float damage)
    {
        Debug.Log("Damage taken:" + damage);
    }
}
