﻿using UnityEngine;
using System.Collections;

public class YureiLevl2ReDone : Base_Enemy
{
    //Distance away from player to destory AI
    public float m_Distance = 25f;

    // Use this for initialization
    public override void Start()
    {
        base.Start();
        Health = Max_Health;
        Type = PoolManager.EnemiesType.Yurei_Level2;
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

    }
    public override void WalkState()
    {
        //transform.position -= transform.right * Time.deltaTime * MoveSpeed;
        Vector3 temp = gameObject.transform.position - Morgan.transform.position;
        temp = temp.normalized;
        GetComponent<Rigidbody2D>().velocity = -temp*MoveSpeed;
    }
    public override void DeathState()
    {
        base.DeathState();
        if (DeathSound)
        {
            sound.Stop();
            sound.clip = DeathSound;
            sound.Play();
        }
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
            if (HitSound)
            {
                sound.Stop();
                sound.clip = HitSound;
                sound.Play();
            }

        }
        else if ((other.tag == "Player") && (msm.isPossessing == true))
        {
            msm.TransitionFrom();
            SetState(EnemyState.Death);
            anim.SetBool("Dead", true);
            anim.Play("Death");
            GetComponent<Rigidbody2D>().velocity = new Vector2();
            if (HitSound)
            {
                sound.Stop();
                sound.clip = HitSound;
                sound.Play();
            }
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
                if (HitSound)
                {
                    sound.Stop();
                    sound.clip = HitSound;
                    sound.Play();
                }
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
}
