
using System.Collections;
using UnityEngine;
using static UnityEngine.ParticleSystem;
using Cinemachine;

public class RotateCameraOnTrigger : MonoBehaviour
{
    [SerializeField] Transform camFollow;
    [Header("Camera Rotation Settings")]
    [SerializeField] bool useTriggerAsAngleOffset;
    [SerializeField] bool useCamStartAngleAsExit = true;
    [SerializeField] float angleOffset;
    
    [SerializeField] float rotateDuration;
    [SerializeField] MinMaxCurve rotationCurve;

    [Header("Debug Stats")]
    [SerializeField] float? previousRotationAngle = null;
    [SerializeField] float AlignmentBetweenCamAndTrigger;
    [SerializeField] float startAngle;
    float previosuXDamping;

    [SerializeField] bool shouldDestroy;


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
            AlignmentBetweenCamAndTrigger = Vector3.Dot(transform.forward, directionFromCamera);
            StopAllCoroutines();
           
            if (AlignmentBetweenCamAndTrigger > 0.1f && previousRotationAngle==null ) // you never know when these stupid edge casses happen so i just put a small value above 0
            {
                previousRotationAngle = camFollow.transform.eulerAngles.y;
                float targetAngleOffset = useTriggerAsAngleOffset ? transform.rotation.eulerAngles.y : angleOffset;
                StartCoroutine(RotateCamera(targetAngleOffset));
                if (shouldDestroy)
                {
                    Destroy(this.gameObject, rotateDuration + 1f);
                }
                return;
            }
            float resetAngle = useCamStartAngleAsExit ? startAngle : (float)previousRotationAngle;
            StartCoroutine(RotateCamera(resetAngle));
            previousRotationAngle = null;

        }

    }


  

    IEnumerator RotateCamera(float? angle)
    {
        float timer = 0;
        float randomValue = rotationCurve.mode == ParticleSystemCurveMode.TwoConstants ? Random.value : 0;
        float currentAngle = camFollow.transform.eulerAngles.y;
      
        while (timer < rotateDuration)
        {
            timer += Time.deltaTime;
            float t = rotationCurve.Evaluate(timer / rotateDuration, randomValue);
            camFollow.transform.eulerAngles = new Vector3(camFollow.transform.eulerAngles.x, Mathf.LerpAngle(currentAngle, (float)angle, t), camFollow.transform.eulerAngles.z);
            yield return null;
        }
    }
}
