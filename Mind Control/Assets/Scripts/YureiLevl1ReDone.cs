using UnityEngine;
using System.Collections;

public class YureiLevl1ReDone : Base_Enemy
{

    // Use this for initialization
    public override void Start()
    {
        Health = Max_Health = 5;
        base.Start();
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();
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
        else if (other.gameObject.tag == "Bullet")
        {
            SetState(EnemyState.Death);
            anim.SetBool("Dead", true);
            anim.Play("Death");
            TurnOffCollision();
            GetComponent<Rigidbody2D>().velocity = new Vector2();
        }
        else if ((other.tag == "Player") && (msm.isPossessing == true))
        {
            msm.TransitionFromYurei();
            SetState(EnemyState.Death);
            anim.SetBool("Dead", true);
            anim.Play("Death");
            TurnOffCollision();
            GetComponent<Rigidbody2D>().velocity = new Vector2();
        }

        else if (other.tag == "Player")
        {
            if (playerHealth.currentHealth > 0)
            {
                msm.GetThrown();
                playerHealth.TakeDamage(Damage);       //deals damage to player
                //anim.SetTrigger("Flinch");                       //plays damage animation
                //{
                //    Debug.Log("play damage animation");
                //}
                //Debug.Log("Health works");
                //			msm.Possess (false); // Update this once we get a health and damage system.
                SetState(EnemyState.Death);
                anim.SetBool("Dead", true);
                anim.Play("Death");
                TurnOffCollision();
                GetComponent<Rigidbody2D>().velocity = new Vector2();
            }
        }
    }

    void OnBecameInvisible()
    {
        Destroy(gameObject);
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
