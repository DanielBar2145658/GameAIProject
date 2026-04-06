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

    public enum ActionState {IDLE, WORKING}
    ActionState state = ActionState.IDLE;

    Node.Status treeStatus = Node.Status.RUNNING;
    private Coroutine currentBoost;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        tree = new BehaviourTree();

      
        Leaf goToPowerUp = new Leaf("Go to power-up", GoToPowerUp);
        Leaf goToFinishLine = new Leaf("Go to finish line", GoToFinishLine);

     
        Selector root = new Selector("Root Selector");
        root.AddChild(goToPowerUp);
        root.AddChild(goToFinishLine);

        tree.AddChild(root);
    }

    void Update()
    {
        treeStatus = tree.Process();
        CheckForPowerUpPickup();

    
        if (targetPowerUp == null && state != ActionState.IDLE)
            state = ActionState.IDLE;
    }
    
    Node.Status GoToPowerUp()
    {
        if (targetPowerUp == null || !targetPowerUp.activeInHierarchy)
        {
            targetPowerUp = FindClosestPowerUp();
            if (targetPowerUp == null) return Node.Status.FAILURE;
        }
        
        float distToPower = Vector3.Distance(transform.position, targetPowerUp.transform.position);
        float distToFinish = Vector3.Distance(transform.position, finishLine.transform.position);
        if (distToPower >= distToFinish) return Node.Status.FAILURE;

        return GoToLocation(targetPowerUp.transform.position);
    }

    Node.Status GoToFinishLine()
    {
        return GoToLocation(finishLine.transform.position);
    }

    Node.Status GoToLocation(Vector3 destination)
    {
        float distance = Vector3.Distance(transform.position, destination);
        
        if (IsCarDangerous())
        {
            agent.isStopped = true;
            return Node.Status.RUNNING;
        }
        else
        {
            agent.isStopped = false;
        }

        if (agent.destination != destination)
            agent.SetDestination(destination);

        if (distance < 1.5f)
        {
            state = ActionState.IDLE;
            return Node.Status.SUCCESS;
        }

        state = ActionState.WORKING;
        return Node.Status.RUNNING;
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

    GameObject FindClosestPowerUp()
    {
        GameObject[] powerUps = GameObject.FindGameObjectsWithTag("PowerUp");
        GameObject closest = null;
        float minDist = Mathf.Infinity;

        foreach (GameObject p in powerUps)
        {
            if (!p.activeInHierarchy) continue;
            float dist = Vector3.Distance(transform.position, p.transform.position);
            if (dist < minDist)
            {
                minDist = dist;
                closest = p;
            }
        }

        return closest;
    }

    public void ApplySpeedBoost(float multiplier, float duration)
    {
        if (currentBoost != null) StopCoroutine(currentBoost);
        currentBoost = StartCoroutine(SpeedBoostRoutine(multiplier, duration));
    }

    private IEnumerator SpeedBoostRoutine(float multiplier, float duration)
    {
        agent.speed *= multiplier;
        yield return new WaitForSeconds(duration);
        agent.speed /= multiplier;
    }
    
    bool IsCarDangerous()
    {
        RaycastHit hit;

        
        Vector3 dir = agent.velocity.normalized;

        if (dir == Vector3.zero)
            dir = transform.forward; // fallback

        if (Physics.Raycast(transform.position + Vector3.up * 0.5f, dir, out hit, 3f))
        {
            if (hit.collider.CompareTag("Car"))
            {
                return true;
            }
        }

        return false;
    }
}