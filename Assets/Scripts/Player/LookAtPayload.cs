
using UnityEngine;

public class LookAtPayload : MonoBehaviour
{
    [Header("References")]
    [SerializeField] Transform parent;
    [SerializeField] Transform player;

    [Header("Settings")]
    [SerializeField] float distanceToShow;
    [SerializeField] Vector3 offset;

    [Header("Debug")]
    [SerializeField] float distanceFromPayload;

    PayloadStats payload;
    bool isShowed = false;
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
        if (payload == null || arrowSprite == null) return;

        float _distanceFromPayload = Vector3.Distance(player.position, payload.transform.position);
        distanceFromPayload = _distanceFromPayload;
        if (_distanceFromPayload > distanceToShow)
        {
            isShowed = true;
            arrowSprite.enabled = true;
        }
        else
        {
            if (!isShowed) return;
            isShowed = false;
            arrowSprite.enabled = false;
        }
        DirectSpriteToPayload();
    }

    void DirectSpriteToPayload()
    {
        if (isShowed)
        {
            parent.position = player.position;
            transform.localPosition = offset;
            parent.LookAt(payload.transform);
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
