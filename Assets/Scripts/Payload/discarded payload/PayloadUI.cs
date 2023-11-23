using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class PayloadUI : MonoBehaviour
{
    [Header("Health Bar Settings")]
    [SerializeField] private GameObject healthUI;
    [SerializeField] private Image healthBar;

    [Header("Payload State Settings")]
    [SerializeField] private DecalProjector payloadStateDecal;
    [SerializeField] private Texture[] payloadStatesTextures;

    [Header("Range Indicator Settings")]
    [SerializeField] private GameObject rangeIndicator;
    [SerializeField] private DecalProjector rangeIndicatorDecal;
    [SerializeField] private Color rangeIndicatorForwardColor;
    [SerializeField] private Color rangeIndicatorReverseColor;
    [SerializeField] private Color rangeIndicatorStopColor;
    [SerializeField][Range(1, 10)] private float rangeIndicatorColorIntensity;

    [Header("Progress Bar Settings")]
    [SerializeField] private Image pathProgressBar;
    [SerializeField] private GameObject checkpointMarkerPrefab;
    [SerializeField] private RectTransform progressBarTransform;
    [SerializeField] private Transform payloadTransform;

    private PayloadPath payloadPath;
    private int previousWayPointIndex;
    private float totalPathLength;

    private void Start()
    {
        payloadPath = FindObjectOfType<PayloadPath>();
        if (GameManager.Instance != null)
            GameManager.Instance.onPhaseChange.AddListener(EnablePayloadUI);

        totalPathLength = CalculateTotalPathLength();
        InstantiateCheckpointMarkers();
        //gameObject.SetActive(false);
    }

    private void Update()
    {
        UpdateHealthBar();
        UpdateRangeIndicatorScale();
        UpdateProgressBar();
    }

    private void LateUpdate()
    {
        LookAtCamera(healthUI);
    }

    public void ChangePayloadStateDisplay(int state)
    {
        payloadStateDecal.material.SetTexture("Base_Map", payloadStatesTextures[state]);
        Color color = state switch
        {
            0 => rangeIndicatorStopColor,
            1 or 2 or 3 => rangeIndicatorForwardColor,
            4 => rangeIndicatorReverseColor,
            _ => Color.white
        };
        UpdateRangeIndicatorColor(color, rangeIndicatorColorIntensity);
    }

    private void UpdateHealthBar()
    {
        healthBar.fillAmount = PayloadStats.instance.payloadHealth / PayloadStats.instance.maxPayloadHealth;
    }

    private void UpdateRangeIndicatorScale()
    {
        rangeIndicator.transform.localScale = new Vector3(PayloadStats.instance.payloadRange / transform.localScale.x, PayloadStats.instance.payloadRange / transform.localScale.y, PayloadStats.instance.payloadRange / transform.localScale.z);
    }

    private void UpdateRangeIndicatorColor(Color color, float ringGlowIntensity)
    {
        rangeIndicatorDecal.material.SetColor("_Color", new Color(color.r * ringGlowIntensity, color.g * ringGlowIntensity, color.b * ringGlowIntensity, 1));
    }

    private float CalculateTotalPathLength()
    {
        float length = 0;
        for (int i = 0; i < payloadPath.pathNodes.Count - 1; i++)
        {
            length += Vector3.Distance(payloadPath.pathNodes[i].position, payloadPath.pathNodes[i + 1].position);
        }
        return length;
    }

    private float CalculatePathLengthUpToWaypoint(int waypointIndex)
    {
        float length = 0;
        for (int i = 0; i < waypointIndex; i++)
        {
            length += Vector3.Distance(payloadPath.pathNodes[i].position, payloadPath.pathNodes[i + 1].position);
        }
        return length;
    }

    private void UpdateProgressBar()
    {
        float traveledPathLength = CalculatePathLengthUpToWaypoint(previousWayPointIndex);
        if (previousWayPointIndex < payloadPath.pathNodes.Count - 1 && previousWayPointIndex > 0)
            traveledPathLength += Vector3.Distance(payloadTransform.position, payloadPath.pathNodes[previousWayPointIndex].position);

        pathProgressBar.fillAmount = traveledPathLength / totalPathLength;
    }

    private void InstantiateCheckpointMarkers()
    {
        for (int i = 0; i < payloadPath.pathNodes.Count; i++)
        {
            if (payloadPath.pathNodes[i].tag == "Checkpoint")
            {
                float checkpointPosition = CalculatePathLengthUpToWaypoint(i) / totalPathLength;
                InstantiateCheckpointMarker(checkpointPosition);
            }
        }
    }

    private void InstantiateCheckpointMarker(float position)
    {
        GameObject checkpointMarker = Instantiate(checkpointMarkerPrefab, progressBarTransform);
        RectTransform checkpointMarkerRectTransform = checkpointMarker.GetComponent<RectTransform>();
        checkpointMarkerRectTransform.anchorMin = new Vector2(0.5f, position);
        checkpointMarkerRectTransform.anchorMax = new Vector2(0.5f, position);
        checkpointMarkerRectTransform.pivot = new Vector2(0.5f, 0.5f);
        checkpointMarkerRectTransform.anchoredPosition = Vector2.zero;
    }

    public void UpdateLastWayPointIndex(int index)
    {
        previousWayPointIndex = index;
    }

    private void LookAtCamera(GameObject gameObject)
    {
        gameObject.transform.LookAt(gameObject.transform.position + Camera.main.transform.rotation * Vector3.forward, Camera.main.transform.rotation * Vector3.up);
    }

    void EnablePayloadUI()
    {
        gameObject.SetActive(true);
    }
}