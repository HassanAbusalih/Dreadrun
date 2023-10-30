public class Sequencer :Node
{
    Node[] nodes;

    public Sequencer(params Node[] nodes)
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
                case NodeState.Failure:
                    nodeState = NodeState.Failure;
                    return nodeState;
            }
        }
        nodeState = NodeState.Success;
        return nodeState;
    }
}