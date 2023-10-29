using UnityEngine;

public class SpriteBillboarding : MonoBehaviour
{
    Vector3 startingScale;

    void Start()
    {
        startingScale = transform.localScale;
    }

    void Update()
    {
        Vector3 dir = Camera.main.transform.position - transform.position;

        // Calculate the rotation required to face the camera
        Quaternion rotation = Quaternion.LookRotation(dir);

        // Apply the rotation while maintaining the original local scale
        transform.rotation = rotation;
        transform.localScale = startingScale;
    }
}