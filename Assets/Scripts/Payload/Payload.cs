using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

public class Payload : MonoBehaviour, IDamagable
{
    [Header("Stats")]
    [SerializeField] float speed;
    [SerializeField] float maxSpeed;
    [SerializeField] float health;
    [Header("Events")]
    public UnityEvent stopPayload;
    public UnityEvent StartPayload;

    [Header("Path Points transforms")]
    public Transform grandparentTransform;
    public List<Transform> pathPointsParent = new List<Transform>();
    public List<Transform> pathPointsList = new List<Transform>();
    int currentPathIndex;
    int currentParentIndex = 0;

    [SerializeField] bool followPath;


    private void OnValidate()
    {
        AddToList(grandparentTransform, pathPointsParent);
        AddToList(pathPointsParent[0], pathPointsList);
    }
    void Start()
    {
        
    }

    void FixedUpdate()
    {
        if (followPath)
        {
            if (currentPathIndex < pathPointsList.Count)
            {
                Transform targetPoint = pathPointsList[currentPathIndex];
                transform.position = Vector3.MoveTowards(transform.position, targetPoint.position, speed * Time.deltaTime);
                transform.LookAt(targetPoint);
                if (Vector3.Distance(transform.position, targetPoint.position) < 0.1f)
                {
                    currentPathIndex++;
                }
            }
            else
            {
                followPath = false;
                currentParentIndex++;
                AddToList(pathPointsParent[currentParentIndex],pathPointsList);
            }
        }

    }

    void AddToList(Transform parent, List<Transform> children)
    {
       children.Clear();
       PathPointsAddToList.AddChildrenToPathPointsList(parent, children);
       followPath = true;
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
            PathPointsAddToList.AddChildrenToPathPointsList(pathPoints.pathPointsParent[0], pathPoints.pathPointsList);
        }
    }
}
#endif
