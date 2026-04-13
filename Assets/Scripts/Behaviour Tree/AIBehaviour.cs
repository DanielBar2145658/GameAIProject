using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class AIBehaviour : MonoBehaviour
{
    BehaviourTree tree;
    public GameObject finishLine;
    public NavMeshAgent agent;

    public enum ActionState { IDLE, WORKING }
    ActionState state = ActionState.IDLE;

    Node.Status treeStatus = Node.Status.RUNNING;

    private Coroutine currentBoost;

    Transform currentPlatform;
    
    private GameObject targetPowerUp;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        tree = new BehaviourTree();

        Leaf goToFinishLine = new Leaf("Go to finish line", GoToFinishLine);

        Selector root = new Selector("Root Selector");
        root.AddChild(goToFinishLine);

        tree.AddChild(root);
    }

    void Update()
    {
        treeStatus = tree.Process();
        state = ActionState.WORKING;


    }

    Node.Status GoToFinishLine()
    {
        return GoToLocation(finishLine.transform.position);
    }

    Node.Status GoToLocation(Vector3 destination)
    {
if (!HasGroundAhead())
{
    agent.isStopped = true;
    return Node.Status.RUNNING;
}
        float distance = Vector3.Distance(transform.position, destination);

        if (IsCarDangerous())
        {
            agent.isStopped = true;
            return Node.Status.RUNNING;
        }

        if (IsInWaterDanger())
        {
            GameObject log = FindNearestLog();

            if (log != null)
            {
                agent.isStopped = false;
                agent.SetDestination(log.transform.position);
            }
            else
            {
                agent.isStopped = true;
            }

            return Node.Status.RUNNING;
        }

        GameObject powerUp = FindClosestPowerUp();

        if (powerUp != null)
        {
            float distToPower = Vector3.Distance(transform.position, powerUp.transform.position);
            float distToFinish = Vector3.Distance(transform.position, finishLine.transform.position);

            if (distToPower < distToFinish)
            {
                destination = powerUp.transform.position;
            }
        }

        agent.isStopped = false;

        if (agent.destination != destination)
            agent.SetDestination(destination);

        if (distance < 1.5f)
        {
            state = ActionState.IDLE;
            return Node.Status.SUCCESS;
        }

        return Node.Status.RUNNING;
    }

    bool IsCarDangerous()
    {
        RaycastHit hit;

        Vector3 dir = agent.velocity.normalized;
        if (dir == Vector3.zero)
            dir = transform.forward;

        if (Physics.Raycast(transform.position + Vector3.up * 0.5f, dir, out hit, 3f))
        {
            return hit.collider.CompareTag("Car");
        }

        return false;
    }

    bool IsInWaterDanger()
    {
        return IsInWater() && currentPlatform == null;
    }

    bool IsInWater()
    {
        return Physics.Raycast(
            transform.position + Vector3.up * 0.5f,
            Vector3.down,
            2f,
            LayerMask.GetMask("Water")
        );
    }

    GameObject FindNearestLog()
    {
        GameObject[] logs = GameObject.FindGameObjectsWithTag("Log");

        GameObject closest = null;
        float minDist = Mathf.Infinity;

        foreach (GameObject log in logs)
        {
            float dist = Vector3.Distance(transform.position, log.transform.position);
            if (dist < minDist)
            {
                minDist = dist;
                closest = log;
            }
        }

        return closest;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Log"))
        {
            currentPlatform = other.transform; 
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Log"))
        {
            if (currentPlatform == other.transform)
                currentPlatform = null;
        }
    }

    public void ApplySpeedBoost(float multiplier, float duration)
    {
        if (currentBoost != null)
            StopCoroutine(currentBoost);

        currentBoost = StartCoroutine(SpeedBoostRoutine(multiplier, duration));
    }

    private IEnumerator SpeedBoostRoutine(float multiplier, float duration)
    {
        agent.speed *= multiplier;
        yield return new WaitForSeconds(duration);
        agent.speed /= multiplier;
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

bool HasGroundAhead()
{
    Vector3 checkPos = transform.position + transform.forward * 1.2f;

    return Physics.Raycast(
        checkPos + Vector3.up * 0.5f,
        Vector3.down,
        2f
    );
}
}