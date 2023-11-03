using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeleteVFX : MonoBehaviour
{
    void Start()
    {
        Invoke("DestroyThis", 1f);
    }

    void DestroyThis()
    {
        Destroy(gameObject);
    }
}
