using UnityEngine;
using System.Collections.Generic;

public class FlockingBehavior : MonoBehaviour
{
    [SerializeField] float neighborRadius = 10f;
    [SerializeField] float separationDistance = 2f;
    [SerializeField] LayerMask flockLayer;
    [SerializeField] float alignmentStrength = 1f;
    [SerializeField] float cohesionStrength = 1f;
    [SerializeField] float separationStrength = 1f;
    [SerializeField] float maxSteerPercentage = 1f;

    public Vector3 Flocking(float maxSpeed)
    {
        List<Transform> neighbors = GetNeighbors();
        if (neighbors.Count == 0) { return Vector3.zero; }
        Vector3 flockingForce = GetAlignment(neighbors) + GetCohesion(neighbors) + GetSeparation(neighbors);
        Vector3 steering = Vector3.ClampMagnitude(flockingForce, maxSteerPercentage * maxSpeed);
        return steering;
    }

    List<Transform> GetNeighbors()
    {
        var neighbors = new List<Transform>();
        Collider[] nearbyColliders = Physics.OverlapSphere(transform.position, neighborRadius, flockLayer);
        foreach (Collider collider in nearbyColliders)
        {
            if (collider.transform != transform)
            {
                neighbors.Add(collider.transform);
            }
        }
        return neighbors;
    }

    Vector3 GetAlignment(List<Transform> neighbors)
    {
        Vector3 alignment = Vector3.zero;
        foreach (Transform neighbor in neighbors)
        {
            alignment += neighbor.GetComponent<Rigidbody>().velocity;
        }
        return (alignment / neighbors.Count) * alignmentStrength;
    }

    Vector3 GetCohesion(List<Transform> neighbors)
    {
        Vector3 cohesion = Vector3.zero;
        foreach (Transform neighbor in neighbors)
        {
            cohesion += neighbor.position;
        }

        cohesion /= neighbors.Count;
        return (cohesion - transform.position).normalized * cohesionStrength;
    }

    Vector3 GetSeparation(List<Transform> neighbors)
    {
        Vector3 separation = Vector3.zero;
        foreach (Transform neighbor in neighbors)
        {
            float distance = Vector3.Distance(neighbor.position, transform.position);
            if (distance < separationDistance)
            {
                separation += (transform.position - neighbor.position).normalized;
            }
        }
        return separation * separationStrength;
    }
}