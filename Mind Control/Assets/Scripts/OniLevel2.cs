using UnityEngine;
using System.Collections;

public class OniLevel2 : Base_Enemy
{

    public float m_JumpPower;
    public float m_Distance = 25f;

    //Test
    float timer;
    public float ConstTimer;

    public Collider2D AttackBox;

    public bool m_UseNavMesh;
    UnityEngine.AI.NavMeshAgent Agent;

    //

    float m_ProjectileTimer;
    public float m_ConstProjectileTimer;
    // Use this for initialization
    public override void Start()
    {
        base.Start();
        Health = Max_Health;
        AttackBox.enabled = false;
        if (m_UseNavMesh)
        {
            Agent = gameObject.AddComponent<UnityEngine.AI.NavMeshAgent>();
            Agent.updateRotation = false;
            //Agent.updatePosition = false;

            Agent.radius = 2;
            Agent.height = 5;
            Agent.baseOffset = 3.36f;
            Agent.speed = 2;
            Agent.angularSpeed = 10;
            Agent.acceleration = 3;
        }
        Type = PoolManager.EnemiesType.Oni_Level1;
        m_ProjectileTimer = 1;
    }

    // Update is called once per frame
    public override void Update()
    {
        if (m_UseNavMesh)
        {
            Agent.SetDestination(Morgan.transform.position);
            base.Update();
            float x, y;

            x = GetComponent<Rigidbody2D>().velocity.x;
            y = GetComponent<Rigidbody2D>().velocity.y;
            Agent.velocity = new Vector2(GetComponent<Rigidbody2D>().velocity.x, GetComponent<Rigidbody2D>().velocity.y);

            y = 5;
        }
        else
        {
            base.Update();
            if (Vector2.Distance(Morgan.transform.position, transform.position) > m_Distance)
            {
                GetPoolManager().Remove(gameObject, Type);
            }
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
                anim.Play("Idle");
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

    void WaitJump()
    {
        SetState(EnemyState.Walk);
    }

    public override void IdleState()
    {
        WalkState();
        anim.Play("Idle");
    }

    public override void AttackState()
    {
        WalkState();
        if (Vector2.Distance(Morgan.transform.position, transform.position) > 5)
        {
            SetState(EnemyState.Idle);
        }
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
        if (!m_UseNavMesh)
        {
            GetComponent<Rigidbody2D>().velocity = new Vector2(temp.x, GetComponent<Rigidbody2D>().velocity.y);
        }
        else
        {
            GetComponent<Rigidbody2D>().velocity = new Vector2(temp.x, GetComponent<Rigidbody2D>().velocity.y);
            //Agent.velocity = Agent.velocity + new Vector3( GetComponent<Rigidbody2D>().velocity.x, GetComponent<Rigidbody2D>().velocity.y, 0);
            Agent.velocity = new Vector2(temp.x, GetComponent<Rigidbody2D>().velocity.y);
            //Debug.Log(Agent.velocity);
        }
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            SetState(EnemyState.Jump);
            timer = ConstTimer;
        }

        if (Vector2.Distance(Morgan.transform.position, transform.position) < 5)
        {
            SetState(EnemyState.Attack);
        }

        m_ProjectileTimer -= Time.deltaTime;
        if (m_ProjectileTimer <= 0)
        {
            GameObject TempBullet = GetPoolManager().FindClass(PoolManager.EnemiesType.EnemyBullets);
            TempBullet.transform.position = gameObject.transform.position;
            TempBullet.transform.rotation = Quaternion.identity;
            if (!Mirror)
                TempBullet.SendMessage("FlipAxisLeft");
            else
                TempBullet.SendMessage("FlipAxisRight");

            m_ProjectileTimer = m_ConstProjectileTimer;
        }

    }
    public override void DeathState()
    {
        GetPoolManager().Remove(gameObject, Type);
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

            GetPoolManager().Remove(gameObject, Type);
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

                GetComponent<Rigidbody2D>().velocity = new Vector2();

                Vector3 temp = gameObject.transform.position - Morgan.transform.position;
                temp = temp.normalized;
                temp.y = 15;
                temp.x = temp.x * 10;
                GetComponent<Rigidbody2D>().AddForce(temp, ForceMode2D.Impulse);
               // if (m_UseNavMesh)
               //     GetComponent<Rigidbody2D>().velocity = temp;
                Invoke("WaitJump", 2f);
                //SetState(EnemyState.Walk);
                SetState(EnemyState.MaxStates);
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
        AttackBox.enabled = false;
        timer = 0;
        m_ProjectileTimer = 1;
    }
}
