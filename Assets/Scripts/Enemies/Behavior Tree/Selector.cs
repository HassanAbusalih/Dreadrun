public class Selector : Node
{
    Node[] nodes;

    public Selector(params Node[] nodes)
    {
        this.nodes = nodes;
    }

    public override NodeState Execute()
    {
        foreach (Node node in nodes)
        {
            switch (node.Execute())
            {
                case NodeState.Running:
                    nodeState = NodeState.Running;
                    return nodeState;
                case NodeState.Success:
                    nodeState = NodeState.Success;
                    return nodeState;
            }
        }
        nodeState = NodeState.Failure;
        return nodeState;
    }
}