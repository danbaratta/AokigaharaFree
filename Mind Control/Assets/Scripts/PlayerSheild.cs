using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSheild : MonoBehaviour
{
   public int m_Health=125;
    public float m_Timer=10;
    float timer;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0)
            gameObject.SetActive(false);
    }


   public void TakeDamage(int dmg)
    {
        m_Health -= dmg;
        if (m_Health <= 0)
            gameObject.SetActive(false);
    }

    public void SetHealth(int hp)
    {
        m_Health = hp;
    }

    public void SetTimer()
    {
        timer = m_Timer;
    }
}
