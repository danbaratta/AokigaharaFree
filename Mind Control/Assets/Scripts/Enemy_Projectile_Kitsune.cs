using UnityEngine;
using System.Collections;

public class Enemy_Projectile_Kitsune : MonoBehaviour
{

    public float moveSpeed = 9.0f;


    bool DirectionRight = true;

    public int Damage = 5;
    //pool manger ref
    PoolManager m_PoolManager;
    PoolManager.EnemiesType Type = PoolManager.EnemiesType.Hanya_Bullet;
    public float LifeTimer = 3;
    float m_LifeTimer;

    public bool Fire = false;
    // Use this for initialization
    void Start()
    {
        m_PoolManager = GameObject.Find("PoolManager").GetComponent<PoolManager>();
        m_LifeTimer = LifeTimer;
    }

    // Update is called once per frame
    void Update()
    {
        if (Fire)
        {
            MoveProjectile();
            m_LifeTimer -= Time.deltaTime;
            if (m_LifeTimer <= 0)
                m_PoolManager.Remove(gameObject, Type);
        }
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
            if (Fire)
                m_PoolManager.Remove(gameObject, Type);
        }
        else if (other.tag == "Bullet")
        {
            m_PoolManager.Remove(other.gameObject, PoolManager.EnemiesType.PlayerBullets);
        }
    }

    public void FlipAxisLeft()
    {
        DirectionRight = false;

        Vector3 scale = transform.localScale;
        scale.x = -Mathf.Abs(scale.x);
        transform.localScale = scale;
    }

    public void FlipAxisRight()
    {
        DirectionRight = true;
        Vector3 scale = transform.localScale;
        scale.x = Mathf.Abs(scale.x);
        transform.localScale = scale;
    }

    // bullets may not want to be destory if it not in camera view
    //void OnBecameInvisible ()
    //{
    //	Destroy (gameObject);
    //}


    public void Reset()
    {
        m_LifeTimer = LifeTimer;
        Fire = false;
    }
    public void FireBullet()
    {
        Fire = true;
    }
}
