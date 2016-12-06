using UnityEngine;
using System.Collections;

public class SpikeFloor : MonoBehaviour
{
    public int m_Damage=5;
    public float m_DelayDamageTimer=.3f;
    float m_Timer;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        m_Timer -= Time.deltaTime;
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (m_Timer <= 0)
        {
            other.SendMessage("TakeDamage", m_Damage);
            m_Timer = m_DelayDamageTimer;
        }
    }
}
