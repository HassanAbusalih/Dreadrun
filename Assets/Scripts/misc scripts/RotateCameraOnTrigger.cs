
using System.Collections;
using UnityEngine;


public class RotateCameraOnTrigger : MonoBehaviour
{
    [SerializeField] Transform camFollow;
    [Header("Camera Rotation Settings")]
    [SerializeField] bool useTriggerAsAngleOffset;
    [SerializeField] bool useCamStartAngleAsExit = true;
    [SerializeField] float angleOffset;

    [SerializeField] float rotateDuration;
    [SerializeField] AnimationCurve rotationCurve;

    [Header("Debug Stats")]
    [SerializeField] float previousRotationAngle = 0;
    [SerializeField] float AlignmentBetweenCamAndTrigger;
    [SerializeField] float startAngle;

    [SerializeField] bool shouldDestroy;
    bool alreadyEntered;

    private void Start()
    {
        if (camFollow == null)
        {
            camFollow = FindObjectOfType<CameraFocus>().transform;
        }


        Invoke("GetStartAngle", 1.5f);
    }

    void GetStartAngle()
    {
        startAngle = camFollow.transform.eulerAngles.y;
    }

    private void OnValidate()
    {
        if (useTriggerAsAngleOffset)
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
            AlignmentBetweenCamAndTrigger = Vector3.Dot(directionFromCamera,transform.forward);
            StopAllCoroutines();

            if (AlignmentBetweenCamAndTrigger > 0.1f && !alreadyEntered) // you never know when these stupid edge casses happen so i just put a small value above 0
            {
                previousRotationAngle = camFollow.transform.eulerAngles.y;
                alreadyEntered = true;
                float targetAngleOffset = useTriggerAsAngleOffset ? transform.rotation.eulerAngles.y : angleOffset;
                StartCoroutine(RotateCamera(targetAngleOffset));
                if (shouldDestroy)
                {
                    Destroy(this.gameObject, rotateDuration + 1f);
                }
                return;
            }
            else
            {
                float resetAngle = useCamStartAngleAsExit ? startAngle : previousRotationAngle;
                StartCoroutine(RotateCamera(resetAngle));
                alreadyEntered = false;
            }
               
        }

    }

    IEnumerator RotateCamera(float? angle)
    {
        float timer = 0;
        float currentAngle = camFollow.transform.eulerAngles.y;

        while (timer < rotateDuration)
        {
            timer += Time.fixedDeltaTime;
            float t = rotationCurve.Evaluate(timer / rotateDuration);
            camFollow.transform.eulerAngles = new Vector3(camFollow.transform.eulerAngles.x, Mathf.LerpAngle(currentAngle, (float)angle, t), camFollow.transform.eulerAngles.z);
            yield return null;
        }
    }
}
