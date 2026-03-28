using UnityEngine;

public class FihnishLine : MonoBehaviour
{
    [SerializeField]
    GameObject winText;
    [SerializeField]
    GameObject loseText;

    bool LineCrossed = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && LineCrossed == false)
        {
            LineCrossed = true;
            winText.SetActive(true);


        }
        else if (other.CompareTag("AI") && LineCrossed == false) 
        {
            LineCrossed = true;
            loseText.SetActive(true);

        }
    }


}
