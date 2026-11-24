using UnityEngine;

public class UpgradeObject : MonoBehaviour
{

    public bool giveAttack = false;

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
            
            if (giveAttack)
            {
                RoomManager.Instance.canAttack = true;
            }

            Destroy(gameObject);

        }
    }
}
