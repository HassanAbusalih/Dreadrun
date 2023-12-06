using UnityEngine;

public class NewCheckpoint : MonoBehaviour
{
    [SerializeField] float timer;
    [SerializeField] float stopDuration;
    [SerializeField] bool enteredLastCheckpoint;
    [SerializeField] Payload payload;

    private void Start()
    {
        timer = 0;
        payload = FindAnyObjectByType<Payload>();
    }
    private void FixedUpdate()
    {
        if (enteredLastCheckpoint)
        {
            timer = Time.deltaTime;
            if (timer >= stopDuration)
            {
                payload.StartFollowingPath();
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PayloadPath"))
        {
            enteredLastCheckpoint = true;
        }
    }
}
