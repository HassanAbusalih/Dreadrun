using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PayloadUI : MonoBehaviour
{
    private PayloadStats payloadStats;
    private PayloadPath payloadPath;
    private int NextWayPointIndex;

    [Header("Health Bar Settings")]
    [SerializeField] private Image healthBar;

    [Header("Payload State Settings")]
    [SerializeField] private Image payloadStateImage;
    [SerializeField] private Sprite[] payloadStatesSprites;

    [Header("Range Indicator Settings")]
    [SerializeField] private MeshRenderer rangeIndicatorRingRenderer;
    [SerializeField] private MeshRenderer rangeIndicatorFillRenderer;
    [SerializeField] private Material[] rangeIndicatorFillMaterials;
    [SerializeField] private Material[] rangeIndicatorRingMaterials;

    [Header("Progress Bar Settings")]
    [SerializeField] private Image pathProgressBar;
    [SerializeField] private GameObject checkpointMarkerPrefab;
    [SerializeField] private RectTransform progressBarTransform;
    [SerializeField] Transform payloadTransform;
    float totalPathLength;

    private void Start()
    {
        payloadStats = FindObjectOfType<PayloadStats>();
        payloadPath = FindObjectOfType<PayloadPath>();


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
    }

    private void Update()
    {
        healthBar.fillAmount = payloadStats.payloadHealth / payloadStats.maxPayloadHealth;

        // Calculate length of path travelled and update progress bar
        float traveledPathLength = CalculatePathLengthUpToWaypoint(NextWayPointIndex);
        if (NextWayPointIndex < payloadPath.pathNodes.Count - 1)
            traveledPathLength += Vector3.Distance(payloadTransform.position, payloadPath.pathNodes[NextWayPointIndex].position);

        pathProgressBar.fillAmount = traveledPathLength / totalPathLength;
    }

    private void LateUpdate()
    {
        LookAtCamera();
    }

    public void ChangePayloadStateDisplay(int state)
    {
        if (payloadStateImage.sprite != payloadStatesSprites[state])
        {
            payloadStateImage.sprite = payloadStatesSprites[state];

            switch (state)
            {
                case 0:
                    rangeIndicatorFillRenderer.material = rangeIndicatorFillMaterials[0];
                    rangeIndicatorRingRenderer.material = rangeIndicatorRingMaterials[0];
                    break;

                case 1:
                case 2:
                case 3:
                    rangeIndicatorFillRenderer.material = rangeIndicatorFillMaterials[1];
                    rangeIndicatorRingRenderer.material = rangeIndicatorRingMaterials[1];
                    break;

                case 4:
                    rangeIndicatorFillRenderer.material = rangeIndicatorFillMaterials[2];
                    rangeIndicatorRingRenderer.material = rangeIndicatorRingMaterials[2];
                    break;
            }
        }

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

    public void UpdateLastWayPointIndex(int index)
    {
        NextWayPointIndex = index;
    }

    public void LookAtCamera()
    {
        transform.LookAt(transform.position + Camera.main.transform.rotation * Vector3.forward, Camera.main.transform.rotation * Vector3.up);
    }
}