using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Triple_Floor_Blast : MonoBehaviour
{

    SpriteRenderer DisplaySprite;
    GameObject[] Spikes;
    float[] OrginalY;
    public float LocalPositionY;

    public float DelayBetweenSpikes = .4f;
    float m_SpikeDelay;
    public float SpikeTimeSpeed = 1;
    float m_SpikeTimeSpeed;


    public float DistanceAwayFromMorgan=10;
    public int Damage =25;

    GameObject m_SpikeObj;

    bool m_On;
    // Use this for initialization

    MorganStateMachine Morgan;
    int m_Cur;

    void Awake()
    {
        DisplaySprite = gameObject.GetComponent<SpriteRenderer>();
        Spikes = new GameObject[gameObject.transform.childCount];
        OrginalY = new float[Spikes.Length];
        for (int i = 0; i < Spikes.Length; i++)
        {
            Spikes[i] = gameObject.transform.GetChild(i).gameObject;
            OrginalY[i] = Spikes[i].transform.position.y;
        }
        Morgan = GameObject.Find("Morgan").GetComponent<MorganStateMachine>();

        //// Parent To morgan
        //gameObject.transform.parent = GameObject.Find("Morgan").transform;
        //gameObject.transform.localPosition = new Vector3(DistanceAwayFromMorgan, gameObject.transform.localPosition.y, gameObject.transform.localPosition.z);

    }
    void Start()
    {
        Reset();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 NewPos = Morgan.gameObject.transform.position;
        if (Morgan.GetReflect())
            NewPos.x += DistanceAwayFromMorgan;
        else
            NewPos.x -= DistanceAwayFromMorgan;
        NewPos.y = gameObject.transform.position.y;
        NewPos.z = gameObject.transform.position.z;
        gameObject.transform.position = NewPos;
        if ((Input.GetKeyDown(KeyCode.Mouse1) || Input.GetButtonDown("joystick button 0")) && !m_On)
        {
            m_On = true;
            DisplaySprite.enabled = false;
            m_SpikeTimeSpeed = 0;
            m_SpikeObj = Spikes[0];
            m_SpikeObj.SetActive(true);
            m_SpikeDelay = 0;
        }
        else if (m_On)
        {
            m_SpikeDelay -= Time.deltaTime;
            if (m_SpikeDelay <= 0)
            {
                // Dont want world want local to parent
                Vector3 temp = m_SpikeObj.transform.localPosition;

                //Check Spike speed how long should take to be at  100% up
                //Base on how much time has pass should speed up or decress speed;

                //Dis Form
                float Distance = temp.y - LocalPositionY;
                Distance *= Distance;
                Distance = Mathf.Sqrt(Distance);
                m_SpikeTimeSpeed += Time.deltaTime;

                float Speed = 0, Dis = 0;

                if (Distance != 0)
                {//temp.y += (Distance / m_SpikeTimeSpeed) * Time.deltaTime;

                    Speed = (Distance / SpikeTimeSpeed);
                    Dis = Speed * m_SpikeTimeSpeed;
                }

                temp.y += Dis;

                m_SpikeObj.transform.localPosition = temp;

                if (m_SpikeTimeSpeed >= SpikeTimeSpeed)
                {
                    temp.y = LocalPositionY;
                    m_SpikeObj.transform.localPosition = temp;
                    m_Cur++;
                    if (m_Cur <= Spikes.Length - 1)
                    {
                        m_SpikeObj = Spikes[m_Cur];
                        m_SpikeObj.SetActive(true);
                    }
                    else
                    {
                        //Safe Check
                        Reset();
                        gameObject.SetActive(false);
                    }
                    m_SpikeDelay = DelayBetweenSpikes;
                    m_SpikeTimeSpeed = 0;
                }
            }
        }
    }

    void Reset()
    {
        DisplaySprite.enabled = true;
        for (int i = 0; i < Spikes.Length; i++)
        {
            Spikes[i].SetActive(false);
            Vector3 temp = Spikes[i].transform.position;
            temp.y = OrginalY[i];
            Spikes[i].transform.position = temp;
        }
        m_Cur = 0;
        m_On = false;
    }

    //void OnTriggerEnter2D(Collider2D other)
    //{
    //    if (other.gameObject.tag == "Enemy")
    //    {
    //    }
    //}


    public void SendDamage(GameObject obj)
    {
        obj.SendMessage("TakeDamage", Damage);
    }
}
