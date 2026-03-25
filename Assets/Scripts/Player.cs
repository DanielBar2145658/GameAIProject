using UnityEngine;

public class Player : MonoBehaviour
{
    public float moveSpeed;
    private Rigidbody rigidBody;
    private Animator animator;

    void Awake()
    {
        rigidBody = GetComponent<Rigidbody>();

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
}
