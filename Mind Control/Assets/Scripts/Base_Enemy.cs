using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
public class Base_Enemy : MonoBehaviour
{
    private enum EnemyState
    {
        Idle,
        Walk,
        Attack,
        Death,

        MaxStates,
    }
    // Use this for initialization
    int Max_Health;
    int Health;

    float Max_Defense;
    float Defense;

    int Max_Damage;
    int Damage;

    // Movement variables
    [SerializeField]
    float MoveSpeed = 2.0f;
    float Speed;

    EnemyState curState;

    Dictionary<EnemyState, Action> States = new Dictionary<EnemyState, Action>();

    void Start()
    {
        States.Add(EnemyState.Idle, IdleState);
        States.Add(EnemyState.Walk, WalkState);
        States.Add(EnemyState.Attack, AttackState);
        States.Add(EnemyState.Death, DeathState);

    }

    // Update is called once per frame
    void Update()
    {
        States[curState].Invoke();
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

    }
}
