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
        
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();
        if (Vector2.Distance(Morgan.transform.position, transform.position) > m_Distance)
        {
            Destroy(this.gameObject);
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
        if (anim.GetBool("RealDeath"))
        {
            Destroy(this);
        }
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

            Destroy(gameObject);
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
                msm.GetThrown();
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
}
