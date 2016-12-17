using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
public class Base_Enemy : MonoBehaviour
{
    public PoolManager.EnemiesType Type;

    public enum EnemyState
    {
        Idle,
        Walk,
        Attack,
        Death,
        Jump,

        MaxStates,
    }
    // Use this for initialization
    public int Max_Health;
    public int Health;
    //
    public float Max_Defense;
    public float Defense;
    //
    public int Max_Damage;
    public int Damage;
    // Animator ref
    public Animator anim;

    //Attacking player variables
    public GameObject Morgan;
    public PlayerHealth playerHealth;

    // Movement variables
    public float MoveSpeed = 2.0f;
    public float Speed;

    public bool NotSpawned = false;

    //state
    public EnemyState curState;
    //Use for checking if enemie should look other direction
    public bool Mirror = false;

    Dictionary<EnemyState, Action> States = new Dictionary<EnemyState, Action>();


    // Drop
    public GameObject m_ItemToDrop;
    // 0-1 scale % ex:.75%
    public float m_ChanceOfDrop;

    // Script References
    public MorganStateMachine msm;
    //pool manger ref
    PoolManager m_PoolManager;

    public virtual void Awake()
    {
        States.Add(EnemyState.Idle, IdleState);
        States.Add(EnemyState.Walk, WalkState);
        States.Add(EnemyState.Attack, AttackState);
        States.Add(EnemyState.Death, DeathState);
        States.Add(EnemyState.Jump, Jump);
        States.Add(EnemyState.MaxStates, Stub);
        anim = GetComponent<Animator>();
        Health = Max_Health;
    }

    public virtual void Start()
    {
        Morgan = GameObject.FindGameObjectWithTag("Player");
        playerHealth = Morgan.GetComponent<PlayerHealth>();

        msm = GameObject.Find("Morgan").GetComponent<MorganStateMachine>();
        m_PoolManager = GameObject.Find("PoolManager").GetComponent<PoolManager>();

    }
    void Stub()
    {

    }

    // Update is called once per frame
    public virtual void Update()
    {
        States[curState].Invoke();

    }

    public virtual void SetState(EnemyState nextState)
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

    public virtual void Jump()
    {

    }

    public virtual void DeathState()
    {
        if (anim.GetBool("RealDeath"))
        {
            m_PoolManager.Remove(gameObject, Type);
        }
    }

    public virtual void TakeDamage(int dmg)
    {
        Health -= dmg;
        if (Health <= 0)
        {
            SetState(EnemyState.Death);
            TurnOffCollision();
            anim.SetBool("Dead", true);
            anim.Play("Death");
            GetComponent<Rigidbody2D>().velocity = new Vector2();
        }
    }

    /// <summary>
    /// Flip the image True = facing right False = facing left
    /// </summary>
    public void Flip(bool Turn)
    {
        Vector3 scale = transform.localScale;
        if (Turn)
            scale.x = Math.Abs(scale.x);
        else
            scale.x = -Math.Abs(scale.x);
        transform.localScale = scale;
        Mirror = !Turn;
    }

    public void TurnOffCollision()
    {
        foreach (var item in GetComponents<BoxCollider2D>())
        {
            item.enabled = false;
        }
    }

    public void TurnOnCollision()
    {
        foreach (var item in GetComponents<BoxCollider2D>())
        {
            item.enabled = true;
        }
    }

    /// <summary>
    /// Move toward the direction you give it
    /// One time call
    /// Most have Rigidbody
    /// </summary>
    /// <param name="Direction"></param>
    virtual public void Move(Vector3 Direction)
    {
        if (GetComponent<Rigidbody>())
            GetComponent<Rigidbody>().velocity = Direction;
    }
    /// <summary>
    /// Move Toward the object you want to go to
    /// </summary>
    virtual public void MoveTowards(Vector3 Target)
    {
        if (GetComponent<Rigidbody>())
        {
            Vector3 temp = gameObject.transform.position - Target;
            temp = temp.normalized;
            GetComponent<Rigidbody>().velocity = temp;
        }
    }

    public void DropItem()
    {
        if (UnityEngine.Random.Range(0f, 1f) > m_ChanceOfDrop)
        {
            if (m_ItemToDrop)
                Instantiate(m_ItemToDrop, this.gameObject.transform.position, Quaternion.identity);
        }
    }


    public void PlayerReset()
    {
        m_PoolManager.Remove(gameObject, Type);
    }

    public PoolManager GetPoolManager()
    {
        return m_PoolManager;
    }
    virtual public void Reset()
    {
        Health = Max_Health;
        anim.SetBool("Dead", false);
        anim.SetBool("RealDeath", false);
        anim.Play("Idle");
        TurnOnCollision();
        SetState(EnemyState.Idle);
    }


    public bool RemoveThis()
    {
        if (!NotSpawned)
        {
            m_PoolManager.Remove(gameObject, Type);
            return true;
        }
	Destroy(gameObject);
        return false;
    }

    /// <summary>
    /// Safe Check make sure if this object was spawn normal method well set var to correct sets
    /// </summary>
    public void Spanwed()
    {
        NotSpawned = false;
    }
}
