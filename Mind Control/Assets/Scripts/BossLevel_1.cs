using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.UI;

public class BossLevel_1 : Base_Enemy
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
        Low
    }

    // timer
    float timer;
    float SpawnTimer;
    float intervalTimer;
    float RegenTimer;
    public float m_SheildTimer;
    //Animation

    // attack mode
    Vector2 StartLocation;
    bool AttackMode;

    public List<GameObject> EnemySpawn;

    //
    GameObject Sheild;

    //
    public BoxCollider2D AttackBox;

    //15% health
    public GameObject m_Enemy;
    public int m_HowManyToSpawn = 3;
    bool m_BossLowHealth;
    bool m_Finish;
    List<GameObject> m_SpawnEnemies;

    HealthStats curHealthState;
    Dictionary<HealthStats, Action> BossStates = new Dictionary<HealthStats, Action>();
    // Use this for initialization
    public override void Start()
    {
        m_SpawnEnemies = new List<GameObject>();
        timer = 3;
        base.Start();
        bossHealthSlider.maxValue = Health = Max_Health;
        BossStates.Add(HealthStats.Full, FullHealth);
        BossStates.Add(HealthStats.High, HighHealth);
        BossStates.Add(HealthStats.Mid, MidHealth);
        BossStates.Add(HealthStats.Low, LowHeath);

        Sheild = gameObject.transform.GetChild(0).gameObject;

        StartLocation = transform.position;
        intervalTimer = 10;
        var bla = Input.GetJoystickNames();
    }

    // Update is called once per frame
    public override void Update()
    {
        if (!m_BossLowHealth)
        {
            base.Update();
            if (Sheild && Sheild.activeSelf)
            {
                RegenTimer -= Time.deltaTime;
                if (RegenTimer <= 0)
                {
                    Health += 15;
                    RegenTimer = 1;
                    bossHealthSlider.value = Health;
                }
                if (Health >= Max_Health / 2)
                {
                    Sheild.SetActive(false);
                }
            }
        }
        else
        {
            bool temp=false;
            for (int i = 0; i < m_SpawnEnemies.Count; i++)
            {
                if (m_SpawnEnemies[i])
                    temp = true;
            }
            if (!temp)
            {
                m_BossLowHealth = false;
                Health = Max_Health/2;
                bossHealthSlider.value = Health;
                TakeDamage(0);
                m_Finish = true;
                TurnOnCollision();
                AttackBox.enabled = false;
                GetComponent<Renderer>().enabled = true;
                m_SpawnEnemies.Clear();
            }
        }

    }

    public override void IdleState()
    {
        anim.SetBool("Attack", false);
    }

    public override void AttackState()
    {
        BossStates[curHealthState].Invoke();
        intervalTimer -= Time.deltaTime;
        if (Sheild && Sheild.activeSelf)
        {
            AttackMode = true;
            AttackBox.enabled = false;
            float direction = gameObject.transform.position.x - StartLocation.x;
            if (direction <= 0)
                GetComponent<Rigidbody2D>().velocity = new Vector2(MoveSpeed * 25 * Time.deltaTime, 0);
            else
                GetComponent<Rigidbody2D>().velocity = new Vector2(-MoveSpeed * 25 * Time.deltaTime, 0);
            anim.SetBool("Attack", false);
        }
        else if (intervalTimer < 0 && !AttackMode)
        {
            anim.SetBool("Attack", true);
            AttackMode = true;
            AttackBox.enabled = true;
            float direction = gameObject.transform.position.x - Morgan.transform.position.x;
            if (direction <= 0)
                GetComponent<Rigidbody2D>().velocity = new Vector2(MoveSpeed * 25 * Time.deltaTime, 0);
            else
                GetComponent<Rigidbody2D>().velocity = new Vector2(-MoveSpeed * 25 * Time.deltaTime, 0);
        }
        if (AttackMode && !AttackBox.enabled)
        {
            if (Vector2.Distance(new Vector2(gameObject.transform.position.x, gameObject.transform.position.y), StartLocation) < .5f)
            {
                AttackMode = false;
                intervalTimer = UnityEngine.Random.Range(10f, 15f);
                GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
                anim.SetBool("Attack", false);
            }
        }
    }


    void OnTriggerEnter2D(Collider2D other)
    {
        // If player is possing a enemy does 10 damage on touching boss
        if (other.tag == "Player" && msm.isPossessing)
        {
            TakeDamage(10);
            msm.TransitionFromYurei();
            msm.GetThrown();
        }
        else if (other.tag == "Player" && AttackMode)
        {
            playerHealth.TakeDamage(Damage);
            //other.gameObject.SendMessage("TakeDamage", Damage);
            AttackBox.enabled = false;
            msm.GetThrown();

            float direction = gameObject.transform.position.x - StartLocation.x;
            if (direction <= 0)
                GetComponent<Rigidbody2D>().velocity = new Vector2(MoveSpeed * 25 * Time.deltaTime, 0);
            else
                GetComponent<Rigidbody2D>().velocity = new Vector2(-MoveSpeed * 25 * Time.deltaTime, 0);
            anim.SetBool("Attack", false);
        }
    }


    public override void TakeDamage(int dmg)
    {
        base.TakeDamage(dmg);
        if ((float)Health / (float)Max_Health > .75)
        {
            curHealthState = HealthStats.Full;
        }
        else if ((float)Health / (float)Max_Health > .50)
        {
            curHealthState = HealthStats.High;
        }
        else if ((float)Health / (float)Max_Health > .25)
        {
            curHealthState = HealthStats.Mid;
        }
        else if ((float)Health / (float)Max_Health > .15)
            curHealthState = HealthStats.Low;
        else
        {
            if (!m_BossLowHealth&& !m_Finish)
            {
                m_BossLowHealth = true;
                SpawnLastBossMinions();
                GetComponent<Renderer>().enabled = false;
                TurnOffCollision();
            }
            else
                curHealthState = HealthStats.Low;
            //Spawn things here and hide guy until spawn things died.
        }


        bossHealthSlider.value = Health;

        if (Health <= 0)
            Destroy(this.gameObject);
    }

    public virtual void FullHealth()
    {
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            SpawnTimer -= Time.deltaTime;
            if (SpawnTimer <= 0)
            {
                Instantiate(EnemySpawn[0], this.gameObject.transform.position, Quaternion.identity);
                SpawnTimer = UnityEngine.Random.Range(2.5f, 5f);
            }
            if (timer < -10)
            {
                timer = 10;
                SpawnTimer = 0;
            }
        }
    }
    public virtual void HighHealth()
    {
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            SpawnTimer -= Time.deltaTime;
            if (SpawnTimer <= 0)
            {
                Instantiate(EnemySpawn[0], this.gameObject.transform.position, Quaternion.identity);
                SpawnTimer = UnityEngine.Random.Range(2.5f, 4f);
            }
            if (timer < -12)
            {
                timer = 12;
                SpawnTimer = 0;
            }
        }
    }

    public virtual void MidHealth()
    {
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            SpawnTimer -= Time.deltaTime;
            if (SpawnTimer <= 0)
            {
                Instantiate(EnemySpawn[0], this.gameObject.transform.position, Quaternion.identity);
                SpawnTimer = UnityEngine.Random.Range(2f, 4f);
            }
            if (timer < -14)
            {
                timer = 14;
                SpawnTimer = 0;
            }
        }
    }
    public virtual void LowHeath()
    {
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            SpawnTimer -= Time.deltaTime;
            if (SpawnTimer <= 0)
            {
                Instantiate(EnemySpawn[0], this.gameObject.transform.position, Quaternion.identity);
                SpawnTimer = UnityEngine.Random.Range(1.5f, 2.5f);
            }
            if (timer < -15)
            {
                timer = 15;
                SpawnTimer = 0;
                if (Sheild)
                    Sheild.SetActive(true);
            }
            else
            {
                if (Sheild)
                    Sheild.SetActive(false);
            }
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
            BossCanvas.SetActive(true);
    }


    void SpawnLastBossMinions()
    {
        m_SpawnEnemies.Add((GameObject)Instantiate(m_Enemy, this.gameObject.transform.position + new Vector3(3, 0, 0), Quaternion.identity));
        m_SpawnEnemies.Add((GameObject)Instantiate(m_Enemy, this.gameObject.transform.position + new Vector3(0, 0, 0), Quaternion.identity));
        m_SpawnEnemies.Add((GameObject)Instantiate(m_Enemy, this.gameObject.transform.position + new Vector3(-3, 0, 0), Quaternion.identity));
    }
}
