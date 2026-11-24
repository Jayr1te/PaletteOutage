using System.Collections;
using UnityEngine;

public class GPTBoss : EnemyBase
{
    [SerializeField]
    private GameObject lCannon;
    [SerializeField]
    private GameObject rCannon;
    // this is to alternate which cannon fires; false is left, true is right
    private bool side = false;
    private int cannonsRemaining = 2;
    // array of attacks the boss uses; if it appears more frequently then it is more likely to be selected
    private string[] attacks =
    {
        "FireBullet",
        "FireBullet",
        "FireBullet",
        "FireZakato",
        "FireZakato"
    };
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Startup();
        StartCoroutine(AttackTimer());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OpenGrate()
    {
        animator.Play("BodyGrateLift");
    }

    private IEnumerator AttackTimer()
    {
        while (true)
        {
            yield return new WaitForSeconds(2);

            if (cannonsRemaining > 0) {
                int attackPick = Random.Range(0, attacks.Length - 1);

                if (!side)
                {
                    lCannon.SendMessage(attacks[attackPick]);
                } else
                {
                    rCannon.SendMessage(attacks[attackPick]);
                }

                if (cannonsRemaining == 2)
                {
                    side = !side;
                }
            }
        }
    }

    public void CannonDestroyed(string name)
    {
        if (name == "LeftCannon")
        {
            lCannon = null;
            side = true;
        } else if (name == "RightCannon")
        {
            rCannon = null;
            side = false;
        }

        --cannonsRemaining;

        if (cannonsRemaining == 0)
        {
            OpenGrate();
        }
    }
}
