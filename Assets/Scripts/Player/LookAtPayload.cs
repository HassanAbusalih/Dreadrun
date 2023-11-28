
using UnityEngine;

public class LookAtPayload : MonoBehaviour
{
    [Header("References")]
    [SerializeField] Transform parent;
    [SerializeField] Transform player;

    [Header("Look At Settings")]
    [SerializeField] float distanceToShow;
    [SerializeField] Vector3 offset;
    [SerializeField] KeyCode toggleKey;

    [Header("Animation Settings")]
    [SerializeField] Color loopingColor;
    [SerializeField] AnimationCurve scaleCurve;
    [SerializeField] float frequency;
    [SerializeField] float amplitude;


    [Header("Debug")]
    [SerializeField] float distanceFromPayload;

    Payload payload;
    SpriteRenderer arrowSprite;

    bool isShowing = false;
    bool runOnce;
    bool overriddenByInput = false;



    private void OnValidate()
    {
        transform.localPosition = offset;
    }

    void Start()
    {
        payload = FindObjectOfType<Payload>();
        arrowSprite = GetComponent<SpriteRenderer>();

        if (payload != null)
        {
            distanceToShow += payload.interactionRange;
        }
        if (arrowSprite != null)
        {
            arrowSprite.enabled = false;
        }

        if (toggleKey == KeyCode.None) toggleKey = KeyCode.Q; 
    }

    void Update()
    {
        if (payload == null || arrowSprite == null || player == null) return;

        float _distanceFromPayload = Vector3.Distance(player.position, payload.transform.position);
        distanceFromPayload = _distanceFromPayload;

        isShowing = _distanceFromPayload > distanceToShow ? true : false;
        bool showArrowDynamically = isShowing && !runOnce;

        if(showArrowDynamically) EnablePayloadArrow(true);
        if(!isShowing && !overriddenByInput) EnablePayloadArrow(false);

        if(Input.GetKeyDown(toggleKey))
        {
            overriddenByInput = !overriddenByInput;
            EnablePayloadArrow(!arrowSprite.enabled);
        }

        if (isShowing || overriddenByInput)
        {
            DirectSpriteToPayload();
            animateArrow();
        }
    }


    void EnablePayloadArrow(bool _enabled)
    {
        // allows arrow to be shown dynamically
        runOnce = isShowing ? true : false; 
        arrowSprite.enabled = _enabled;
    }

    void DirectSpriteToPayload()
    {
        parent.position = player.position;
        transform.localPosition = offset;
        parent.LookAt(payload.transform);
    }

    void animateArrow()
    {
        float _scaleTime = scaleCurve.Evaluate(Mathf.Sin(frequency * Time.time) * amplitude);
        transform.localScale = Vector3.Lerp(Vector3.one * 1.5f, Vector3.one, _scaleTime);
        arrowSprite.color = Color.Lerp(Color.white, loopingColor, _scaleTime);
    }

    private void OnDrawGizmos()
    {
        if (parent == null) return;
        Gizmos.color = Color.red;
        Gizmos.DrawLine(parent.position, transform.position);
        Gizmos.DrawSphere(parent.position, 0.3f);
        Gizmos.DrawSphere(transform.position, 0.3f);
    }
}
