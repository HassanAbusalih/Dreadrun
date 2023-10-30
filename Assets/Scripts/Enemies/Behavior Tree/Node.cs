public abstract class Node
{
    public enum NodeState { Running, Success, Failure };
    public NodeState nodeState;
    public abstract NodeState Execute();
}