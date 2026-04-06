using UnityEngine;

public class Traffic : MonoBehaviour
{
    [Header("Movement")]
    public float speed = 5f;

    // Use +1 for right, -1 for left
    public int direction = 1;

    [Header("Bounds (World X)")]
    public float minX = -30f;
    public float maxX = 30f;

    void Update()
    {
       
        float move = direction * speed * Time.deltaTime;
        transform.position += new Vector3(move, 0f, 0f);

        float x = transform.position.x;

        
        if (direction > 0 && x > maxX)
        {
            transform.position = new Vector3(minX, transform.position.y, transform.position.z);
        }
        else if (direction < 0 && x < minX)
        {
            transform.position = new Vector3(maxX, transform.position.y, transform.position.z);
        }
    }
}