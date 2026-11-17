using System;
using System.Data;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{

    public float gravity = 10.0f;

    private Rigidbody2D rb;
    private BoxCollider2D boxCollider;
    private Animator animator;

    private float direction = 1.0f;
    private float maxFallSpeed = -20.0f;
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

                animator.SetBool("OnFloor",true);

                rb.linearVelocityY = 0.0f;
                rb.linearVelocityX = moveAction.ReadValue<float>() * 6.0f;

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

                if (attackAction.WasPressedThisFrame())
                {
                    animator.SetBool("StartAttack",true);
                }

                if (jumpAction.WasPressedThisFrame())
                {
                    rb.linearVelocityY += 7.5f;
                    state = 1;
                    break;
                }

                
                break;
            
            case 1: // airborne

                animator.SetBool("OnFloor",false);
                maxFallSpeed = -20.0f;
                rb.linearVelocityX = moveAction.ReadValue<float>() * 6.0f;
                rb.linearVelocityY -= gravity * Time.deltaTime; // Apply gravity

                if (moveAction.ReadValue<float>() != 0.0)
                {
                    direction = Mathf.Sign(moveAction.ReadValue<float>());
                }

                if (IsOnFloor() && rb.linearVelocityY <= 0)
                {
                    state = 0;
                }

                if (rb.linearVelocityY < maxFallSpeed)
                {
                    rb.linearVelocityY = maxFallSpeed;
                }

                if (attackAction.WasPressedThisFrame())
                {
                    animator.SetBool("StartAttack",true);
                }

                if (jumpAction.WasPressedThisFrame())
                {
                    rb.linearVelocityY = 5.0f;
                    state = 2;
                    break;
                }

                break;
            
            case 2: // gliding

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
                    state = 0;
                }

                if (rb.linearVelocityY < maxFallSpeed)
                {
                    rb.linearVelocityY = maxFallSpeed;
                }

                if (attackAction.WasPressedThisFrame())
                {
                    animator.SetBool("StartAttack",true);
                    state = 1;
                }

                break;

        }

        transform.localScale = new Vector3(direction,1.0f,1.0f); // If moving left, then the player is flipped horizontally
        animator.SetFloat("xVel",rb.linearVelocityX);
        animator.SetFloat("yVel",rb.linearVelocityY);

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
