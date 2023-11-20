using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

public class Payload : PathPointsAddToList, IDamagable
{
    [Header("Stats")]
    [SerializeField] float moveSpeed;
    [SerializeField] float health;
    [Header("Events")]
    public UnityEvent stopPayload;
    public UnityEvent StartPayload;

    [Header("Path Points transforms")]
    public Transform pathPointsParent;
    public List<Transform> pathPointsList = new List<Transform>();
    int currentPathIndex;

    [SerializeField] bool followPath;


    private void OnValidate()
    {
        AddChildrenToPathPointsList(pathPointsParent, pathPointsList );
    }
    void Start()
    {

    }

    void FixedUpdate()
    {
        if (followPath && currentPathIndex < pathPointsList.Count)
        {
            Transform targetPoint = pathPointsList[currentPathIndex];
            transform.position = Vector3.MoveTowards(transform.position, targetPoint.position, moveSpeed * Time.deltaTime);
            transform.LookAt(targetPoint);
            if (Vector3.Distance(transform.position, targetPoint.position) < 0.1f)
            {
                currentPathIndex++;
            }
        }
    }

    public void TakeDamage(float amount)
    {
        health--;
        if (health < 0)
        {
            GameManager.Instance.Lose();
        }
    }

    public void StartFollowingPath()
    {
        followPath = true;
    }

    public void StopFollowingPath()
    {
        followPath = false;
    }

}

#if UNITY_EDITOR
[CustomEditor(typeof(Payload))]
public class PayloadEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        Payload pathPoints = (Payload)target;

        if (GUILayout.Button("Update Path Points List"))
        {
            pathPoints.AddChildrenToPathPointsList(pathPoints.pathPointsParent, pathPoints.pathPointsList);
        }
    }
}
#endif
