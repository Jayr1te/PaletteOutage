using UnityEngine;

public class Projectile : MonoBehaviour
{
    protected Vector2 direction;
    protected Rigidbody2D rb;
    protected Collider2D hitbox;
    protected SpriteRenderer sprite;

    public int speed;
    public int damage;
    public GameObject target;

    protected void Startup()
    {
        rb = GetComponent<Rigidbody2D>();
        hitbox = GetComponent<Collider2D>();
        sprite = GetComponent<SpriteRenderer>();
    }

    public virtual void TargetLocked(GameObject obj)
    {
        target = obj;
    }
}
