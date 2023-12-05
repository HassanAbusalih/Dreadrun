
using System.Collections;
using UnityEngine;
using static UnityEngine.ParticleSystem;

public class RotateCameraOnTrigger : MonoBehaviour
{
    [SerializeField] Transform camFollow;

    [Header("Camera Rotation Settings")]
    [SerializeField] bool useTriggerAsAngleOffset;
    [SerializeField] float angleOffset;
    [SerializeField] float rotateDuration;
    [SerializeField] MinMaxCurve rotationCurve;

    [Header("Debug Stats")]
    [SerializeField] float previousRotationAngle;
    [SerializeField] float AlignmentBetweenCamAndTrigger;

    private void OnValidate()
    {
        if(useTriggerAsAngleOffset)
        {
            angleOffset = 0;
            return;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (camFollow == null) return;
        if (other.gameObject.TryGetComponent(out Player player))
        {
            Vector3 directionFromCamera = (camFollow.transform.position - transform.position).normalized;
            AlignmentBetweenCamAndTrigger = Vector3.Dot(transform.forward, directionFromCamera);
            StopAllCoroutines();

            if (AlignmentBetweenCamAndTrigger > 0.015f) // you never know when these stupid edge casses happen so i just put a small value above 0
            {
                previousRotationAngle = camFollow.transform.eulerAngles.y;
                float targetAngleOffset = useTriggerAsAngleOffset ? transform.rotation.eulerAngles.y : angleOffset;
                StartCoroutine(RotateCamera(targetAngleOffset));
                return;
            }
            StartCoroutine(RotateCamera(previousRotationAngle));
        }
    }

    IEnumerator RotateCamera(float angle)
    {
        float timer = 0;
        float randomValue = Random.value;
        float currentAngle = camFollow.transform.eulerAngles.y;
        while (timer < rotateDuration)
        {
            timer += Time.deltaTime;
            float t = rotationCurve.Evaluate(timer / rotateDuration, randomValue);
            camFollow.transform.eulerAngles = new Vector3(camFollow.transform.eulerAngles.x, Mathf.LerpAngle(currentAngle, angle, t), camFollow.transform.eulerAngles.z);
            yield return null;
        }
    }
}
