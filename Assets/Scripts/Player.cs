using System.Data;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{

    public float gravity = 7.5f;

    private Rigidbody2D rb;
    private BoxCollider2D boxCollider;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Get Rigidbody2D and BoxCollider2D and store them in easy to access variables
        // This is to make code cleaner hopefully
        rb = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        // Using the old input system for right now as it is faster to implement for testing
        // Will hopefully be replaced by a better input system once stuff is further along

        // Handle horzontal movement
        if (Input.GetKey(KeyCode.LeftArrow)) // Left Arrow to move left
        {
            rb.linearVelocityX = -4.0f;
        } 
        else if (Input.GetKey(KeyCode.RightArrow)) // Right Arrow to move right
        {
            rb.linearVelocityX = 4.0f;
        }
        else // Don't move horzontally at all if nothing is pressed
        {
            rb.linearVelocityX = 0;
        }

        if (IsOnFloor()) // If the player is on the floor
        {
            if (rb.linearVelocityY < 0.0f) // Set vertical velocity to 0 if on the ground
            {
                rb.linearVelocityY = 0.0f;
            }

            if (Input.GetKeyDown(KeyCode.Z)) // Press Z to Jump
            {
                rb.linearVelocityY += 7.5f;
            }

        } else // If the player is not on the floor
        {
            rb.linearVelocityY -= gravity * Time.deltaTime; // Apply gravity
        }

    }

    // Checks if the Player is on the floor
    private bool IsOnFloor()
    {

        // Raycast downward to check for ground
        RaycastHit2D cast = Physics2D.BoxCast(
            boxCollider.bounds.center,
            boxCollider.bounds.size,
            0.0f,
            Vector2.down,
            0.025f,
            64
        );

        if (cast.collider != null)
        {
            return true;
        }
        return false;

    }
}
