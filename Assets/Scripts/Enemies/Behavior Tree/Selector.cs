using System;
using UnityEngine;

public class Selector : Node
{
    Node[] nodes;
    public Rigidbody body;
    public Transform playerTarget;
    public Func<Transform> closestPlayerFunction;


    public Selector(Node[] nodes, Rigidbody body, Transform player, Func<Transform> closest)
    {
        this.nodes = nodes;
        this.body = body;
        this.playerTarget = player;
        this.closestPlayerFunction = closest;
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
        body.velocity = Vector3.zero;
        Debug.Log("Transform position: " + playerTarget.position);
        Debug.Log("Func position: " + closestPlayerFunction().position);
        nodeState = NodeState.Failure;
        return nodeState;
    }
}