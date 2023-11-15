using UnityEngine;

public class ExpandingTrap : Trap
{
    [SerializeField] float expansionTime = 3f;
    [SerializeField] float shrinkDelay = 10f;
    GameObject childVFX;
    bool triggered = false;

    private void Start()
    {
        childVFX = GetComponentInChildren<ParticleSystem>().gameObject;
    }

    private void Update()
    {
        if (playerCount > 0 && !triggered)
        {
            timer += Time.deltaTime;
            if (timer > trapDelay)
            {
                triggered = true;
                childVFX.transform.LeanScale(Vector3.one, expansionTime);
                Invoke("Shrink", shrinkDelay);
            }
        }
        else
        {
            timer = 0;
        }
    }

    void Shrink()
    {
        childVFX.transform.LeanScale(Vector3.zero, expansionTime);
        triggered = false;
    }
}