using UnityEngine;

public class RoboGoombaBehaviour : EnemyBase
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Startup();
        direction = Vector2.right;
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

    public override void AggroResponse()
    {
        base.AggroResponse();
        animator.Play("RoboGoombaAggroed");
    }

    public override void AggroLeave()
    {
        base.AggroLeave();
        animator.Play("RoboGoombaPassive");
    }
}
