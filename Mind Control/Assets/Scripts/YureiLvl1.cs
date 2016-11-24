using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class YureiLvl1 : MonoBehaviour
{

    private enum YureiStateMachine
    {
        AI_CONTROLLED,
        DYING,

        NUM_STATE
    }

    Dictionary<YureiStateMachine, Action> ysm = new Dictionary<YureiStateMachine, Action>();

    // Script References
    MorganStateMachine msm;

    //animation variables
    Animator a;
    bool right = true;
    private float yureiX;

    // Movement variables
    [SerializeField]
    float MoveSpeed = 2.0f;
    float Speed;

    // State Machine variables
    [SerializeField]
    YureiStateMachine curState;

    // Transform Position variables
    float curTransformX;
    float curTransformY;

    //Attacking player variables
    GameObject Morgan;
    PlayerHealth playerHealth;
    public int attackDamage = 10;

    //attacking boss variables
    GameObject Boss;
    BossHealth bossHealth;
    public int bossAttackDamage = 20;

    void Awake()
    {
        Morgan = GameObject.FindGameObjectWithTag("Player");
        playerHealth = Morgan.GetComponent<PlayerHealth>();
        //Boss = GameObject.FindGameObjectWithTag ("Boss"); 
        //bossHealth = Boss.GetComponent<BossHealth> ();
    }
    // Use this for initialization
    void Start()
    {
        msm = GameObject.Find("Morgan").GetComponent<MorganStateMachine>();         // get MorganStateMachine script

        ysm.Add(YureiStateMachine.AI_CONTROLLED, StateAI_Controlled);
        ysm.Add(YureiStateMachine.DYING, StateDying);
 
        a = GetComponent<Animator>();
        SetState(YureiStateMachine.AI_CONTROLLED);
        a.Play("Idle");
    }

    void Update()
    {
        ysm[curState].Invoke();
        if (curState != YureiStateMachine.DYING)
        {
            GetComponent<Rigidbody2D>().velocity = new Vector2(yureiX * MoveSpeed, GetComponent<Rigidbody2D>().velocity.y);
        }
    }
    // Update is called once per frame

    void FixedUpdate()
    {
        if (yureiX > 0 && !right)
            Flip();

        else
        {
            if (yureiX < 0 && right)
                Flip();
        }
    }

    void Flip()
    {
        right = !right;
        Vector2 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    // STATE FUNCTIONS

    void SetState(YureiStateMachine nextState)
    {
        GrabPosition();
        curState = nextState;
        switch (nextState)
        {
            case YureiStateMachine.AI_CONTROLLED:
                a.Play("Idle");
                break;
            case YureiStateMachine.DYING:
                a.Play("Death");
                break;
            default:
                break;
        }
    }

    void StateAI_Controlled()
    {
        YureiAIMovement();
    }

    void StateDying()
    {
        a.SetBool("Dead", true);
        if (a.GetBool("RealDeath"))
        {
            Debug.Log("Ack, I'm dyin'!");
            Destroy(gameObject);
        }
    }

    // HELPER FUNCTIONS

    void YureiAIMovement()
    {
        transform.position -= transform.right * Time.deltaTime * MoveSpeed;
    }

    void GrabPosition()
    {
        curTransformX = GetComponent<Transform>().position.x;
        curTransformY = GetComponent<Transform>().position.y;
    }

    void SendPosition()
    {
        msm.GetTargetX(curTransformX);
        msm.GetTargetY(curTransformY);
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
            GrabPosition();
            SendPosition();
            Destroy(gameObject);
        }
        else if (other.gameObject.tag == "Bullet")
        {
            SetState(YureiStateMachine.DYING);
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
                playerHealth.TakeDamage(attackDamage);       
                SetState(YureiStateMachine.DYING);
            }
        }
    }
    void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}
