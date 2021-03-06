﻿using UnityEngine;
using System.Collections;

public class Projectile : MonoBehaviour
{
    [SerializeField]
    float moveSpeed = 9.0f;

    public GameObject bullet;
    bool DirectionRight = true;

    public int Damage = 5;

    public AudioClip FireSound;
    public AudioClip HitSound;
    AudioSource sound = new AudioSource();
    //pool manger ref
    PoolManager m_PoolManager;
    PoolManager.EnemiesType Type = PoolManager.EnemiesType.PlayerBullets;


    float LifeTimer = 3;
    // Use this for initialization
    void Start()
    {
        m_PoolManager = GameObject.Find("PoolManager").GetComponent<PoolManager>();
    }

    // Update is called once per frame
    void Update()
    {
        MoveProjectile();
        LifeTimer -= Time.deltaTime;
        if (LifeTimer <= 0)
            m_PoolManager.Remove(gameObject, Type);
    }

    void MoveProjectile()
    {
        if (DirectionRight)
        {
            transform.position += transform.right * Time.deltaTime * moveSpeed;
        }
        else
        {
            transform.position -= transform.right * Time.deltaTime * moveSpeed;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Enemy")
        {
            other.gameObject.SendMessage("TakeDamage", Damage, SendMessageOptions.DontRequireReceiver);
            m_PoolManager.Remove(gameObject, Type);
            if (HitSound)
            {
                sound.Stop();
                sound.clip = HitSound;
                sound.Play();
            }
        }
      //else  if(other.tag == "Bullet")
      //  {
      //      m_PoolManager.Remove(gameObject, Type);
      //  }
    }

    public void FlipAxisLeft()
    {
        DirectionRight = false;

        Vector3 scale = transform.localScale;
        scale.x = -Mathf.Abs(scale.x);
        transform.localScale = scale;
        if (FireSound)
        {
            sound.clip = FireSound;
            sound.Play();
        }
    }

    public void FlipAxisRight()
    {
        DirectionRight = true;
        Vector3 scale = transform.localScale;
        scale.x = Mathf.Abs(scale.x);
        transform.localScale = scale;
        if (FireSound)
        {
            sound.clip = FireSound;
            sound.Play();
        }
    }
    // bullets may not want to be destory if it not in camera view
    //void OnBecameInvisible ()
    //{
    //	Destroy (gameObject);
    //}


    public void Reset()
    {
        LifeTimer = 3;
    }
}
