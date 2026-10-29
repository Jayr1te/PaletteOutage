using UnityEngine;

public class EnemyBase : MonoBehaviour
{
    public bool aggro;   // whether or not the enemy is aggroed
    public int health;   // how many hits it takes to kill the enemy
    public int damage;   // how much damage the enemy deals on hit
    public int speed;
    public Vector2 direction;   // direction vector for movement
    public Rigidbody2D rb;
    public SpriteRenderer sprite;
    public Animator animator;

    public virtual void AggroResponse()
    {
        
    }

    public virtual void AggroLeave()
    {

    }
}
