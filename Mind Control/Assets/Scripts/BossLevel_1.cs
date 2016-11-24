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
    //Animation

    // attack mode
    Vector2 StartLocation;
    bool AttackMode;

    public List<GameObject> EnemySpawn;

    //
    GameObject Sheild;

    //
    public BoxCollider2D AttackBox;

    HealthStats curHealthState;
    Dictionary<HealthStats, Action> BossStates = new Dictionary<HealthStats, Action>();
    // Use this for initialization
    public override void Start()
    {
        timer = 3;
        base.Start();
        bossHealthSlider.maxValue = Health = Max_Health = 1000;
        BossStates.Add(HealthStats.Full, FullHealth);
        BossStates.Add(HealthStats.High, HighHealth);
        BossStates.Add(HealthStats.Mid, MidHealth);
        BossStates.Add(HealthStats.Low, LowHeath);

        Sheild = gameObject.transform.GetChild(0).gameObject;

        StartLocation = transform.position;
        intervalTimer = 10;
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();

    }

    public override void IdleState()
    {
        anim.SetBool("Attack", false);
    }

    public override void AttackState()
    {
        BossStates[curHealthState].Invoke();
        intervalTimer -= Time.deltaTime;

        if (intervalTimer < 0 && !AttackMode)
        {
            anim.SetBool("Attack", true);
            AttackMode = true;
            AttackBox.enabled = true;
        }
        if (AttackMode&& AttackBox.enabled)
        {
            //float direction = Vector2.Angle(new Vector2(gameObject.transform.position.x, gameObject.transform.position.y),
            //     new Vector2(Morgan.transform.position.x, Morgan.transform.position.y));
            float direction = gameObject.transform.position.x- Morgan.transform.position.x;
            if (direction <= 0)
                GetComponent<Rigidbody2D>().velocity = new Vector2(MoveSpeed *25 * Time.deltaTime, 0);
            else
                GetComponent<Rigidbody2D>().velocity = new Vector2(-MoveSpeed *25* Time.deltaTime, 0);

        }
        if(AttackMode &&!AttackBox.enabled)
        {
            float direction = gameObject.transform.position.x-StartLocation.x;
            if ( direction <= 0)
                GetComponent<Rigidbody2D>().velocity = new Vector2(MoveSpeed *25* Time.deltaTime, 0);
            else
                GetComponent<Rigidbody2D>().velocity = new Vector2(-MoveSpeed *25* Time.deltaTime, 0);

            if(Vector2.Distance(new Vector2(gameObject.transform.position.x, gameObject.transform.position.y),StartLocation)<.5f)
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
        if (other.tag == "Player")
        {
            other.gameObject.SendMessage("TakeDamage", Damage);
            AttackBox.enabled = false;
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
            curHealthState = HealthStats.High;
        }
        else
        {
            // Hard code always be 3 child for now
            if (Sheild)
                Sheild.SetActive(true);
            curHealthState = HealthStats.Low;
        }

        bossHealthSlider.value = Health;
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
                timer = 10;
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
                Instantiate(EnemySpawn[0]);
                SpawnTimer = UnityEngine.Random.Range(2.5f, 4f);
            }
            if (timer < -12)
                timer = 12;
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
                Instantiate(EnemySpawn[0]);
                SpawnTimer = UnityEngine.Random.Range(2f, 4f);
            }
            if (timer < -14)
                timer = 14;
        }
    }
    public virtual void LowHeath()
    {
        if (timer <= 0)
        {
            if (SpawnTimer <= 0)
            {
                Instantiate(EnemySpawn[0]);
                SpawnTimer = UnityEngine.Random.Range(1.5f, 2.5f);
            }
            if (timer < -15)
                timer = 15;
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
}
