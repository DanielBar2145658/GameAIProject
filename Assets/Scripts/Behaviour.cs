using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class Behaviour : MonoBehaviour
{
    BehaviourTree tree;

    public GameObject finishLine;

    public NavMeshAgent agent;

    public enum ActionState {IDLE,WORKING };
    ActionState state = ActionState.IDLE;

    Node.Status treeStatus = Node.Status.RUNNING;



    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        tree = new BehaviourTree();
        Sequence finishRace = new Sequence("Go to the finish line!");


        Leaf goToFinishLine = new Leaf("Go to Diamond", FinishRace);





        finishRace.AddChild(goToFinishLine);
        tree.AddChild(finishRace);


        
        
    }

    public Node.Status FinishRace() 
    {

        return GoToLocation(finishLine.transform.position);
    }


    Node.Status GoToLocation(Vector3 destination) 
    {
        float distanceToTarget = Vector3.Distance(destination.normalized, this.transform.position);
        if (state == ActionState.IDLE)
        {
            agent.SetDestination(destination);
            state = ActionState.WORKING;
        }
        else if (Vector3.Distance(agent.pathEndPosition, destination) >= 2)
        {

            state = ActionState.IDLE;
            return Node.Status.FAILURE;
        }
        else if (distanceToTarget < 2) 
        {
            state = ActionState.IDLE;
            return Node.Status.SUCCESS;
        }
        return Node.Status.RUNNING;
        
    }


    void Update()
    {
        if (treeStatus == Node.Status.RUNNING) 
        {
            treeStatus = tree.Process();
        
        }
    }
}
