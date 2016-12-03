using UnityEngine;
using System.Collections;

public class OniLevel1 : Base_Enemy
{

    public float m_JumpPower;
    public float m_Distance = 25f;

    //Test
    float timer;
    public float ConstTimer;

    public Collider2D AttackBox;

    // Use this for initialization
    public override void Start()
    {
        base.Start();
        Health = Max_Health;
        AttackBox.enabled = false;

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

    public override void SetState(EnemyState nextState)
    {
        curState = nextState;
        switch (nextState)
        {
            case EnemyState.Idle:
                {
                    anim.Play("Idle");
                    AttackBox.enabled = false;
                }
                break;
            case EnemyState.Walk:
                //anim.Play("Walk");
                break;
            case EnemyState.Attack:
                {
                    anim.Play("Attack");
                    AttackBox.enabled = true;
                }
                break;
            case EnemyState.Death:
                break;
            case EnemyState.Jump:
                break;
            case EnemyState.MaxStates:
                break;
            default:
                break;
        }
    }

    public override void Jump()
    {
        GetComponent<Rigidbody2D>().velocity += new Vector2(GetComponent<Rigidbody2D>().velocity.x, m_JumpPower);
        SetState(EnemyState.Walk);

    }

    public override void IdleState()
    {
        WalkState();
        anim.Play("Idle");
    }

    public override void AttackState()
    {
        //WalkState();
    }
    public override void WalkState()
    {
        //transform.position -= transform.right * Time.deltaTime * MoveSpeed;
        Vector3 temp = gameObject.transform.position - Morgan.transform.position;
        temp = temp.normalized;
        temp.y = 0;
        temp.x = -temp.x * MoveSpeed;

        if (temp.x <= 0)
            Flip(true);
        else if (temp.x > 0)
            Flip(false);
        GetComponent<Rigidbody2D>().velocity = new Vector2(temp.x, GetComponent<Rigidbody2D>().velocity.y);

        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            SetState(EnemyState.Jump);
            timer = ConstTimer;
        }

        if (Vector2.Distance(Morgan.transform.position, transform.position) < 2)
        {
            SetState(EnemyState.Attack);
        }

    }
    public override void DeathState()
    {
        Destroy(this.gameObject);
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

                GetComponent<Rigidbody2D>().velocity = new Vector2();

                Vector3 temp = gameObject.transform.position - Morgan.transform.position;
                temp = temp.normalized;
                temp.y = 20;
                temp.x = temp.x * 30;
                GetComponent<Rigidbody2D>().AddForce(temp, ForceMode2D.Impulse);
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
