using System;
using System.Data;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{

    private float gravity = 15.0f;

    private Rigidbody2D rb;
    private BoxCollider2D boxCollider;
    private Animator animator;

    private float direction = 1.0f;
    private float maxFallSpeed = -20.0f;
    private float maxSpeed = 6.0f;
    private float groundSpeed = 0.0f;
    private int state = 0; // 0 = grounded, 1 = airborne

    private InputAction moveAction;
    private InputAction jumpAction;
    private InputAction attackAction;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Get Rigidbody2D and BoxCollider2D and store them in easy to access variables
        // This is to make code cleaner hopefully
        rb = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();
        animator = GetComponent<Animator>();

        moveAction = InputSystem.actions.FindAction("Move");
        jumpAction = InputSystem.actions.FindAction("Jump");
        attackAction = InputSystem.actions.FindAction("Attack");
    }

    // Update is called once per frame
    void Update()
    {
        animator.SetBool("StartAttack",false);

        switch (state)
        {
            case 0: // grounded

                maxSpeed = 6.0f;
                animator.SetBool("OnFloor",true);

                rb.linearVelocityY = 0.0f;

                if (direction == 1 && groundSpeed < maxSpeed)
                {
                    groundSpeed += moveAction.ReadValue<float>() * 12.0f * Time.deltaTime;
                    if (groundSpeed < 0)
                    {
                        groundSpeed += moveAction.ReadValue<float>() * 12.0f * Time.deltaTime;
                    }
                }

                if (direction == -1 && groundSpeed > -maxSpeed)
                {
                    groundSpeed += moveAction.ReadValue<float>() * 12.0f * Time.deltaTime;
                    if (groundSpeed > 0)
                    {
                        groundSpeed += moveAction.ReadValue<float>() * 12.0f * Time.deltaTime;
                    }
                }

                if (Mathf.Abs(moveAction.ReadValue<float>()) < 0.25)
                {
                    if (groundSpeed > 0)
                    {
                        groundSpeed += -1 * 24.0f * Time.deltaTime;
                    }

                    if (groundSpeed < 0)
                    {
                        groundSpeed += 1 * 24.0f * Time.deltaTime;
                    }

                    if (Mathf.Abs(groundSpeed) < 0.1)
                    {
                        groundSpeed = 0;
                    }
                }

                rb.linearVelocityX = groundSpeed;

                var snap = snapFloor();

                if (moveAction.ReadValue<float>() != 0.0)
                {
                    direction = Mathf.Sign(moveAction.ReadValue<float>());
                }

                if (!snap)
                {
                    state = 1;
                    break;
                }

                if (attackAction.WasPressedThisFrame() && RoomManager.Instance.canAttack)
                {
                    animator.SetBool("StartAttack",true);
                }

                if (jumpAction.WasPressedThisFrame())
                {
                    rb.linearVelocityY += 10.0f;
                    state = 1;
                    break;
                }

                
                break;
            
            case 1: // airborne

                maxSpeed = 6.0f;
                animator.SetBool("OnFloor",false);
                maxFallSpeed = -20.0f;
                rb.linearVelocityY -= gravity * Time.deltaTime; // Apply gravity

                if (moveAction.ReadValue<float>() != 0.0)
                {
                    direction = Mathf.Sign(moveAction.ReadValue<float>());
                }

                if (direction == 1 && rb.linearVelocityX < maxSpeed)
                {
                    rb.linearVelocityX += moveAction.ReadValue<float>() * 6.0f * Time.deltaTime;
                    if (rb.linearVelocityX < 0)
                    {
                        rb.linearVelocityX += moveAction.ReadValue<float>() * 6.0f * Time.deltaTime;
                    }
                }

                if (direction == -1 && rb.linearVelocityX > -maxSpeed)
                {
                    rb.linearVelocityX += moveAction.ReadValue<float>() * 6.0f * Time.deltaTime;
                    if (rb.linearVelocityX > 0)
                    {
                        rb.linearVelocityX += moveAction.ReadValue<float>() * 6.0f * Time.deltaTime;
                    }
                }

                if (Mathf.Abs(moveAction.ReadValue<float>()) < 0.25)
                {

                    if (Mathf.Abs(rb.linearVelocityX) < 0.25)
                    {
                        rb.linearVelocityX = 0;
                    }
                }

                if (IsOnFloor() && rb.linearVelocityY <= 0)
                {
                    groundSpeed = rb.linearVelocityX;
                    state = 0;
                }

                if (rb.linearVelocityY < maxFallSpeed)
                {
                    rb.linearVelocityY = maxFallSpeed;
                }

                if (attackAction.WasPressedThisFrame() && RoomManager.Instance.canAttack)
                {
                    animator.SetBool("StartAttack",true);
                }

                if (jumpAction.WasPressedThisFrame() && RoomManager.Instance.canGlide)
                {
                    rb.linearVelocityY = 5.0f;
                    state = 2;
                    break;
                }

                if(rb.linearVelocityY < 0 && checkWall() && RoomManager.Instance.canWallJump)
                {
                    state = 3;
                }

                break;
            
            case 2: // gliding

                maxSpeed = 8.0f;
                animator.SetBool("OnFloor",false);
                maxFallSpeed = -2.0f;
                rb.linearVelocityX += 8.0f * Time.deltaTime * direction;
                rb.linearVelocityY -= gravity * Time.deltaTime; // Apply gravity

                if (rb.linearVelocityX > 8.0f)
                {
                    rb.linearVelocityX = 8.0f;
                }

                if (rb.linearVelocityX < -8.0f)
                {
                    rb.linearVelocityX = -8.0f;
                }

                if (rb.linearVelocityX < 0.0f && direction == 1)
                {
                    rb.linearVelocityX += 8.0f * Time.deltaTime * direction;
                }

                if (rb.linearVelocityX > 0.0f && direction == -1)
                {
                    rb.linearVelocityX += 8.0f * Time.deltaTime * direction;
                }

                if (moveAction.ReadValue<float>() != 0.0)
                {
                    direction = Mathf.Sign(moveAction.ReadValue<float>());
                }

                if (IsOnFloor() && rb.linearVelocityY <= 0)
                {
                    groundSpeed = rb.linearVelocityX;
                    state = 0;
                }

                if (rb.linearVelocityY < maxFallSpeed)
                {
                    rb.linearVelocityY = maxFallSpeed;
                }

                if (attackAction.WasPressedThisFrame() && RoomManager.Instance.canAttack)
                {
                    animator.SetBool("StartAttack",true);
                    state = 1;
                }

                if(rb.linearVelocityY < 0 && checkWall() && RoomManager.Instance.canWallJump)
                {
                    state = 3;
                }

                break;
            
            case 3: // wallsliding

                maxSpeed = 6.0f;
                animator.SetBool("OnFloor",false);
                maxFallSpeed = -2.5f;
                rb.linearVelocityY -= gravity * Time.deltaTime; // Apply gravity

                if (moveAction.ReadValue<float>() != 0.0)
                {
                    direction = Mathf.Sign(moveAction.ReadValue<float>());
                }

                if (direction == 1 && rb.linearVelocityX < maxSpeed)
                {
                    rb.linearVelocityX += moveAction.ReadValue<float>() * 6.0f * Time.deltaTime;
                    if (rb.linearVelocityX < 0)
                    {
                        rb.linearVelocityX += moveAction.ReadValue<float>() * 6.0f * Time.deltaTime;
                    }
                }

                if (direction == -1 && rb.linearVelocityX > -maxSpeed)
                {
                    rb.linearVelocityX += moveAction.ReadValue<float>() * 6.0f * Time.deltaTime;
                    if (rb.linearVelocityX > 0)
                    {
                        rb.linearVelocityX += moveAction.ReadValue<float>() * 6.0f * Time.deltaTime;
                    }
                }

                if (Mathf.Abs(moveAction.ReadValue<float>()) < 0.25)
                {

                    if (Mathf.Abs(rb.linearVelocityX) < 0.25)
                    {
                        rb.linearVelocityX = 0;
                    }
                }

                if (rb.linearVelocityY < maxFallSpeed)
                {
                    rb.linearVelocityY = maxFallSpeed;
                }

                if (!checkWall())
                {
                    state = 1;
                }

                if (IsOnFloor() && rb.linearVelocityY <= 0)
                {
                    groundSpeed = rb.linearVelocityX;
                    state = 0;
                }

                if (jumpAction.WasPressedThisFrame())
                {
                    rb.linearVelocityY = 7.5f;
                    rb.linearVelocityX = 8.0f * -direction;
                    direction = -direction;
                    state = 1;
                    break;
                }

                break;

        }

        transform.localScale = new Vector3(direction,1.0f,1.0f); // If moving left, then the player is flipped horizontally
        animator.SetFloat("xVel",rb.linearVelocityX);
        animator.SetFloat("yVel",rb.linearVelocityY);

    }

    private bool checkWall()
    {
        // Raycast downward to check for ground
        RaycastHit2D cast = Physics2D.BoxCast(
            boxCollider.bounds.center,
            boxCollider.bounds.size,
            0.0f,
            Vector2.right * direction,
            0.05f,
            64
        );

        if (cast.collider != null)
        {
            return true;
        }
        return false;
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
            0.15f,
            64
        );

        if (cast.collider != null)
        {
            return true;
        }
        return false;

    }

    private bool snapFloor()
    {
        RaycastHit2D cast = Physics2D.BoxCast(
            boxCollider.bounds.center,
            boxCollider.bounds.size,
            0.0f,
            Vector2.down,
            0.15f,
            64
        );

        if (cast.collider != null)
        {
            rb.transform.position = cast.centroid;
            return true;
        }
        return false;
    }
}
