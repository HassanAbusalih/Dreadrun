
using System.Collections;
using UnityEngine;

public class LookAtPayload : MonoBehaviour
{
    [Header("References")]
    [SerializeField] Transform parent;
    [SerializeField] Transform player;
    [SerializeField] Sprite blankSprite;
    [SerializeField] Sprite arrowSprite;
    [SerializeField] Sprite warningSprite;
    [SerializeField] Sprite emergencySprite;

    [Header("Look At Settings")]
    [SerializeField] float distanceToShow;
    [SerializeField] Vector3 offset;
    [SerializeField] KeyCode toggleKey;

    [Header("Animation Settings")]
    [SerializeField] Color loopingColor;
    [SerializeField] AnimationCurve scaleCurve;
    [SerializeField] float frequency;
    [SerializeField] float amplitude;

    [Header("Sprite Display Settings")]
    [SerializeField] float spriteSwitchInterval;
    [SerializeField] float displayDuration;
    [SerializeField] float hideDuration;
    float lastSwitchTime;
    float lastDisplayTime;

    [Header("Debug")]
    [SerializeField] float distanceFromPayload;
    [SerializeField] bool arrowActivated = false;

    Payload payload;
    PayloadFeedback payloadFeedback;
    SpriteRenderer indicator;
    Teleport teleport;

    private void OnEnable()
    {
        teleport = FindObjectOfType<Teleport>();
        if (teleport != null)
        {
            teleport.OnTimerOver += ToggleArrow;
            arrowActivated = false;
        }
    }

    void OnDisable()
    {
        if (teleport != null)
        {
            teleport.OnTimerOver -= ToggleArrow;
        }
    }

    private void OnValidate()
    {
        transform.localPosition = offset;
    }

    void Start()
    {
        payload = FindObjectOfType<Payload>();
        indicator = GetComponent<SpriteRenderer>();
        payloadFeedback = payload.GetComponent<PayloadFeedback>();
        StartCoroutine(ArrowBehavior());

        if (payload != null)
        {
            distanceToShow += payload.InteractionRange;
        }

        if (toggleKey == KeyCode.None) toggleKey = KeyCode.Q; 
    }

    void Update()
    {
        if (Input.GetKeyDown(toggleKey))
        {
            if (Time.time <= lastDisplayTime + displayDuration)
            {
                lastDisplayTime -= displayDuration;
            }
            else if (Time.time <= lastDisplayTime + displayDuration + hideDuration)
            {
                lastDisplayTime -= displayDuration + hideDuration;
            }
        }
    }

    void ToggleArrow()
    {
        arrowActivated = true;
    }

    void DirectSpriteToPayload()
    {
        parent.position = player.position;
        transform.localPosition = offset;
        parent.LookAt(payload.transform);
    }

    void AnimateArrow()
    {
        float _scaleTime = scaleCurve.Evaluate(Mathf.Sin(frequency * Time.time) * amplitude);
        transform.localScale = Vector3.Lerp(Vector3.one * 1.5f, Vector3.one, _scaleTime);
        //indicator.color = Color.Lerp(Color.white, loopingColor, _scaleTime);
    }

    IEnumerator ArrowBehavior()
    {
        while (true)
        {
            distanceFromPayload = Vector3.Distance(player.position, payload.transform.position);

            if (!arrowActivated || distanceFromPayload < distanceToShow)
            {
                indicator.sprite = blankSprite;
                yield return null;
                continue;
            }

            if (Time.time <= lastDisplayTime + displayDuration)
            {
                DirectSpriteToPayload();
                AnimateArrow();

                if (Time.time > (lastSwitchTime + spriteSwitchInterval))
                {
                    lastSwitchTime = Time.time;
                    UpdateIndicatorSprite();
                }
            }
            else if (Time.time <= lastDisplayTime + displayDuration + hideDuration)
            {
                indicator.sprite = blankSprite;
            }
            else
            {
                lastDisplayTime = Time.time;
            }

            yield return null;
        }
    }

    void UpdateIndicatorSprite()
    {
        switch (payloadFeedback.PayloadState)
        {
            case PayloadState.Normal:
                indicator.sprite = arrowSprite;
                break;
            case PayloadState.Contested:
                indicator.sprite = indicator.sprite == arrowSprite ? warningSprite : arrowSprite;
                break;
            case PayloadState.Stopped:
                indicator.sprite = indicator.sprite == arrowSprite ? emergencySprite : arrowSprite;
                break;
        }
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
