using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GizmoVisualizer : MonoBehaviour
{
    bool showGizmos = true;
    [SerializeField] Color gizmoMeshColor = Color.red;
    [SerializeField] Color gizmoWireMeshColor = Color.white;
    [SerializeField] Vector3 gizmoScaleAddon;


    private void OnDrawGizmos()
    {
        if (!showGizmos) { return; }

        Vector3 gizmoScale = transform.localScale + gizmoScaleAddon;
        Gizmos.color = gizmoMeshColor;
        Gizmos.DrawSphere(transform.position,gizmoScale.y);

        Gizmos.color = gizmoWireMeshColor;
        Gizmos.DrawWireSphere(transform.position, gizmoScale.y);
  
    }
}
