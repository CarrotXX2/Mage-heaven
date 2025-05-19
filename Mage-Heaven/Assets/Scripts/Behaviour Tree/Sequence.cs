using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace BehaviourTree
{
    public class Sequence : Node
    {
        public override NodeState Evaluate()
        {
            bool anyChildIsRunning = false;

            foreach (var node in children)
            {
                switch (node.Evaluate())
                {
                     case NodeState.FAILURE:
                         state = NodeState.FAILURE;
                         return state;
                     case NodeState.SUCCESS:
                         state = NodeState.SUCCESS;
                         return state;
                     case NodeState.RUNNING:
                         state = NodeState.RUNNING;
                         return state;
                     default:
                         state = NodeState.SUCCESS;
                         return state;
                }
            }

            return NodeState.FAILURE;
        }
    }
}
