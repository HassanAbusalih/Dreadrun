using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathPointsAddToList : MonoBehaviour
{
    public void AddChildrenToPathPointsList(Transform pathPointParent, List<Transform> pathPoints)
    {
        if (pathPointParent == null) { return; }
        foreach (Transform child in pathPointParent)
        {
            if (pathPoints.Contains(child)) { continue; }
            pathPoints.Add(child);
        }
        pathPoints.RemoveAll(item => item == null);
    }
}
