using UnityEngine;
using System.Collections;

public class Projectile : MonoBehaviour
{
	[SerializeField]
    float moveSpeed = 9.0f;

    public GameObject bullet;
    bool DirectionRight = true;

    public int Damage = 5;

    // Use this for initialization
    void Start()
    {
        Destroy(gameObject, 3f);
    }

    // Update is called once per frame
    void Update()
    {
        MoveProjectile();
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
            Destroy(gameObject);
        }
    }

    public void FlipAxis()
    {
        DirectionRight = false;
        Vector3 scale = transform.localScale;

        scale.x = -scale.x;
        transform.localScale = scale;
    }

    // bullets may not want to be destory if it not in camera view
    //void OnBecameInvisible ()
    //{
    //	Destroy (gameObject);
    //}
}
