using UnityEngine;

public class KillPlane : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            FindObjectOfType<FinishLine>().PlayerDied();
        }
    }
}
