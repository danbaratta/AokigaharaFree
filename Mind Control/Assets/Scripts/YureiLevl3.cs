using UnityEngine;
using System.Collections;

public class YureiLevl3 : Base_Enemy
{
    //Distance away from player to destory AI
    public float m_Distance = 25f;

    public GameObject m_Projectile;

    public float m_ConstProjectileTimer;
    float m_ProjectileTimer = .5f;

    bool m_Invisable = false;
    public float m_Timer;

    public Vector2 m_TimerRange = new Vector2(5, 10);

    public bool m_Yurei3B;

    // Use this for initialization
    public override void Start()
    {
        base.Start();
        Health = Max_Health;
        Type = PoolManager.EnemiesType.Yurei_Level3;
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

        //transform.position -= transform.right * Time.deltaTime * MoveSpeed;
        Vector3 temp = gameObject.transform.position - Morgan.transform.position;
        temp = temp.normalized;

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
    public override void WalkState()
    {
        //transform.position -= transform.right * Time.deltaTime * MoveSpeed;
        Vector3 temp = gameObject.transform.position - Morgan.transform.position;
        temp = temp.normalized;
        GetComponent<Rigidbody2D>().velocity = -temp * MoveSpeed;
        if (temp.x < 0)
            Flip(false);
        else
            Flip(true);
        //
        AttackState();
        //
        if (m_Yurei3B)
        {
            m_Timer -= Time.deltaTime;
            if (m_Timer < 0 && !m_Invisable)
            {
                Color GhostColor = GetComponent<Renderer>().material.color;
                GhostColor.a -= Time.deltaTime;
                if (GhostColor.a > 0)
                    GetComponent<Renderer>().material.color = GhostColor;
                else
                {
                    m_Invisable = true;
                    GhostColor.a = 0;
                    GetComponent<Renderer>().material.color = GhostColor;
                }
            }
            if (m_Timer < -5 && m_Invisable)
            {
                Color GhostColor = GetComponent<Renderer>().material.color;
                GhostColor.a += Time.deltaTime * 2;
                if (GhostColor.a < 1)
                    GetComponent<Renderer>().material.color = GhostColor;
                else
                {
                    m_Timer = Random.Range(m_TimerRange.x, m_TimerRange.y);
                    m_Invisable = false;

                }
            }
        }

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
            Color GhostColor = GetComponent<Renderer>().material.color;
            GhostColor.a = 1;
            GetComponent<Renderer>().material.color = GhostColor;
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
                Color GhostColor = GetComponent<Renderer>().material.color;
                GhostColor.a = 1;
                GetComponent<Renderer>().material.color = GhostColor;
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
        m_ProjectileTimer = .5f;

        m_Invisable = false;
        m_Timer = 0;
        Color GhostColor = GetComponent<Renderer>().material.color;
        GhostColor.a = 1;
        GetComponent<Renderer>().material.color = GhostColor;
    }
}
