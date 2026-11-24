using UnityEngine;

public class Projectile : MonoBehaviour
{
    protected Vector2 direction;
    protected Rigidbody2D rb;
    protected Collider2D hitbox;
    protected SpriteRenderer sprite;
    protected Animator animator;
    protected GameObject target;

    public int speed;
    public int damage;

    protected void Startup()
    {
        rb = GetComponent<Rigidbody2D>();
        hitbox = GetComponent<Collider2D>();
        sprite = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }

    public virtual void TargetLocked(GameObject obj)
    {
        target = obj;
    }

    public virtual void ReceiveDirection(Vector2 dir)
    {
        direction = dir;
        rb.linearVelocity = direction * speed;
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Ground" || other.tag == "FireBreakable")
        {
            Destroy(gameObject);
        } else if (other.tag == "Player")
        {
            other.SendMessage("ProjectileHit", damage);
            Destroy(gameObject);
        }
    }
}
