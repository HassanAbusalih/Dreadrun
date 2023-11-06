using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class PayloadUI : MonoBehaviour
{
    private PayloadPath payloadPath;
    private int previousWayPointIndex;

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
    [SerializeField][Range(1, 10)] private float rangeIndicatorColorIntensity; // Glow intensity of the ring color
    private int lastState = -1; // The last state of the payload, used to check if the state has changed

    [Header("Progress Bar Settings")]
    [SerializeField] private Image pathProgressBar;
    [SerializeField] private GameObject checkpointMarkerPrefab;
    [SerializeField] private RectTransform progressBarTransform;
    [SerializeField] Transform payloadTransform;
    float totalPathLength;

    private void Start()
    {
        payloadPath = FindObjectOfType<PayloadPath>();

        GameManager.Instance.onPhaseChange.AddListener(EnablePayloadUI);

        // Calculate total path length
        for (int i = 0; i < payloadPath.pathNodes.Count - 1; i++)
        {
            totalPathLength += Vector3.Distance(payloadPath.pathNodes[i].position, payloadPath.pathNodes[i + 1].position);
        }

        // Instantiate checkpoint markers
        for (int i = 0; i < payloadPath.pathNodes.Count; i++)
        {
            if (payloadPath.pathNodes[i].tag == "Checkpoint")
            {
                float checkpointPosition = CalculatePathLengthUpToWaypoint(i) / totalPathLength;
                InstantiateCheckpointMarker(checkpointPosition);
            }
        }

        gameObject.SetActive(false);
    }

    private void Update()
    {
        healthBar.fillAmount = PayloadStats.instance.payloadHealth / PayloadStats.instance.maxPayloadHealth;

        UpdateRangeIndicatorScale();

        // Calculate length of path travelled and update progress bar
        float traveledPathLength = CalculatePathLengthUpToWaypoint(previousWayPointIndex);
        if (previousWayPointIndex < payloadPath.pathNodes.Count - 1 && previousWayPointIndex > 0)
            traveledPathLength += Vector3.Distance(payloadTransform.position, payloadPath.pathNodes[previousWayPointIndex].position);

        pathProgressBar.fillAmount = traveledPathLength / totalPathLength;
    }

    private void LateUpdate()
    {
        LookAtCamera(healthUI);
    }

    public void ChangePayloadStateDisplay(int state)
    {
        payloadStateDecal.material.SetTexture("Base_Map", payloadStatesTextures[state]);

        switch (state)
        {
            case 0:
                UpdateRangeIndicatorColor(rangeIndicatorStopColor, rangeIndicatorColorIntensity);
                break;

            case 1:
            case 2:
            case 3:
                UpdateRangeIndicatorColor(rangeIndicatorForwardColor, rangeIndicatorColorIntensity);
                break;

            case 4:
                UpdateRangeIndicatorColor(rangeIndicatorReverseColor, rangeIndicatorColorIntensity);
                break;
        }

    }
    private void UpdateRangeIndicatorScale()
    {
        // Update the scale of the range indicator to match the payload's range
        rangeIndicator.transform.localScale = new Vector3(PayloadStats.instance.payloadRange / transform.localScale.x, PayloadStats.instance.payloadRange / transform.localScale.y, PayloadStats.instance.payloadRange / transform.localScale.z);
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

    private void InstantiateCheckpointMarker(float position)
    {
        GameObject checkpointMarker = Instantiate(checkpointMarkerPrefab);
        checkpointMarker.transform.SetParent(progressBarTransform, false);

        RectTransform checkpointMarkerRectTransform = checkpointMarker.GetComponent<RectTransform>();
        checkpointMarkerRectTransform.anchorMin = new Vector2(0.5f, position);
        checkpointMarkerRectTransform.anchorMax = new Vector2(0.5f, position);
        checkpointMarkerRectTransform.pivot = new Vector2(0.5f, 0.5f);
        checkpointMarkerRectTransform.anchoredPosition = new Vector2(0, 0);
    }

    private void UpdateRangeIndicatorColor(Color color, float ringGlowIntensity)
    {
        rangeIndicatorDecal.material.SetColor("_Color", new Color(color.r * ringGlowIntensity, color.g * ringGlowIntensity, color.b * ringGlowIntensity, 1));
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