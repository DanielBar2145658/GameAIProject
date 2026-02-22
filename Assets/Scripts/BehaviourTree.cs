using System.Collections.Generic;
using UnityEngine;

public class BehaviourTree : Node
{
    public BehaviourTree() 
    {
        name = "Tree";

    }

    struct NodeLevel 
    {
        public int level;
        public Node node;
    }

    public BehaviourTree(string n) 
    {
        name = n;
    }

    public override Status Process()
    {
        return children[currentChild].Process();
    }


    public void PrintTree() 
    {
        string treePrintout = "";
        Stack<NodeLevel> nodeStack = new Stack<NodeLevel>();
        Node currentNode = this;
        //nodeStack.Push(currentNode);
        nodeStack.Push(new NodeLevel { level = 0, node = currentNode });

        while (nodeStack.Count != 0) 
        {
            //Node nextNode = nodeStack.Pop();
            NodeLevel nextNode = nodeStack.Pop();
            //treePrintout += nextNode.name + "\n";
            treePrintout += new string('-',nextNode.level)+ nextNode.node.name;
            
            for (int i = nextNode.node.children.Count - 1; i >= 0; i--) 
            {
                //nodeStack.Push(nextNode.children[i]);
                nodeStack.Push(new NodeLevel{ level= nextNode.level + 1, node = nextNode.node.children[i]});
            }
        
        }
        Debug.Log(treePrintout);


    }

}
