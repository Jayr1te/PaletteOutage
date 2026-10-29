using UnityEngine;

public class RoboGoombaBehaviour : EnemyBase
{
    public GameObject player;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        direction = Vector2.right;
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (aggro)
        {
            direction = Vector2.Normalize(new Vector2(player.transform.position.x - transform.position.x, 0.0f));
        } else
        {
            if (rb.linearVelocity == Vector2.zero)
            {
                direction.x = -direction.x;
            }
        }
    }

    void FixedUpdate()
    {
        if (rb.linearVelocityY == 0)
        {
            rb.linearVelocityX = speed * direction.x;
        } else
        {
            rb.linearVelocityX = 0;
        }
    }

    public override void Die()
    {
        Destroy(gameObject);
    }

    public override void AggroResponse()
    {
        animator.Play("RoboGoombaAggroed");
        aggro = true;
    }

    public override void AggroLeave()
    {
        animator.Play("RoboGoombaPassive");
        aggro = false;
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("trigger");
        if (other.tag == "PlayerFireAttack")
        {
            Die();
        }
    }
}
