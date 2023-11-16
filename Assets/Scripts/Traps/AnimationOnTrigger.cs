using UnityEngine;

public class AnimationOnTrigger : Trap
{
    [SerializeField] string animatorBool = "";
    Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }
    private void Update()
    {
        if (playerCount > 0 && !triggered)
        {
            triggered = true;
            Invoke(nameof(TriggerAnimation), trapDelay);
        }
    }

    void TriggerAnimation()
    {
        animator.SetBool(animatorBool, true);
    }

    public void ResetTrap()
    {
        animator.SetBool(animatorBool, false);
        triggered = false;
    }
}