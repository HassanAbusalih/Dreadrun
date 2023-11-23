using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Payload : MonoBehaviour , IDamagable
{

    [Header("Stats")]
    [SerializeField] float speed;
    float currentSpeed;
    [SerializeField] float maxSpeed = 5f;
    [SerializeField] float rotationSpeed = 100f;
    [SerializeField] public float maxhealth = 100f;
    public float health = 100f;
    [SerializeField] float healAmount = 0.5f;
    [SerializeField] float healingInterval;
    [SerializeField] bool followPath = false;
    [SerializeField] float slowdownRange = 10;
    [SerializeField][Range(0.1f, 0.9f)] float slowSpeed = 0.5f;
    private float timer = 0.0f;

    [SerializeField] GameObject visualEffects;

    [Header("Path Points transforms")]
    public Transform grandparentTransform;
    public List<Transform> pathPointsParent = new List<Transform>();
    public List<Transform> pathPointsList = new List<Transform>();
    int currentPathIndex;
    int currentParentIndex = 0;
    Player player;
    PayloadFeedback feedback;

    private void OnValidate()
    {
        AddToList(grandparentTransform, pathPointsParent);
        AddToList(pathPointsParent[currentParentIndex], pathPointsList);
    }

    void Start()
    {
        player = FindObjectOfType<Player>();
        feedback = GetComponent<PayloadFeedback>();
        visualEffects.SetActive(false);
        speed = maxSpeed;
        health = maxhealth;
    }

    void FixedUpdate()
    {
        if (followPath)
        {
            currentSpeed = speed;
            if (EnemiesInRange())
            {
                currentSpeed *= slowSpeed;
                feedback.ChangeColor(currentSpeed, maxSpeed);
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
                StopFollowingPath();
                currentParentIndex++;
                currentPathIndex = 0;
                if (currentParentIndex > pathPointsParent.Count - 1)
                {
                    GameManager.Instance.Win();
                    return;
                }
                AddToList(pathPointsParent[currentParentIndex], pathPointsList);
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
        if (EnemyPool.Instance == null)
        {
            return false;
        }

        foreach (var enemy in EnemyPool.Instance.Enemies)
        {
            if (enemy == null) continue;
            if (Vector3.Distance(transform.position, enemy.transform.position) < slowdownRange)
            {
                return true;
            }
        }
        return false;
    }

    public void TakeDamage(float amount)
    {
        health--;
        //feedback.UpdateHealth(health);
        if (health < 0)
        {
            GameManager.Instance.Lose();
        }
    }

    public void StartFollowingPath()
    {
        followPath = true;
        feedback.SetColor(Color.green);
        visualEffects.SetActive(true);
    }

    public void StopFollowingPath()
    {
        visualEffects.SetActive(false);
        feedback.SetColor(Color.red);
        followPath = false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, slowdownRange);
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.GetComponent<Player>())
        {
            StartHealthing();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<Player>())
        {
            StopHealing();
        }
    }

    private void StartHealthing()
    {
        timer = 0.0f;
        InvokeRepeating("IncreaseHealth", timer, healingInterval);
    }

    private void StopHealing()
    {
        CancelInvoke("IncreaseHealth");
    }

    private void IncreaseHealth()
    {
        player.ChangeHealth(healAmount);
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