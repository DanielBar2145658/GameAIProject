using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour
{
    public float moveSpeed;
    private float baseSpeed;

    private Rigidbody rigidBody;
    private Animator animator;

    private Coroutine currentBoost;

    void Awake()
    {
        rigidBody = GetComponent<Rigidbody>();
        baseSpeed = moveSpeed;
    }

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void FixedUpdate()
    {
        Move();
    }

    void Move()
    {
        float xInput = Input.GetAxis("Horizontal");
        float zInput = Input.GetAxis("Vertical");

        Vector3 dir = new Vector3(xInput, 0, zInput) * moveSpeed;

        
        dir.y = rigidBody.linearVelocity.y;
        rigidBody.linearVelocity = dir;

        Vector3 facingDir = new Vector3(xInput, 0, zInput);
        if (facingDir.magnitude > 0)
        {
            transform.forward = facingDir;
        }
    }

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
        moveSpeed = baseSpeed * multiplier;

        yield return new WaitForSeconds(duration);

        moveSpeed = baseSpeed;
    }
}
