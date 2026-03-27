using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class Behaviour : MonoBehaviour
{
    BehaviourTree tree;

    public GameObject finishLine;

    private GameObject targetPowerUp;

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
        if (targetPowerUp == null || !targetPowerUp.activeInHierarchy)
        {
            targetPowerUp = null;
        }

        if (ShouldGetPowerUp())
        {
            return GoToLocation(targetPowerUp.transform.position);
        }

        return GoToLocation(finishLine.transform.position);
    }
   
    public Node.Status FindPowerUp()
    {

        return GetPowerUp(targetPowerUp.transform.position);
    }


    Node.Status GoToLocation(Vector3 destination) 
    {
        float distanceToTarget = Vector3.Distance(destination, transform.position);
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
        float distanceToTarget = Vector3.Distance(destination, transform.position);
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


    

    void Update()
    {
        treeStatus = tree.Process();

        CheckForPowerUpPickup();
        
        if (targetPowerUp == null && state != ActionState.IDLE)
        {
            state = ActionState.IDLE;
        }
    }
    
    private Coroutine currentBoost;

    public void ApplySpeedBoost(float multiplier, float duration)
    {
        if (currentBoost != null)
        {
            StopCoroutine(currentBoost);
        }

        currentBoost = StartCoroutine(SpeedBoostRoutine(multiplier, duration));
    }

    private IEnumerator SpeedBoostRoutine(float multiplier, float duration)
    {
        agent.speed *= multiplier;

        yield return new WaitForSeconds(duration);

        agent.speed /= multiplier;
    }
    
    bool ShouldGetPowerUp()
    {
        targetPowerUp = FindClosestPowerUp();

        if (targetPowerUp == null) return false;

        float distToPower = Vector3.Distance(transform.position, targetPowerUp.transform.position);
        float distToFinish = Vector3.Distance(transform.position, finishLine.transform.position);
        
        return distToPower < distToFinish;
    }
    
    GameObject FindClosestPowerUp()
    {
        GameObject[] powerUps = GameObject.FindGameObjectsWithTag("PowerUp");

        GameObject closest = null;
        float minDistance = Mathf.Infinity;

        foreach (GameObject p in powerUps)
        {
            if (!p.activeInHierarchy) continue;

            float dist = Vector3.Distance(transform.position, p.transform.position);

            if (dist < minDistance)
            {
                minDistance = dist;
                closest = p;
            }
        }

        return closest;
    }
    
    void CheckForPowerUpPickup()
    {
        if (targetPowerUp == null) return;

        float distance = Vector3.Distance(transform.position, targetPowerUp.transform.position);

        if (distance < 1.5f)
        {
            ApplySpeedBoost(2f, 2f);
            targetPowerUp.SetActive(false);
            
            targetPowerUp = null;
            state = ActionState.IDLE;
        }
    }
}
