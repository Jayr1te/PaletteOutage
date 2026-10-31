using UnityEngine;

public class EnemyBase : MonoBehaviour
{
    public bool aggro = false;   // whether or not the enemy is aggroed
    public int health;   // how many hits it takes to kill the enemy
    public int damage;   // how much damage the enemy deals on hit
    public int speed;
    public Vector2 direction;   // direction vector for movement
    public Rigidbody2D rb;
    public SpriteRenderer sprite;
    public Animator animator;

    // sets references to components common to every enemy
    public virtual void Startup()
    {
        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }

    // deals damage to an enemy and kills it if health is low enough
    public virtual void Die(int dmg)
    {
        health -= dmg;

        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }

    // defines what to do in response to player entering aggro range
    public virtual void AggroResponse()
    {
        aggro = true;
    }

    // defines what to do in response to player leaving aggro range
    public virtual void AggroLeave()
    {
        aggro = false;
    }

    // a function (should probably be defined elsewhere) to check if two values are
    // "close enough" to count as equivalent
    public bool CloseEnough(float a, float b)
    {
        if (a >= b - 0.05 && a <= b + 0.05)
        {
            return true;
        }

        return false;
    }

    // how to respond to various triggers, such as being attacked
    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "PlayerFireAttack")
        {
            Die(1);
        }
    }
}
