using UnityEngine;

public class FinishLine : MonoBehaviour
{
    [SerializeField] GameObject winText;
    [SerializeField] GameObject loseText;
    [SerializeField] GameObject restartButton;

    bool gameEnded = false;

    private void OnTriggerEnter(Collider other)
    {
        if (gameEnded) return;

        if (other.CompareTag("Player"))
        {
            EndGame(true);
        }
        else if (other.CompareTag("AI"))
        {
            EndGame(false);
        }
    }

    void EndGame(bool playerWon)
    {
        gameEnded = true;

        if (playerWon)
        {
            winText.SetActive(true);
        }
        else
        {
            loseText.SetActive(true);
        }

        restartButton.SetActive(true);

        Time.timeScale = 0f;
    }
    
    public void PlayerDied()
    {
        if (!gameEnded)
        {
            EndGame(false);
        }
    }
}
