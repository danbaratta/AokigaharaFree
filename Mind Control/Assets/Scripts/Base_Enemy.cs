using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
public class Base_Enemy : MonoBehaviour
{
    public enum EnemyState
    {
        Idle,
        Walk,
        Attack,
        Death,

        MaxStates,
    }
    // Use this for initialization
   public int Max_Health;
    public int Health;

    public float Max_Defense;
    public float Defense;

    public int Max_Damage;
    public int Damage;

    public Animator anim;

    //Attacking player variables
   public GameObject Morgan;
   public PlayerHealth playerHealth;

    // Movement variables
    public float MoveSpeed = 2.0f;
    public float Speed;

   public EnemyState curState;
    public bool Mirror = false;

    Dictionary<EnemyState, Action> States = new Dictionary<EnemyState, Action>();

    // Script References
    public MorganStateMachine msm;

    public virtual void Start()
    {
        States.Add(EnemyState.Idle, IdleState);
        States.Add(EnemyState.Walk, WalkState);
        States.Add(EnemyState.Attack, AttackState);
        States.Add(EnemyState.Death, DeathState);

        Morgan = GameObject.FindGameObjectWithTag("Player");
        playerHealth = Morgan.GetComponent<PlayerHealth>();
        anim = GetComponent<Animator>();
        msm = GameObject.Find("Morgan").GetComponent<MorganStateMachine>();

    }

    // Update is called once per frame
    public virtual void Update()
    {
        States[curState].Invoke();

        if(anim.GetBool("Dead"))
        {
            Destroy(this);
        }
    }

    void SetState(EnemyState nextState)
    {
        curState = nextState;
    }

    public virtual void IdleState()
    {

    }

    public virtual void AttackState()
    {

    }
    public virtual void WalkState()
    {

    }
    public virtual void DeathState()
    {
        if (anim.GetBool("Dead"))
        {
            Destroy(this);
        }
    }

    public virtual void TakeDamage(int dmg)
    {
        Health -= dmg;
        if (Health <= 0)
        {
            curState = EnemyState.Death;
            foreach (var item in GetComponents<BoxCollider>())
            {
                item.enabled = false;
            } 
        }
    }
}
