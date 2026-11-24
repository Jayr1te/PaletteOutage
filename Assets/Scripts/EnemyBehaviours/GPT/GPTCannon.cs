using UnityEngine;

public class GPTCannon : EnemyBase
{
    [SerializeField]
    private GameObject pivot;
    private Rigidbody2D pivotBody;
    [SerializeField]
    private GameObject bulletHole;
    [SerializeField]
    private GameObject cannonBullet;
    [SerializeField]
    private GameObject zakato;
    [SerializeField]
    private GameObject boss;
    private bool following = true;
    private string spawnBullet;

    public float error;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Startup();
        pivotBody = pivot.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (following)
        {
            // c_angle is the angle of the cannon's pivot in world space
            // p_angle is the angle between the vector from the pivot to the player and the y-axis
            direction = Vector2.Normalize(player.transform.position - pivot.transform.position);
            float c_angle = pivot.transform.rotation.eulerAngles.z;
            float p_angle = -Vector2.SignedAngle(direction, Vector2.down);

            // the quaternion angles range 0-360 and the vector angles range -180-180 but in a weird way, 
            // so this is to convert the quaternion angles to the vector ones
            c_angle = Mathf.Abs(c_angle) > 180 ? -(180 - (c_angle - 180)) : c_angle;
            // the higher the speed, the higher error needs to be for the cannons to not jitter
            // however, a higher speed makes up for a larger error, so it's fiiiiiineee
            if (!CloseEnough(c_angle, p_angle, error)) {
                pivotBody.angularVelocity = p_angle - c_angle < 0 ? -speed : speed;
            } else
            {
                pivotBody.angularVelocity = 0;
            }
        }
    }

    public void FireBullet()
    {
        spawnBullet = "cannonBullet";
        animator.Play("CannonFiring");
    }

    public void FireZakato()
    {
        spawnBullet = "zakato";
        animator.Play("CannonFiring");
    }

    // you can put event triggers in animations, so the CannonFiring animation triggers the event to call this method
    public void FireAway()
    {
        following = false;
        pivotBody.angularVelocity = 0;
        animator.Play("CannonAttack");
        GameObject cannonFire;

        if (spawnBullet == "zakato")
        {
            cannonFire = Instantiate(zakato, bulletHole.transform.position, Quaternion.identity);
            cannonFire.SendMessage("TargetLocked", player);
        } else
        {
            cannonFire = Instantiate(cannonBullet, bulletHole.transform.position, Quaternion.identity);
        }

        cannonFire.SendMessage("ReceiveDirection", direction);
    }

    // the CannonAttack animation sends triggers the event to call this method
    public void Follow()
    {
        animator.Play("CannonPulsate");
        following = true;
    }

    void OnDestroy()
    {
        Destroy(pivot);
        boss.SendMessage("CannonDestroyed", gameObject.name);
    }
}
