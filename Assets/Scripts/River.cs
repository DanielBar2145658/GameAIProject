using UnityEngine;

public class River : MonoBehaviour
{
    public float speed = 3f;
    public int direction = -1;

    public float minX = -29f;
    public float maxX = 29f;

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