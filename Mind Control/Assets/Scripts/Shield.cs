using UnityEngine;
using System.Collections;

public class Shield : MonoBehaviour
{

    // Use this for initialization
    public int Max_Health;
    public int Health;
    public int Def;

    bool m_Damaged;
    float m_Timer;
    public float m_ConstTimer;
    void Start()
    {
        Max_Health = Health;
        
    }

    // Update is called once per frame
    void Update()
    {
        if (m_Damaged)
        {
            m_Timer += Time.deltaTime;
            if (m_Timer <= m_ConstTimer)
            {
                Color temp = new Color(1, 0, 0);
                temp.g = (((float)Health / (float)Max_Health) * m_Timer / m_ConstTimer);
                temp.b = (((float)Health / (float)Max_Health) * m_Timer / m_ConstTimer);
                GetComponent<Renderer>().material.color = temp;
            }
            else
            {
                m_Timer = 0;
                m_Damaged = false;
            }
        }
    }

    void TakeDamage(int dmg)
    {
        if (Def < dmg)
            Health -= dmg;
        if (Health <= 0)
            Destroy(this.gameObject);
        Color temp = GetComponent<Renderer>().material.color;
        temp.g = (float)Health / (float)Max_Health;
        temp.b = (float)Health / (float)Max_Health;
        GetComponent<Renderer>().material.color = temp;
        m_Damaged = true;
    }
}
