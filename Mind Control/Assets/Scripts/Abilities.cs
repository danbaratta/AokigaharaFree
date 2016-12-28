﻿using UnityEngine;
using System.Collections;

public class Abilities : MonoBehaviour
{
    public enum m_Abilities
    {
        SimpleBullet,
        MindBullet,
        Dash,
        Telekinesis,
        Teleport,
        MaxSize,
    }
    // Energy
    public int m_TeleportEnergy = 25;
    public int m_DashEnergy = 5;
    public int m_PossesEnergy = 5;
    public int m_ShieldEnergy = 25;
    public int m_FloorBlastEnergy = 35;
    public int m_MegaBlastEnergy = 65;

    public bool[] UnlockedSkills;

    public m_Abilities[] Skills;

    PoolManager m_PoolManager;

    //Skills
    public GameObject m_MindBullet;

    GameObject m_MovingObject;

    public bool m_Telekinesis;
    Teleport m_Teleport;

    // Morgan
    MorganStateMachine Morgan;

    public float m_TeleportTimer = 5;

    m_Abilities m_LastUsed = m_Abilities.MaxSize;

    // Use this for initialization
    void Start()
    {
        m_PoolManager = GameObject.Find("PoolManager").GetComponent<PoolManager>();
        m_Teleport = GameObject.Find("Teleport").GetComponent<Teleport>();
        Morgan = GameObject.Find("Morgan").GetComponent<MorganStateMachine>();
    }

    // Update is called once per frame
    void Update()
    {

    }


    public bool CanUseAbilities(m_Abilities Type)
    {
        switch (Type)
        {
            case m_Abilities.SimpleBullet:
                {
                    if (UnlockedSkills[0])
                        return true;
                    else
                        return false;
                }
                break;
            case m_Abilities.MindBullet:
                {
                    if (UnlockedSkills[1])
                        return true;
                    else
                        return false;
                }
                break;
            case m_Abilities.Dash:
                {
                    if (UnlockedSkills[2])
                        return true;
                    else
                        return false;
                }
                break;
            case m_Abilities.Telekinesis:
                {
                    if (UnlockedSkills[3])
                        return true;
                    else
                        return false;
                }
                break;
            default:
                {
                    return false;
                }
                break;
        }
    }


    /// <param name="Direction">
    /// False mean Left
    /// True mean Right
    /// </param>
    public void FireSimpleBullet(Vector3 location, bool Direction)
    {
        GameObject TempBullet = m_PoolManager.FindClass(PoolManager.EnemiesType.PlayerBullets);
        TempBullet.transform.position = location;
        TempBullet.transform.rotation = Quaternion.identity;
        if (!Direction)
            TempBullet.SendMessage("FlipAxisLeft");
        else
            TempBullet.SendMessage("FlipAxisRight");
    }


    public bool FireMindBullet(Vector3 location, bool Direction)
    {
        if (Morgan.CanUseAbilbity(m_PossesEnergy))
        {
            GameObject TempBullet = (GameObject)Instantiate(m_MindBullet, location, Quaternion.identity);
            if (!Direction)
                TempBullet.SendMessage("FlipAxisLeft");
            else
                TempBullet.SendMessage("FlipAxisRight");
            return true;
        }
        else
            return false;
    }

    public bool DashOn(GameObject Player, float speed, bool Direction)
    {
        if (Morgan.CanUseAbilbity(m_DashEnergy))
        {
            Player.GetComponent<Rigidbody2D>().gravityScale = 0;
            if (Direction)
                Player.GetComponent<Rigidbody2D>().velocity = new Vector2(speed, 0);
            else
                Player.GetComponent<Rigidbody2D>().velocity = new Vector2(-speed, 0);
            return true;
        }
        else
            return false;       
    }

    public void DashOff(GameObject Player)
    {
        Player.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
        Player.GetComponent<Rigidbody2D>().gravityScale = 1;
    }


    public void TelekinesisOn()
    {
        Vector3 temp = Input.mousePosition;
        temp = Camera.main.ScreenToWorldPoint(temp);

        temp.z = -10;

        int Bits = 0;
        for (int i = 0; i < 16; i++)
        {
            Bits |= 1 << i;
        }

        Bits = ~Bits;
        RaycastHit2D hit;

        hit = Physics2D.Raycast(temp, Camera.main.transform.forward, 25f, Bits);
        if (hit.collider)
        {
            if (hit.collider.tag == "Blocks")
            {
                m_MovingObject = hit.collider.gameObject;
                m_MovingObject.SendMessage("MoveBlock");
                m_Telekinesis = true;
            }
        }
    }
    public void TelekinesisOff()
    {
        m_MovingObject.SendMessage("StopMoveBlock");
        m_MovingObject = null;
        m_Telekinesis = false;
    }


    public bool Teleport()
    {
        if (Morgan.CanUseAbilbity(m_TeleportEnergy))
        {
            if (m_Teleport)
            {
                m_Teleport.m_TeleportOn = true;
                m_Teleport.SetTimer(5);
            }
            return true;
        }
        else
            return false;
    }


    public void SetAbility(m_Abilities num)
    {
        m_LastUsed =num;
    }

    public m_Abilities GetAbility()
    {
        return m_LastUsed;
    }
}
