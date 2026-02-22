using UnityEngine;

public class Sequence: Node
{
    public Sequence(string n)
    {
        name = n;
    
    }

    public override Status Process()
    {
        Status childSatus = children[currentChild].Process();
        if (childSatus == Status.RUNNING) return Status.RUNNING;
        else if (childSatus == Status.FAILURE) return childSatus;

        currentChild++;
        if (currentChild >= children.Count) 
        {
            currentChild = 0;
            return Status.SUCCESS;
        }
        return Status.RUNNING;
    }

}
