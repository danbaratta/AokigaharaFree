using UnityEngine;
using System.Collections;

public class HealthDrop : MonoBehaviour
{

    public int Health;

    GameObject Player;

    public float m_Distance = 10;
    public float Speed = 5;
    // Use this for initialization
    void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector2.Distance(gameObject.transform.position, Player.transform.position) < m_Distance)
        {
            Vector2 temp = gameObject.transform.position - Player.transform.position;
            temp.Normalize();
            temp *= Time.deltaTime * Speed;
            temp = -temp;
            gameObject.transform.position += new Vector3(temp.x, temp.y, 0);
        }
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        Player.SendMessage("TakeDamage",-Health);
        Destroy(gameObject);
    }
}