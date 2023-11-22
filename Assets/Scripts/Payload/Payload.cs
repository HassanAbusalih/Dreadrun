using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.Events;

public class Payload : MonoBehaviour, IDamagable
{
    [Header("Stats")]
    [SerializeField] float speed;
    [SerializeField] float rotationSpeed;
    [SerializeField] float maxSpeed;
    [SerializeField] float health;
    [Header("Events")]
    public UnityEvent stopPayload;
    public UnityEvent StartPayload;

    [Header("Path Points transforms")]
    public Transform grandparentTransform;
    public List<Transform> pathPointsParent { get; private set; } = new List<Transform>();
    public List<Transform> pathPointsList = new List<Transform>();
    int currentPathIndex;
    int currentParentIndex = 0;

    [SerializeField] bool followPath;
    [SerializeField] float slowdownRange = 10;
    [SerializeField] [Range(0.1f , 0.9f)] float slowSpeed = 0.5f;

    private void OnValidate()
    {
        AddToList(grandparentTransform, pathPointsParent);
        AddToList(pathPointsParent[currentParentIndex], pathPointsList);
    }

    void FixedUpdate()
    {
        if (followPath)
        {
            float currentSpeed = speed;
            if (EnemiesInRange())
            {
                currentSpeed *= slowSpeed;
            }
            if (currentPathIndex < pathPointsList.Count)
            {
                Transform targetPoint = pathPointsList[currentPathIndex];
                transform.position = Vector3.MoveTowards(transform.position, targetPoint.position, currentSpeed * Time.deltaTime);
                Vector3 directionToPoint = targetPoint.transform.position - transform.position;
                Quaternion targetRotation = Quaternion.LookRotation(directionToPoint);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
                if (Vector3.Distance(transform.position, targetPoint.position) < 0.1f)
                {
                    currentPathIndex++;
                }
            }
            else
            {
                followPath = false;
                currentParentIndex++;
                currentPathIndex = 0;
                if(currentParentIndex> pathPointsParent.Count)
                {
                    GameManager.Instance.Win();
                }
                AddToList(pathPointsParent[currentParentIndex],pathPointsList);
            }
        }
    }

    void AddToList(Transform parent, List<Transform> children)
    {
       children.Clear();
       PathPointsAddToList.AddChildrenToPathPointsList(parent, children);
       
    }

    bool EnemiesInRange()
    {
        if(EnemyPool.Instance == null)
        {
            return false ;
        }

        foreach(var enemy in EnemyPool.Instance.Enemies)
        {
            if (enemy == null) continue;
            if(Vector3.Distance(transform.position, enemy.transform.position) < slowdownRange)
            {
                return true;
            }
        }
        return false;
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

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, slowdownRange);
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
