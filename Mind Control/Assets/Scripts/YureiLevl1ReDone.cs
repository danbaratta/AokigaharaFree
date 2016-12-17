using UnityEngine;
using System.Collections;

public class YureiLevl1ReDone : Base_Enemy
{
    //Distance away from player to destory AI
    public float m_Distance = 25f;

    // Use this for initialization
    public override void Start()
    {
        base.Start();
        Health = Max_Health = 5;
        Type = PoolManager.EnemiesType.Yurei_Level1;
    }

    // Update is called once per frame
    public override void Update()
    {
        if (Vector2.Distance(Morgan.transform.position, transform.position) > m_Distance)
        {
            if (!NotSpawned)
                RemoveThis();
            GetComponent<Rigidbody2D>().velocity = new Vector2();
        }
        else
        {
            base.Update();
        }
    }


    public override void IdleState()
    {
        WalkState();
        anim.Play("Idle");
    }

    public override void AttackState()
    {

    }
    public override void WalkState()
    {
        //transform.position -= transform.right * Time.deltaTime * MoveSpeed;

        GetComponent<Rigidbody2D>().velocity = new Vector2(-MoveSpeed,0);
    }
    public override void DeathState()
    {
        base.DeathState();
    }


    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "MindBullet")
        {
            if (msm.right == false)
            {
                msm.Right(true);
            }

            msm.IsYurei(true);

            msm.GetTargetX(transform.position.x);
            msm.GetTargetY(transform.position.y);

             RemoveThis();
        }
        else if ((other.tag == "Player") && (msm.isPossessing == true))
        {
            msm.TransitionFromYurei();
            SetState(EnemyState.Death);
            anim.SetBool("Dead", true);
            anim.Play("Death");
            GetComponent<Rigidbody2D>().velocity = new Vector2();
        }

        else if (other.tag == "Player")
        {
            if (playerHealth.currentHealth > 0)
            {
                float direction = gameObject.transform.position.x - Morgan.transform.position.x;
                if (direction <= 0)
                    msm.GetThrown(false);
                else
                    msm.GetThrown(true);
                playerHealth.TakeDamage(Damage);       //deals damage to player

                SetState(EnemyState.Death);
                anim.SetBool("Dead", true);
                anim.Play("Death");
                TurnOffCollision();
                GetComponent<Rigidbody2D>().velocity = new Vector2();
            }
        }
    }

    /// <summary>
    /// Set location for player to move when possesing someone
    /// </summary>
    void SendPosition()
    {
        msm.GetTargetX(transform.position.x);
        msm.GetTargetY(transform.position.y);
    }

    override public void Reset()
    {
        base.Reset();

    }
}
