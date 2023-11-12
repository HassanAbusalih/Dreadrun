
using UnityEngine;

public class LookAtPayload : MonoBehaviour
{
    [Header("References")]
    [SerializeField] Transform parent;
    [SerializeField] Transform player;

    [Header("Settings")]
    [SerializeField] float distanceToShow;
    [SerializeField] Vector3 offset;
    [SerializeField] Color loopingColor;
    [SerializeField] AnimationCurve scaleCurve;
    [SerializeField] float frequency;
    [SerializeField] float amplitude;


    [Header("Debug")]
    [SerializeField] float distanceFromPayload;

    PayloadStats payload;
    bool isShowing = false;
    SpriteRenderer arrowSprite;


    private void OnValidate()
    {
        transform.localPosition = offset;
    }

    void Start()
    {
        payload = FindObjectOfType<PayloadStats>();
        arrowSprite = GetComponent<SpriteRenderer>();

        if (payload != null)
        {
            distanceToShow += payload.payloadRange;
        }
        if (arrowSprite != null)
        {
            arrowSprite.enabled = false;
        }
    }

    void Update()
    {
        if (payload == null || arrowSprite == null || player == null) return;

        float _distanceFromPayload = Vector3.Distance(player.position, payload.transform.position);
        distanceFromPayload = _distanceFromPayload;

        if (_distanceFromPayload > distanceToShow)
        {
            isShowing = true;
            arrowSprite.enabled = true;
        }
        else if (_distanceFromPayload <= payload.payloadRange)
        {
            if (!isShowing) return;
            isShowing = false;
            arrowSprite.enabled = false;
        }

        if (isShowing)
        {
            DirectSpriteToPayload();
            animateArrow();
        }
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
