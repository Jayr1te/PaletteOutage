using UnityEngine;
using UnityEngine.SceneManagement;

public class RoomTransition : MonoBehaviour
{

    public int targetScene = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D other)
    {

        print("a");

        if (other.CompareTag("Player"))
        {
            SceneManager.LoadScene(targetScene);
        }
    }
}
