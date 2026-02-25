using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class Behaviour : MonoBehaviour
{
    BehaviourTree tree;

    public GameObject finishLine;

    public GameObject powerUps;

    public NavMeshAgent agent;

    bool foundPower;

    public enum ActionState {IDLE,WORKING,POWER };
    ActionState state = ActionState.IDLE;

    Node.Status treeStatus = Node.Status.RUNNING;



    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        tree = new BehaviourTree();
        Sequence finishRace = new Sequence("Go to the finish line!");
        Sequence getPowerUp = new Sequence("Get Power up");

        Leaf goToFinishLine = new Leaf("Go to finish line", FinishRace);
        //Leaf goToPowerUp = new Leaf("Go to power up", );




        finishRace.AddChild(goToFinishLine);
        tree.AddChild(finishRace);


        
        
    }

    public Node.Status FinishRace() 
    {

        return GoToLocation(finishLine.transform.position);
    }
    public Node.Status FindPowerUp()
    {

        return GetPowerUp(powerUps.transform.position);
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
    Node.Status GetPowerUp(Vector3 destination) 
    {
        float distanceToTarget = Vector3.Distance(destination.normalized, this.transform.position);
        if (foundPower == true)
        {
            agent.SetDestination(destination);
            state = ActionState.POWER;
            return Node.Status.RUNNING;
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


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PowerUp") == true) 
        {

            powerUps = other.GetComponent<GameObject>();
            foundPower = true;
        
        }
    }

    void Update()
    {
        if (treeStatus == Node.Status.RUNNING) 
        {
            treeStatus = tree.Process();
        
        }
    }
}
