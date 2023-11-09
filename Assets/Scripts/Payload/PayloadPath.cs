using System.Collections.Generic;
using UnityEngine;

public class PayloadPath : MonoBehaviour
{
    public Color pathColor = Color.green;
    public List<Transform> pathNodes = new List<Transform>();
    private Transform[] nodesContainer;

    private void OnDrawGizmos()
    {
        Gizmos.color = pathColor;
        nodesContainer = GetComponentsInChildren<Transform>();
        pathNodes.Clear();

        foreach (Transform pathNode in nodesContainer)
        {
            if (pathNode != transform)
            {
                pathNodes.Add(pathNode);
            }
        }

        for (int i = 0; i < pathNodes.Count; i++)
        {
            Vector3 currentNodePos = pathNodes[i].position;
            if (i > 0)
            {
                Vector3 prevNodePos = pathNodes[i - 1].position;
                Gizmos.DrawLine(prevNodePos, currentNodePos);
                Gizmos.DrawSphere(currentNodePos, 0.2f);
            }
        }
    }
}