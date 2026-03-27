public class Selector : Node
{
    public Selector(string name) : base(name) { }

    public override Status Process()
    {
        foreach (Node child in children)
        {
            Status s = child.Process();
            if (s == Status.SUCCESS || s == Status.RUNNING)
                return s;
        }
        return Status.FAILURE;
    }
}