using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.UI;

public class Boss_Kitsune : Base_Enemy
{
    // UI
    public Slider bossHealthSlider;
    public GameObject BossCanvas;

    //
    enum HealthStats
    {
        Full,
        High,
        Mid,
        Low,
        SuperLow,
    }

    // timer
    float timer;
    float SpawnTimer;
    float intervalTimer;
    float RegenTimer;
    public float m_SheildTimer;

    float m_AttackTimer;
    public float m_ConstAttackTimer = 5;
    //Animation

    // attack mode
    Vector2 StartLocation;
    bool AttackMode;

    public List<PoolManager.EnemiesType> EnemySpawn;

    //
    public BoxCollider2D AttackBox;

    //15% health
    public PoolManager.EnemiesType m_Enemy;
    public int m_HowManyToSpawn = 3;
    bool m_BossLowHealth;
    bool m_Finish;
    List<GameObject> m_SpawnEnemies;

    HealthStats curHealthState;

    Dictionary<HealthStats, Action> BossStates = new Dictionary<HealthStats, Action>();



    // New Stuff
    public float AttackRange = 10;
    public GameObject[] TeleportLocations;

    public float DelayBetweenMeleeAttack = 1;
    float m_MeleeTimer;

    public float DelayBetweenTeleport = 1;
    float m_TeleportTimer;

    public float DelayBetweenDash = 1;
    float m_DashTimer;

    public float DashTimeLength = 1;
    public float DashSpeed = 35;
    bool m_Dash;
    bool DashAttacked;
    public Collider2D BlockingCollider;
    public Collider2D RangeCollider;

    int DashDmg=10,SpinAttackDmg=15, BulletDmg=5;

    // Use this for initialization
    public override void Start()
    {
        base.Start();

        m_SpawnEnemies = new List<GameObject>();
        timer = 3;
        bossHealthSlider.maxValue = Health = Max_Health;
        BossStates.Add(HealthStats.Full, FullHealth);
        BossStates.Add(HealthStats.High, HighHealth);
        BossStates.Add(HealthStats.Mid, MidHealth);
        BossStates.Add(HealthStats.Low, LowHeath);
        BossStates.Add(HealthStats.SuperLow, SuperLowHealth);

        StartLocation = transform.position;
        intervalTimer = 10;
        m_AttackTimer = m_ConstAttackTimer;
        SetState(EnemyState.Walk);

        Physics2D.IgnoreCollision(Morgan.GetComponent<Collider2D>(), RangeCollider, true);
    }

    // Update is called once per frame
    public override void Update()
    {
        //AlwaysUpdateTimers
        TimerUpdate();

        if (!m_BossLowHealth)
        {
            base.Update();

        }
        else
        {
            //bool temp = false;
            //for (int i = 0; i < m_SpawnEnemies.Count; i++)
            //{
            //    if (m_SpawnEnemies[i].activeSelf)
            //        temp = true;
            //}
            //if (!temp)
            //{
            m_BossLowHealth = false;
            //    Health = Max_Health / 2;
            //    bossHealthSlider.value = Health;
            //    TakeDamage(0);
            //    m_Finish = true;
            //    TurnOnCollision();
            //    AttackBox.enabled = false;
            //    GetComponent<Renderer>().enabled = true;
            //    m_SpawnEnemies.Clear();
            //}
        }
    }

    public override void IdleState()
    {
        anim.Play("Idle");
        //anim.Play("SpinAttack");
    }

    public override void SetState(EnemyState nextState)
    {
        if (curState != nextState)
        {
            switch (nextState)
            {
                case EnemyState.Idle:
                    anim.Play("Idle");
                    break;
                case EnemyState.Walk:
                    anim.Play("Walk");
                    break;
                case EnemyState.Attack:
                    {

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
        base.SetState(nextState);
    }

    public override void WalkState()
    {
        AttackState();
        if (!m_Dash)
        {
            if (!anim.GetBool("Attack") && AttackMode)
            {
                anim.Play("Walk");
            }
            float direction = gameObject.transform.position.x - Morgan.transform.position.x;
            if (direction <= 0)
            {
                Flip(true);
                if (AttackMode)
                    GetComponent<Rigidbody2D>().velocity = new Vector2(MoveSpeed * 25 * Time.deltaTime, GetComponent<Rigidbody2D>().velocity.y);
                else
                    GetComponent<Rigidbody2D>().velocity = new Vector2();
            }
            else
            {
                Flip(false);
                if (AttackMode)
                    GetComponent<Rigidbody2D>().velocity = new Vector2(-MoveSpeed * 25 * Time.deltaTime, GetComponent<Rigidbody2D>().velocity.y);
                else
                    GetComponent<Rigidbody2D>().velocity = new Vector2();
            }
            // Debug.Log(GetComponent<Rigidbody2D>().velocity); 
        }
    }
    public override void Jump()
    {

    }

    public override void DeathState()
    {
        if (anim.GetBool("RealDeath"))
        {
            Destroy(this.gameObject);
        }
    }



    public override void AttackState()
    {
        BossStates[curHealthState].Invoke();
    }


    void OnTriggerEnter2D(Collider2D other)
    {
        // If player is possing a enemy does 10 damage on touching boss
        float direction = gameObject.transform.position.x - Morgan.transform.position.x;

        if (other.tag == "Player" && msm.isPossessing)
        {
            // May need change
            TakeDamage(Damage);
            msm.TransitionFromYurei();

            if (direction <= 0)
                msm.GetThrown(false);
            else
                msm.GetThrown(true);
        }
        else if (other.tag == "Player" && m_Dash)
        {
            if (!DashAttacked)
            {
                playerHealth.TakeDamage(DashDmg);
                DashAttacked = true;
                anim.Play("Melee");
                Invoke("DashOff", .1f);
            }
        }
        else if (other.tag == "Player")
        {
            playerHealth.TakeDamage(Damage);
            if (direction <= 0)
                msm.GetThrown(false);
            else
                msm.GetThrown(true);
        }

        if (other.tag == "Bullet")
        {
            if (direction <= 0)
            {
                Flip(false);
                GetComponent<Rigidbody2D>().velocity = new Vector2(MoveSpeed * 25 * Time.deltaTime, 0);
            }
            else
            {
                Flip(true);
                GetComponent<Rigidbody2D>().velocity = new Vector2(-MoveSpeed * 25 * Time.deltaTime, 0);
            }
        }
    }


    public override void TakeDamage(int dmg)
    {
        base.TakeDamage(dmg);
        if ((float)Health / (float)Max_Health > .75) // 100%
        {
            if (curHealthState != HealthStats.Full)
            {
                curHealthState = HealthStats.Full;

            }
        }
        else if ((float)Health / (float)Max_Health > .50) //75%
        {
            if (curHealthState != HealthStats.High)
            {
                curHealthState = HealthStats.High;

            }
        }
        else if ((float)Health / (float)Max_Health > .35)//50%
        {
            if (curHealthState != HealthStats.Mid)
            {
                curHealthState = HealthStats.Mid;

            }
        }
        else if ((float)Health / (float)Max_Health > .25)//35%
        {
            if (curHealthState != HealthStats.Low)
            {
                curHealthState = HealthStats.Low;

            }
        }
        else //25%
        {

        }


        bossHealthSlider.value = Health;

        if (Health <= 0)
        {
            SetState(EnemyState.Death);
            anim.SetBool("Dead", true);
        }
    }

    public void FullHealth()
    {
        float Dis = Vector2.Distance(gameObject.transform.position, Morgan.transform.position);
        if (m_DashTimer <= 0 && Dis >= AttackRange)
        {
            Dash();
        }
        else if (m_MeleeTimer <= 0 && Dis <= AttackRange)
        {
            SwordSlash();
        }
    }
    public void HighHealth()
    {

    }

    public void MidHealth()
    {

    }
    public void LowHeath()
    {

    }

    public void SuperLowHealth()
    {

    }

    void TimerUpdate()
    {
        m_TeleportTimer -= Time.deltaTime;
        m_MeleeTimer -= Time.deltaTime;
        m_DashTimer -= Time.deltaTime;
        if (m_MeleeTimer <= 0 || m_DashTimer <= 0)
        {
            AttackMode = true;
        }
        else
        {
            AttackMode = false;
        }
    }

    public void AttackModeOn()
    {
        curState = EnemyState.Attack;
        if (!BossCanvas.activeSelf)
            BossCanvas.SetActive(true);

    }
    public void AttackModeOff()
    {
        curState = EnemyState.Idle;
        if (BossCanvas.activeSelf)
            BossCanvas.SetActive(false);
    }


    void SpawnLastBossMinions()
    {
        GameObject temp = GetPoolManager().FindClass(m_Enemy);
        temp.transform.position = gameObject.transform.position + new Vector3(3, 0, 0);
        temp.transform.rotation = Quaternion.identity;
        m_SpawnEnemies.Add(temp);

        temp = GetPoolManager().FindClass(m_Enemy);
        temp.transform.position = gameObject.transform.position + new Vector3(0, 0, 0);
        temp.transform.rotation = Quaternion.identity;
        m_SpawnEnemies.Add(temp);

        temp = GetPoolManager().FindClass(m_Enemy);
        temp.transform.position = gameObject.transform.position + new Vector3(-3, 0, 0);
        temp.transform.rotation = Quaternion.identity;
        m_SpawnEnemies.Add(temp);
    }

    void Dash()
    {
        GetComponent<Rigidbody2D>().gravityScale = 0;
        //GetComponent<Rigidbody2D>().velocity = new Vector2();

        float direction = gameObject.transform.position.x - Morgan.transform.position.x;
        if (direction <= 0)
        {
            gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(DashSpeed, 0);
            Flip(true);
        }
        else
        {
            gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(-DashSpeed, 0);
            Flip(false);
        }
        Physics2D.IgnoreCollision(Morgan.GetComponent<Collider2D>(), BlockingCollider, true);
        m_Dash = true;

        DashAttacked = false;
        m_DashTimer = DashTimeLength + DelayBetweenDash;
        anim.Play("Run");
        Invoke("DashOff", DashTimeLength);
        anim.SetBool("Attack", true);
    }

    void DashOff()
    {
        gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2();
        m_Dash = false;
        GetComponent<Rigidbody2D>().gravityScale = 1;
        if (m_DashTimer <= 0)
            m_DashTimer = DelayBetweenDash;
        BlockingCollider.enabled = true;
        DashAttacked = false;
        Physics2D.IgnoreCollision(Morgan.GetComponent<Collider2D>(), BlockingCollider, false);
        if (m_MeleeTimer <= 0 || m_DashTimer <= 0)
            anim.Play("Walk");
        else
            anim.Play("Idle");
        anim.SetBool("Attack", false);
    }

    void SwordSlash()
    {
        float direction = gameObject.transform.position.x - Morgan.transform.position.x;
        playerHealth.TakeDamage(Damage);

        if (direction <= 0)
            msm.GetThrown(false);
        else
            msm.GetThrown(true);
        anim.Play("Melee");
        anim.SetBool("Attack", true);

        m_MeleeTimer = DelayBetweenMeleeAttack;
        // 
        Invoke("SetWalkStateInvoke", .4f);
    }

    void SwordSlashSpin()
    {

    }

    void TeleportEffects()
    {
        TurnOffCollision();

        //Change for later
        Invoke("Teleport", 1);
    }

    void Teleport()
    {
        //Safe Check
        if (TeleportLocations.Length != 0)
        {
            int Loc = UnityEngine.Random.Range(0, TeleportLocations.Length - 1);
            gameObject.transform.position = TeleportLocations[Loc].transform.position;
        }
        else
            Debug.Log("Boss Kitsume Does not have any teleport locations");
    }

    void SetWalkStateInvoke()
    {
        SetState(EnemyState.Walk);
        if (m_MeleeTimer <= 0 || m_DashTimer <= 0)
            anim.Play("Walk");
        else
            anim.Play("Idle");
        anim.SetBool("Attack", false);
    }

    void PlayAnimation()
    {

    }

}
