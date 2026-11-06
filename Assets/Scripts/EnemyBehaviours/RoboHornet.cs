using System.Collections;
using UnityEngine;

public class RoboHornet : EnemyBase
{
    private BoxCollider2D hitbox;

    [SerializeField]
    private GameObject spawnBullet;

    public GameObject[] pathMarkers;
    public int lastMarker = 0;
    public float attackInterval;
    public float attackRange;
    public GameObject player;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Startup();
        hitbox = GetComponent<BoxCollider2D>();

        direction = Vector2.Normalize(pathMarkers[lastMarker].transform.position - transform.position);

        StartCoroutine(AttackTimer());
    }

    // Update is called once per frame
    void Update()
    {
        if (rb.linearVelocityX < 0 && sprite.flipX)
        {
            sprite.flipX = false;
            hitbox.offset = new Vector2(-hitbox.offset.x, hitbox.offset.y);
        }
        else if (rb.linearVelocityX > 0 && !sprite.flipX)
        {
            sprite.flipX = true;
            hitbox.offset = new Vector2(-hitbox.offset.x, hitbox.offset.y);
        }

        if (aggro)
        {
            float distance = Vector2.Distance(player.transform.position, transform.position);

            if (CloseEnough(distance, attackRange))
            {
                direction = Vector2.zero;
            } else if (distance > attackRange + 0.05)
            {
                direction = Vector2.Normalize(player.transform.position - transform.position);
            } else
            {
                direction = Vector2.Normalize(transform.position - player.transform.position);
            }
        } else
        {
            if (CloseEnough(transform.position.x, pathMarkers[lastMarker].transform.position.x) && CloseEnough(transform.position.y, pathMarkers[lastMarker].transform.position.y))
            {
                if (lastMarker == pathMarkers.Length - 1)
                {
                    lastMarker = 0;
                } else
                {
                    ++lastMarker;
                }
                direction = Vector2.Normalize(pathMarkers[lastMarker].transform.position - transform.position);
            }
            
            if (!((rb.linearVelocityX == 0 || rb.linearVelocityY == 0) && !(rb.linearVelocityX == 0 && rb.linearVelocityY == 0)))
            {
                direction = Vector2.Normalize(pathMarkers[lastMarker].transform.position - transform.position);
            } else
            {
                // direction = Vector2.Normalize(rb.linearVelocity);
            }
        }
    }

    void FixedUpdate()
    {
        rb.linearVelocity = direction * speed;
    }

    protected override void AggroResponse()
    {
        base.AggroResponse();
        animator.Play("RoboHornetFlyingAggro");
    }

    protected override void AggroLeave()
    {
        base.AggroLeave();
        animator.Play("RoboHornetFlyingPassive");
        direction = Vector2.Normalize(pathMarkers[lastMarker].transform.position - transform.position);
    }

    private IEnumerator AttackTimer()
    {
        while (true)
        {
            yield return new WaitForSeconds(attackInterval);
            if (aggro) Attack();
        }
    }

    protected override void Attack()
    {
        base.Attack();
        GameObject bullet = Instantiate(spawnBullet, transform.position, Quaternion.identity);
        bullet.SendMessage("TargetLocked", player);
    }
}
