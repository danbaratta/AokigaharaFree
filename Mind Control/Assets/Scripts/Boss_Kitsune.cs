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
    bool m_SkillBeingUsed;


    public float AttackRange = 10;
    public GameObject[] TeleportLocations;

    public float DelayBetweenMeleeAttack = 1;
    float m_MeleeTimer;

    public float DelayBetweenAttackTeleport = 1;
    public float DelayBetweenFleeTeleport = 1;
    public float DelayBetweenRandomTeleport = 1;
    float m_TeleportAttackTimer;
    float m_TeleportFleeTimer;
    float m_TeleportRandomTimer;

    public float DelayBetweenDash = 1;
    public float DelayBetweenDashEvade = 2;
    float m_DashTimer;
    float m_DashEvadeTimer;

    public float DashTimeLength = 1;
    public float DashEvadeTimeLength = 1;
    public float DashSpeed = 35;
    bool m_Dash;
    bool m_DashEvade;
    bool DashAttacked;

    public Collider2D BlockingCollider;
    public Collider2D RangeCollider;

    public float TeleportEffectTimer = 0;
    bool m_TeleportEffect;
    public int DashDmg = 10, SpinAttackDmg = 15, BulletDmg = 5, HanyaDmg = 50;

    //
    public float DelayBetweenHanyaAttack = 8;
    float m_HanyaAttackTimer;

    //
    bool m_SwordSlash;
    bool m_SwordSlashSpin;
    public float SwordSlashSpinLength;
    public float SwordSlashSpinDelay = 10;
    float m_SwordSlashSpinDelay;
    float m_SwordSlashSpinLengthTimer;
    Vector2 m_SowrdSlashStartLocation;

    bool m_IdleAttack;

    public float DelayBetweenHanyaTripleAttack = 15;
    public float DelayBetweenHanyaTripleEffect = 8;
    public float DelayBetweenHanyaTripleFire = 2;
    public float BulletSpacing = 1.5f;

    float m_DelayBetweenHanyaTripleEffectTimer;
    float m_DelayBetweenHanyaTripleFireTimer;
    float m_HanyaTripleAttackTimer;
    bool m_HanyaTripleAttack = false;

    public int HowManyHanyaBullets = 3;
    GameObject[] m_HanyaBullets;
    float[] m_HanyaBulletsAngles;

    bool m_RedFoxSpawn;
    float m_DamageTaken = 0;

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
        SetState(EnemyState.Attack);

        Physics2D.IgnoreCollision(Morgan.GetComponent<Collider2D>(), RangeCollider, true);
        m_HanyaBullets = new GameObject[HowManyHanyaBullets];
        m_HanyaBulletsAngles = new float[HowManyHanyaBullets];
        //HanyaTripleAttackOn();
        m_DelayBetweenHanyaTripleEffectTimer = DelayBetweenHanyaTripleEffect;
        m_DelayBetweenHanyaTripleFireTimer = .1f;
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
            bool temp = false;
            for (int i = 0; i < m_SpawnEnemies.Count; i++)
            {
                if (m_SpawnEnemies[i].activeSelf)
                    temp = true;
            }
            if (m_RedFoxSpawn && !temp)
            {
                m_RedFoxSpawn = false;
                m_BossLowHealth = false;
                TurnOnCollision();
                GetComponent<Renderer>().enabled = true;
                m_SkillBeingUsed = false;
            }
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
        float direction = gameObject.transform.position.x - Morgan.transform.position.x;
        float Dis = direction * direction;
        Dis = Mathf.Sqrt(Dis);
        if (!m_Dash && !m_DashEvade && AttackMode && !m_HanyaTripleAttack && !m_SwordSlashSpin)
        {
            if (!anim.GetBool("Attack"))
            {
                if (!anim.GetBool("Walk") && !m_SwordSlashSpin)
                {
                    anim.Play("Walk");
                    anim.SetBool("Walk", true);
                }
            }
            if (direction <= 0 && Dis > 1)
            {
                Flip(true);
                GetComponent<Rigidbody2D>().velocity = new Vector2(MoveSpeed * 25 * Time.deltaTime, GetComponent<Rigidbody2D>().velocity.y);
            }
            else if (Dis > 1)
            {
                Flip(false);
                GetComponent<Rigidbody2D>().velocity = new Vector2(-MoveSpeed * 25 * Time.deltaTime, GetComponent<Rigidbody2D>().velocity.y);
            }
            else
            {
                GetComponent<Rigidbody2D>().velocity = new Vector2(0, GetComponent<Rigidbody2D>().velocity.y);

            }
            // Debug.Log(GetComponent<Rigidbody2D>().velocity); 
        }
        else if (!m_Dash && !m_DashEvade && m_HanyaTripleAttack)
        {
            //GetComponent<Rigidbody2D>().velocity = new Vector2();
            if (anim.GetBool("Walk"))
            {
                anim.SetBool("Walk", false);
            }
            if (direction <= 0)
                Flip(true);
            else
                Flip(false);
        }
        else if (m_SwordSlashSpin)
        {
            anim.Play("SpinAttack");
            if (m_SwordSlashSpinLengthTimer < 0)
            {
                m_SwordSlashSpin = false;
            }
        }

        if (m_Dash)
        {
            if (!Mirror)
            {
                gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(DashSpeed, 0);
            }
            else
            {
                gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(-DashSpeed, 0);
            }
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
        WalkState();
    }


    void OnTriggerEnter2D(Collider2D other)
    {
        // If player is possing a enemy does 10 damage on touching boss
        float direction = gameObject.transform.position.x - Morgan.transform.position.x;

        if (other.tag == "Player" && msm.isPossessing)
        {
            // May need change
            TakeDamage(Damage);
            msm.TransitionFrom();

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
        else if (other.tag == "Player" && m_DashEvade)
        {
            Invoke("DashOff", .1f);
        }
        else if (other.tag == "Player" && !m_SwordSlashSpin)
        {
            playerHealth.TakeDamage(Damage);
            if (direction <= 0)
                msm.GetThrown(false);
            else
                msm.GetThrown(true);
            anim.Play("Melee");
        }
        else if (other.tag == "Player" && !m_SwordSlashSpin)
        {
            playerHealth.TakeDamage(Damage);
            if (direction <= 0)
                msm.GetThrown(false);
            else
                msm.GetThrown(true);
            anim.Play("Idle");
            transform.position = m_SowrdSlashStartLocation;
            m_SwordSlashSpin = false;
            m_SwordSlashSpinDelay = SwordSlashSpinDelay;
        }
        if (other.tag == "Bullet")
        {
            //if (direction <= 0)
            //{
            //    Flip(false);
            //    GetComponent<Rigidbody2D>().velocity = new Vector2(MoveSpeed * 25 * Time.deltaTime, 0);
            //}
            //else
            //{
            //    Flip(true);
            //    GetComponent<Rigidbody2D>().velocity = new Vector2(-MoveSpeed * 25 * Time.deltaTime, 0);
            //}
        }
    }


    public override void TakeDamage(int dmg)
    {
        if (!m_DashEvade)
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
            if (curHealthState != HealthStats.SuperLow)
                curHealthState = HealthStats.SuperLow;

            m_DamageTaken += dmg;
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
        else if (!m_SkillBeingUsed && m_TeleportAttackTimer <= 0 && !m_TeleportEffect)
        {
            TeleportEffects("AttackTeleport");
        }
        else if (!m_SkillBeingUsed && m_MeleeTimer <= 0 && Dis <= AttackRange)
        {
            SwordSlash();
        }
        else if (!m_SkillBeingUsed && m_TeleportFleeTimer <= 0 && !m_TeleportEffect)
        {
            TeleportEffects("FleeTeleport");
        }
    }
    public void HighHealth()
    {
        if (m_HanyaAttackTimer <= 0)
        {
            HanyaAttack();
            FleeTeleport();
        }
    }

    public void MidHealth()
    {

        float Dis = Vector2.Distance(gameObject.transform.position, Morgan.transform.position);
        if (!m_SkillBeingUsed && m_HanyaAttackTimer <= 0)
        {
            HanyaAttack();
        }
        else if (m_DashTimer <= 0 && Dis >= AttackRange && !m_SkillBeingUsed)
        {
            Dash();
        }
        else if (!m_SkillBeingUsed && m_TeleportAttackTimer <= 0 && !m_TeleportEffect)
        {
            TeleportEffects("AttackTeleport");
        }
        else if (!m_SkillBeingUsed && m_MeleeTimer <= 0 && Dis <= AttackRange)
        {
            SwordSlash();
        }
        else if (!m_SkillBeingUsed && m_TeleportRandomTimer <= 0 && !m_TeleportEffect)
        {
            TeleportEffects("FleeTeleport");
        }
        else if (m_DashEvadeTimer < 0 && !m_DashEvade)
        {
            DashEvasive();
        }
    }
    public void LowHeath()
    {

        float Dis = Vector2.Distance(gameObject.transform.position, Morgan.transform.position);

        if (!m_SkillBeingUsed && m_HanyaTripleAttackTimer <= 0 && !m_HanyaTripleAttack)
        {
            HanyaTripleAttackOn();
        }
        else if (m_HanyaTripleAttack)
            HanyaTripleAttack();
        else if (!m_SkillBeingUsed && m_SwordSlashSpinDelay <= 0 && !m_SwordSlashSpin)
        {
            SwordSlashSpin();
        }
        else if (m_DashTimer <= 0 && Dis >= AttackRange && !m_SkillBeingUsed)
        {
            Dash();
        }
        else if (!m_SkillBeingUsed && m_TeleportAttackTimer <= 0 && !m_TeleportEffect)
        {
            TeleportEffects("AttackTeleport");
        }
        else if (!m_SkillBeingUsed && m_MeleeTimer <= 0 && Dis <= AttackRange)
        {
            SwordSlash();
        }
        else if (!m_SkillBeingUsed && m_TeleportRandomTimer <= 0 && !m_TeleportEffect)
        {
            TeleportEffects("FleeTeleport");
        }
        else if (m_DashEvadeTimer < 0 && !m_DashEvade)
        {
            DashEvasive();
        }
    }

    public void SuperLowHealth()
    {
        if (!m_SkillBeingUsed && m_DamageTaken >= 20 && !m_RedFoxSpawn)
        {
            SpawnLastBossMinions();
            m_DamageTaken = 0;
            m_SkillBeingUsed = true;
        }
        else if (!m_SkillBeingUsed && m_HanyaTripleAttackTimer <= 0 && !m_HanyaTripleAttack)
        {
            HanyaTripleAttackOn();
        }
        else if (m_HanyaTripleAttack)
            HanyaTripleAttack();
    }

    void TimerUpdate()
    {
        m_TeleportAttackTimer -= Time.deltaTime;
        m_TeleportFleeTimer -= Time.deltaTime;
        m_TeleportRandomTimer -= Time.deltaTime;
        m_MeleeTimer -= Time.deltaTime;
        m_DashTimer -= Time.deltaTime;
        m_DashEvadeTimer -= Time.deltaTime;
        m_HanyaAttackTimer -= Time.deltaTime;
        m_HanyaTripleAttackTimer -= Time.deltaTime;
        m_SwordSlashSpinDelay -= Time.deltaTime;
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
        m_RedFoxSpawn = true;
        GameObject temp = GetPoolManager().FindClass(m_Enemy);
        temp.transform.position = gameObject.transform.position + new Vector3(3, 0, 0);
        temp.transform.rotation = Quaternion.identity;
        m_SpawnEnemies.Add(temp);


        // Add Teleport or jump function/action here

        //Hide Boss
        m_BossLowHealth = true;
        GetComponent<Renderer>().enabled = false;
        TurnOffCollision();

    }

    void Dash()
    {
        float direction = gameObject.transform.position.x - Morgan.transform.position.x;

        int Bits = 1 << 8;
        RaycastHit2D Ray = Physics2D.Raycast(gameObject.transform.position, new Vector2(-direction, 0), 25, Bits);
        Debug.DrawRay(gameObject.transform.position, new Vector2(-direction, 0) * 25);
        if (Ray.collider != null)
        {
            if (Ray.collider.gameObject.tag == "Player")
            {
                GetComponent<Rigidbody2D>().gravityScale = 0;
                //GetComponent<Rigidbody2D>().velocity = new Vector2();

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
                anim.SetBool("Walk", false);
                m_SkillBeingUsed = true;

            }
            else
                m_DashTimer = 1;
        }
        else
            m_DashTimer = 1;
    }
    void DashEvasive()
    {
        GetComponent<Rigidbody2D>().gravityScale = 0;
        //GetComponent<Rigidbody2D>().velocity = new Vector2();

        float direction = gameObject.transform.position.x - Morgan.transform.position.x;
        if (direction <= 0)
        {
            gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(DashSpeed, 0);
            Flip(false);
        }
        else
        {
            gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(-DashSpeed, 0);
            Flip(true);
        }
        Physics2D.IgnoreCollision(Morgan.GetComponent<Collider2D>(), BlockingCollider, true);
        m_DashEvade = true;

        m_DashEvadeTimer = DashEvadeTimeLength + DelayBetweenDashEvade;
        anim.Play("Run");
        Invoke("DashOff", DashEvadeTimeLength);
        anim.SetBool("Walk", false);
        m_SkillBeingUsed = true;

    }

    void DashOff()
    {
        gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2();
        m_Dash = false;
        GetComponent<Rigidbody2D>().gravityScale = 1;

        if (m_Dash)
        {
            m_Dash = false;
            if (m_DashTimer <= 0)
                m_DashTimer = DelayBetweenDash;
        }
        if (m_DashEvade)
        {
            if (m_DashEvadeTimer <= 0)
                m_DashEvadeTimer = DelayBetweenDashEvade;
        }
        BlockingCollider.enabled = true;
        DashAttacked = false;
        Physics2D.IgnoreCollision(Morgan.GetComponent<Collider2D>(), BlockingCollider, false);
        if (m_MeleeTimer <= 0 || m_DashTimer <= 0)
            anim.Play("Walk");
        else
            anim.Play("Idle");
        anim.SetBool("Attack", false);
        m_SkillBeingUsed = false;

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
        m_SwordSlash = true;
        m_MeleeTimer = DelayBetweenMeleeAttack;

        // 
        Invoke("SetWalkStateInvoke", .4f);
    }

    void SwordSlashSpin()
    {
        m_SwordSlashSpin = true;
        anim.Play("SpinAttack");
        m_SowrdSlashStartLocation = gameObject.transform.position;
        m_SwordSlashSpinLengthTimer = SwordSlashSpinLength;

    }

    void HanyaAttack()
    {
        anim.Play("Shoot");
        GameObject TempBullet = GetPoolManager().FindClass(PoolManager.EnemiesType.Hanya_Bullet);
        TempBullet.transform.position = gameObject.transform.position;
        TempBullet.transform.rotation = Quaternion.identity;
        if (Mirror)
            TempBullet.SendMessage("FlipAxisLeft");
        else
            TempBullet.SendMessage("FlipAxisRight");
        TempBullet.SendMessage("FireBullet");
        TempBullet.GetComponent<Enemy_Projectile_Kitsune>().Damage = HanyaDmg;

        m_HanyaAttackTimer = DelayBetweenHanyaAttack;
        m_SkillBeingUsed = false;

    }


    void TeleportEffects(string FunctionCall)
    {
        TurnOffCollision();
        m_TeleportEffect = true;
        m_SkillBeingUsed = true;

        //Change for later
        Invoke(FunctionCall, TeleportEffectTimer);
    }

    void RandomTeleport()
    {
        TurnOnCollision();

        m_TeleportEffect = false;
        m_TeleportRandomTimer = DelayBetweenRandomTeleport;
        //Safe Check
        if (TeleportLocations.Length != 0)
        {
            int Loc = UnityEngine.Random.Range(0, TeleportLocations.Length - 1);
            if (TeleportLocations[Loc])
                gameObject.transform.position = TeleportLocations[Loc].transform.position;
            else
                Debug.Log("Boss Kitsume Does teleport locations are NULL!");

        }
        else
            Debug.Log("Boss Kitsume Does not have any teleport locations");
        m_SkillBeingUsed = false;
    }

    void AttackTeleport()
    {
        TurnOnCollision();
        m_TeleportEffect = false;
        m_TeleportAttackTimer = DelayBetweenAttackTeleport;
        //Safe Check
        if (TeleportLocations.Length != 0)
        {
            GameObject Target = null;
            float Distance = float.MaxValue;
            if (TeleportLocations[0])
            {
                Target = TeleportLocations[0];
                Distance = Vector2.Distance(transform.position, Target.transform.position);
            }
            else
            {
                Debug.Log("Boss Kitsume Does teleport locations are NULL!");
                return;
            }

            for (int i = 1; i < TeleportLocations.Length; i++)
            {
                if (Vector2.Distance(transform.position, TeleportLocations[i].transform.position) < Distance)
                {
                    Distance = Vector2.Distance(transform.position, TeleportLocations[i].transform.position);
                    Target = TeleportLocations[i];
                }
            }
            gameObject.transform.position = Target.transform.position;

        }
        else
            Debug.Log("Boss Kitsume Does not have any teleport locations");
        m_SkillBeingUsed = false;
    }

    void FleeTeleport()
    {
        TurnOnCollision();

        m_TeleportEffect = true;
        m_TeleportFleeTimer = DelayBetweenFleeTeleport;
        //Safe Check
        if (TeleportLocations.Length != 0)
        {
            GameObject Target = null;
            float Distance = 0;
            if (TeleportLocations[0])
            {
                Target = TeleportLocations[0];
                Distance = Vector2.Distance(transform.position, Target.transform.position);
            }
            else
            {
                Debug.Log("Boss Kitsume Does teleport locations are NULL!");
                return;
            }

            for (int i = 1; i < TeleportLocations.Length; i++)
            {
                if (Vector2.Distance(transform.position, TeleportLocations[i].transform.position) > Distance)
                {
                    Distance = Vector2.Distance(transform.position, TeleportLocations[i].transform.position);
                    Target = TeleportLocations[i];
                }
            }
            gameObject.transform.position = Target.transform.position;

        }
        else
            Debug.Log("Boss Kitsume Does not have any teleport locations");
        m_SkillBeingUsed = false;

    }

    void SetWalkStateInvoke()
    {
        //SetState(EnemyState.Walk);
        if (m_MeleeTimer <= 0 || m_DashTimer <= 0)
            anim.Play("Walk");
        else
            anim.Play("Idle");
        anim.SetBool("Attack", false);
        if (m_SwordSlash)
        {
            m_SwordSlash = false;
            m_SkillBeingUsed = false;

        }
        if (m_SwordSlashSpin)
        {
            m_SwordSlashSpin = false;
            m_SkillBeingUsed = false;

        }

    }

    public void AttackOff()
    {
        if (!m_SwordSlash || !m_SwordSlashSpin)
            anim.SetBool("Attack", false);
    }



    void HanyaTripleAttackOn()
    {
        anim.Play("Idle");

        GetComponent<Rigidbody2D>().velocity = new Vector2();
        m_HanyaTripleAttack = true;

        for (int i = 0; i < HowManyHanyaBullets; i++)
        {
            m_HanyaBullets[i] = GetPoolManager().FindClass(PoolManager.EnemiesType.Hanya_Bullet);
            m_HanyaBullets[i].transform.parent = gameObject.transform;
            m_HanyaBullets[i].transform.localPosition = new Vector3(0, 8, 0);
            m_HanyaBullets[i].transform.RotateAround(transform.position, new Vector3(1, 1, 1), (float)i * (float)(360 / HowManyHanyaBullets));
            m_HanyaBullets[i].transform.rotation = Quaternion.identity;
            m_HanyaBulletsAngles[i] = (float)i * (float)(360 / HowManyHanyaBullets);

        }

    }

    void HanyaTripleAttack()
    {
        m_DelayBetweenHanyaTripleEffectTimer -= Time.deltaTime;
        m_DelayBetweenHanyaTripleFireTimer -= Time.deltaTime;

        if (m_DelayBetweenHanyaTripleEffectTimer > 0 && m_DelayBetweenHanyaTripleFireTimer > 0)
        {
            for (int i = 0; i < HowManyHanyaBullets; i++)
            {
                if (m_HanyaBullets[i] == null)
                    continue;
                m_HanyaBulletsAngles[i] += Time.deltaTime;
                if (m_HanyaBulletsAngles[i] >= 360)
                    m_HanyaBulletsAngles[i] = m_HanyaBulletsAngles[i] - 360;
                m_HanyaBullets[i].transform.RotateAround(transform.position, new Vector3(1, 1, 1), Time.deltaTime * 50);
                m_HanyaBullets[i].transform.rotation = Quaternion.identity;
            }
        }
        else
        {
            for (int i = 0; i < HowManyHanyaBullets; i++)
            {
                if (m_HanyaBullets[i])
                {
                    m_HanyaBullets[i].transform.localPosition = new Vector3(0, -3 + i * BulletSpacing, 0);
                    m_HanyaBullets[i].transform.parent = null;
                    if (Mirror)
                        m_HanyaBullets[i].SendMessage("FlipAxisLeft");
                    else
                        m_HanyaBullets[i].SendMessage("FlipAxisRight");
                    m_DelayBetweenHanyaTripleFireTimer = DelayBetweenHanyaTripleFire;
                    m_HanyaBullets[i].SendMessage("FireBullet");
                    m_HanyaBullets[i] = null;
                    anim.Play("Shoot");
                    if (i == HowManyHanyaBullets - 1)
                        HanyaTripleAttackOff();
                    break;
                }
            }
        }
    }

    void HanyaTripleAttackOff()
    {
        m_DelayBetweenHanyaTripleFireTimer = DelayBetweenHanyaTripleFire;
        m_DelayBetweenHanyaTripleEffectTimer = DelayBetweenHanyaTripleEffect;
        m_HanyaTripleAttackTimer = DelayBetweenHanyaTripleAttack;
        m_HanyaTripleAttack = false;
        m_SkillBeingUsed = false;

    }
}
