using UnityEngine;

public class AggroTrigger : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            SendMessageUpwards("AggroResponse");
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            SendMessageUpwards("AggroLeave");
        }
    }
}
