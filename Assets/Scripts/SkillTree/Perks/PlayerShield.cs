using UnityEngine;

public class PlayerShield : MonoBehaviour, IDamagable
{
    [SerializeField] public float shieldHP;
    [SerializeField] public float maxShieldHP;

    private void Start()
    {
        shieldHP = maxShieldHP;
    }
    public void TakeDamage(float Amount)
    {
        shieldHP -= Amount;
        shieldHP = Mathf.Max(shieldHP, 0f);
    }
}
