using UnityEngine;

public class PowerUp : MonoBehaviour
{
    public float speedMultiplier = 2f;
    public float duration = 2f;

    private void OnTriggerEnter(Collider other)
    {
        // Player
        Player player = other.GetComponent<Player>();
        if (player != null)
        {
            player.ApplySpeedBoost(speedMultiplier, duration);
            gameObject.SetActive(false);
            return;
        }

        // AI 
        Behaviour ai = other.GetComponent<Behaviour>();
        if (ai != null)
        {
            ai.ApplySpeedBoost(speedMultiplier, duration);
            gameObject.SetActive(false);
            return;
        }
    }
}