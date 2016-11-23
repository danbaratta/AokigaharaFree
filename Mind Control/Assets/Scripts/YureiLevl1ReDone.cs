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
    }

    public override void AttackState()
    {

    }
    public override void WalkState()
    {
        transform.position -= transform.right * Time.deltaTime * MoveSpeed;

        GetComponent<Rigidbody2D>().velocity = new Vector2(0, MoveSpeed*Time.deltaTime);
    }
    public override void DeathState()
    {
       if( anim.GetBool("Dead"))
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
            Destroy(this);
        }
        else if ((other.tag == "Player") && (msm.isPossessing == true))
        {
            msm.TransitionFromYurei();
            Destroy(gameObject);
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
                Destroy(gameObject);
            }
        }
    }

}
