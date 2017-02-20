using UnityEngine;
using System.Collections;

public class Enemy_Projectile : MonoBehaviour
{

    float moveSpeed = 9.0f;

    public AudioClip FireSound;
    public AudioClip HitSound;
    AudioSource sound = new AudioSource();
    public GameObject bullet;
    bool DirectionRight = true;

    public int Damage = 5;
    //pool manger ref
    PoolManager m_PoolManager;
    PoolManager.EnemiesType Type = PoolManager.EnemiesType.EnemyBullets;
    float LifeTimer = 3;
    // Use this for initialization
    void Start()
    {
        bullet = gameObject;
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
        if (other.tag == "Player")
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
       else if (other.tag == "Bullet")
        {
            m_PoolManager.Remove(gameObject, Type);
            m_PoolManager.Remove(other.gameObject, PoolManager.EnemiesType.PlayerBullets);
            if (HitSound)
            {
                sound.Stop();
                sound.clip = HitSound;
                sound.Play();
            }
        }
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
