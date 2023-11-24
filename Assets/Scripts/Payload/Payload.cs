using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Payload : MonoBehaviour , IDamagable
{
    float health;
    
    [Header("Stats")]
    [SerializeField] float maxhealth = 500f;
    [SerializeField] float speed = 5f;
    [SerializeField] float rotationSpeed = 100f;
    [SerializeField] float healAmount = 0.5f;
    [SerializeField] float healingInterval = 5f;
    [SerializeField] bool followPath = false;
    [SerializeField] float interactionRange = 10f;
    [SerializeField][Range(0.1f, 0.9f)] float slowSpeed = 0.5f;

    [SerializeField] GameObject visualEffects;

    [Header("Path Points transforms")]
    [SerializeField] Transform grandParentTransform;
    public List<Transform> pathPointsParent = new();
    public List<Transform> pathPointsList = new();
    int currentPathIndex;
    int currentParentIndex = 0;
    Player[] players;
    PayloadFeedback feedback;

    private void OnValidate()
    {
        AddToList(grandParentTransform, pathPointsParent);
        if (pathPointsParent.Count > 0)
        {
            AddToList(pathPointsParent[currentParentIndex], pathPointsList);
        }
    }

    void Start()
    {
        players = FindObjectsOfType<Player>();
        StartCoroutine(HealOnTimer());
        feedback = GetComponent<PayloadFeedback>();
        visualEffects.SetActive(false);
        health = maxhealth;
    }

    void FixedUpdate()
    {
        if (followPath)
        {
           float currentSpeed = speed;
            if (EnemiesInRange())
            {
                currentSpeed *= slowSpeed;
                
                feedback.ChangeColor(currentSpeed, speed);
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
        if (parent == null) { return; }
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
            if (Vector3.Distance(transform.position, enemy.transform.position) < interactionRange)
            {
                return true;
            }
        }
        return false;
    }

    public void TakeDamage(float amount)
    {
        health--;
        feedback.UpdateHealth(health, maxhealth);
        if (health < 0)
        {
            GameManager.Instance.Lose();
        }
    }

    public void StartFollowingPath()
    {
        followPath = true;
        feedback.SetColor(Color.green);
    }

    public void StopFollowingPath()
    {
        feedback.SetColor(Color.red);
        followPath = false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, interactionRange);
    }

    IEnumerator HealOnTimer()
    {
        while (true)
        {
            if (followPath)
            {
                yield return new WaitForSeconds(healingInterval);
                foreach (var player in players)
                {
                    if (Vector3.Distance(transform.position, player.transform.position) < interactionRange)
                    {
                        if (visualEffects.activeSelf == false)
                        {
                            visualEffects.SetActive(true);
                        }
                        player.ChangeHealth(healAmount);
                    }
                }
                if (visualEffects.activeSelf == true)
                {
                    Invoke(nameof(DisableVFX), 2f);
                }
            }
            yield return null;
        }
    }

    void DisableVFX()
    {
        visualEffects.SetActive(false);
    }
}