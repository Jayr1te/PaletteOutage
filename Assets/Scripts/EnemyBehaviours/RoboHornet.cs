using System;
using System.Timers;
using UnityEngine;

public interface IRoboHornet
{
    void AggroLeave();
    void AggroResponse();
}

public class RoboHornet : EnemyBase, IRoboHornet
{
    private Timer fireTimer;
    public GameObject spawnBullet;
    public GameObject[] pathMarkers;
    public int lastMarker = 0;
    public float attackInterval;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Startup();

        direction = Vector2.Normalize(pathMarkers[lastMarker].transform.position - transform.position);
        rb.linearVelocity = direction * speed;

        fireTimer = new Timer(attackInterval * 1000);
        fireTimer.Elapsed += OnTimedAttack;
    }

    // Update is called once per frame
    void Update()
    {
        if (rb.linearVelocityX < 0)
        {
            sprite.flipX = false;
        }
        else if (rb.linearVelocityX > 0)
        {
            sprite.flipX = true;
        }
    }

    void FixedUpdate()
    {
        if (CloseEnough(transform.position.x, pathMarkers[lastMarker].transform.position.x) && CloseEnough(transform.position.y, pathMarkers[lastMarker].transform.position.y))
        {
            if (lastMarker == pathMarkers.Length - 1)
            {
                lastMarker = 0;
            }
            else
            {
                ++lastMarker;
            }
            direction = Vector2.Normalize(pathMarkers[lastMarker].transform.position - transform.position);
            rb.linearVelocity = direction * speed;
        }
    }

    public override void AggroResponse()
    {
        base.AggroResponse();
        fireTimer.Start();
    }

    public override void AggroLeave()
    {
        base.AggroLeave();
        fireTimer.Stop();
    }

    public override void Attack()
    {
        base.Attack();
        GameObject bullet = Instantiate(spawnBullet);
        // bullet.target = player;
    }

    // this is here because the timer needs a method with certain parameters to be subscribed to its event
    private void OnTimedAttack(object source, ElapsedEventArgs e)
    {
        Attack();
    }

    // these are here because of the way System.Timers.Timer works
    void OnDestroy()
    {
        fireTimer.Dispose();
    }

    void OnApplicationQuit()
    {
        fireTimer.Dispose();
    }
}
