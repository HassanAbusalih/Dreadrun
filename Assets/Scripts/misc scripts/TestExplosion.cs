using UnityEngine;

public class TestExplosion : MonoBehaviour
{
    [SerializeField] Transform origin;
    [SerializeField] float damage = 0;
    [SerializeField] float explosionRadius = 5;
    [SerializeField] float explosionForce = 700;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            Explosion.Explode(origin, damage, explosionRadius, explosionForce, ForceMode.Impulse);
            Debug.Log("Impulse");
        }

        if (Input.GetKeyDown(KeyCode.O))
        {
            Explosion.Explode(origin, damage, explosionRadius, explosionForce, ForceMode.Acceleration);
            Debug.Log("Acceleration");
        }

        if (Input.GetKeyDown(KeyCode.M))
        {
            Explosion.Explode(origin, damage, explosionRadius, explosionForce, ForceMode.VelocityChange);
            Debug.Log("VelocityChange");
        }

        if (Input.GetKeyDown(KeyCode.U))
        {
            Explosion.Explode(origin, damage, explosionRadius, explosionForce, ForceMode.Force);
            Debug.Log("Force");
        }
    }
}