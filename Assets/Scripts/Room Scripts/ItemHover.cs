using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemHover : MonoBehaviour
{
    public float hoverHeight = 1.0f;
    public float hoverSpeed = 1.0f;
    private Vector3 initialPosition;

    void Start()
    {
        initialPosition = transform.position;
    }

    void Update()
    {
        float newY = initialPosition.y + Mathf.Sin(Time.time * hoverSpeed) * hoverHeight;

        // Update the object's position.
        transform.position = new Vector3(transform.position.x, newY, transform.position.z);
    }
}
