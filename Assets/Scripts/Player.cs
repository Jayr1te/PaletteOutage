using System.Data;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class Player : MonoBehaviour
{

    public float gravity = 7.5f;

    private Rigidbody2D rb;
    private BoxCollider2D boxCollider;
    private Animator animator;
    public GameObject camera;

    private float direction = 1.0f;
    private float floorAngle = 0.0f;
    private bool onFloor = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Get Rigidbody2D and BoxCollider2D and store them in easy to access variables
        // This is to make code cleaner hopefully
        rb = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        // Using the old input system for right now as it is faster to implement for testing
        // Will hopefully be replaced by a better input system once stuff is further along

        // Handle horzontal movement
        if (Input.GetKey(KeyCode.LeftArrow)) // Left Arrow to move left
        {
            rb.linearVelocity = 6.0f * Vector2.left.Rotate(-floorAngle);
            direction = -1.0f;
        } 
        else if (Input.GetKey(KeyCode.RightArrow)) // Right Arrow to move right
        {
            rb.linearVelocity = 6.0f * Vector2.right.Rotate(-floorAngle);
            direction = 1.0f;
        }
        else // Don't move horzontally at all if nothing is pressed
        {
            rb.linearVelocity = Vector2.zero;
        }

        if (Input.GetKeyDown(KeyCode.X)) // Attack
        {
            animator.SetBool("StartAttack",true);
        }
        else
        {
            animator.SetBool("StartAttack",false);
        }

        if (onFloor) // If the player is on the floor
        {
            animator.SetBool("OnFloor",true);
            //rb.linearVelocityY = 0.0f;


            floorAngle = getFloorSlope();
            transform.rotation = Quaternion.Euler(0,0,-floorAngle);
            SnapFloor();

            if (Input.GetKeyDown(KeyCode.Z)) // Press Z to Jump
            {
                rb.linearVelocityY += 7.5f;
                onFloor = false;
            }

        } else // If the player is not on the floor
        {
            animator.SetBool("OnFloor",false);
            rb.linearVelocityY -= gravity * Time.deltaTime; // Apply gravity
            floorAngle = 0.0f;
            transform.rotation = Quaternion.Euler(0,0,0);
            onFloor = IsOnFloor();
        }

        transform.localScale = new Vector3(direction,1.0f,1.0f); // If moving left, then the player is flipped horizontally
        animator.SetFloat("xVel",rb.linearVelocityX);
        animator.SetFloat("yVel",rb.linearVelocityY);

        camera.gameObject.transform.rotation = Quaternion.Euler(0,0,0);

    }

    // Checks if the Player is on the floor
    private bool IsOnFloor()
    {

        // Raycast downward to check for ground
        RaycastHit2D cast = Physics2D.BoxCast(
            boxCollider.bounds.center,
            boxCollider.bounds.size,
            -floorAngle,
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

    private void SnapFloor()
    {

        var angleRad = floorAngle * 2 * Mathf.PI;
        Vector2 direction = new Vector2(Mathf.Cos(-angleRad),Mathf.Sin(-angleRad));

        // Raycast downward to check for ground
        RaycastHit2D cast = Physics2D.BoxCast(
            boxCollider.bounds.center,
            boxCollider.bounds.size,
            -floorAngle,
            direction,
            1.5f,
            64
        );

        if (cast.collider != null)
        {
            print("yay");
            transform.position = cast.centroid;
        }
    }

    private float getFloorSlope()
    {
        RaycastHit2D cast = Physics2D.Raycast(
            boxCollider.bounds.center,
            Vector2.down,
            1.25f,
            64
        );

        if (cast.collider != null)
        {
            var angle = Vector2.Angle(cast.normal, Vector2.up);
            return angle;
        }

        return 0.0f;
    }

}
