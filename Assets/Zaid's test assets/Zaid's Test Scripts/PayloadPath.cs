using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PayloadPath : MonoBehaviour
{
    public Color pathColor = Color.green;
    public List<Transform> pathNodes = new List<Transform>();
    public Transform[] nodesContainer;

    private void OnDrawGizmos()
    {
        Gizmos.color = pathColor;
        nodesContainer = GetComponentsInChildren<Transform>();
        pathNodes.Clear();

        foreach (Transform path_Node in nodesContainer)
        {
            if (path_Node != this.transform)
            {
                pathNodes.Add(path_Node);
            }
        }

        for (int i = 0; i < pathNodes.Count; i++)
        {
            Vector3 currentNode_Pos = pathNodes[i].position;
            if (i > 0)
            {
                Vector3 prevNode_Pos = pathNodes[i - 1].position;
                Gizmos.DrawLine(prevNode_Pos, currentNode_Pos);
                Gizmos.DrawSphere(currentNode_Pos, 0.2f);
            }
        }
    }
}