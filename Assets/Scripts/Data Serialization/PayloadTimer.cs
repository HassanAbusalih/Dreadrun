using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PayloadTimer : MonoBehaviour
{
    public float elapsedTime;

    void Update()
    {
        
    }

    public void StartPayloadTimer()
    {
        elapsedTime += Time.deltaTime;
    }
}
