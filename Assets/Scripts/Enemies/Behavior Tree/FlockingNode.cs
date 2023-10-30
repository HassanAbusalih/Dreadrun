public class FlockingNode : Node
{
    FlockingBehavior flockingBehavior;

    public FlockingNode(FlockingBehavior flockingBehavior)
    {
        this.flockingBehavior = flockingBehavior;
    }

    public override NodeState Execute()
    {
        flockingBehavior.Flocking();
        return NodeState.Success;
    }
}
