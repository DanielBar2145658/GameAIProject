using UnityEngine;
using UnityEngine.SceneManagement;

public class Restart : MonoBehaviour
{
    [SerializeField] GameObject startMenu;

    void Start()
    {
        
        Time.timeScale = 0f;

       
        startMenu.SetActive(true);
    }

    public void StartGame()
    {
        
        startMenu.SetActive(false);

        
        Time.timeScale = 1f;
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
