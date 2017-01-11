using UnityEngine;
using System.Collections;

public class Abilities : MonoBehaviour
{
    // All the abilities player will use
    public enum m_Abilities
    {
        SimpleBullet,
        MindBullet,
        Dash,
        //Telekinesis,
        Teleport,
        Sheild,
        Triple_Floor_Blast,
        MaxSize,
    }
    // Energy
    public int m_TeleportEnergy = 25;
    public int m_DashEnergy = 5;
    public int m_PossesEnergy = 5;
    public int m_ShieldEnergy = 25;
    public int m_FloorBlastEnergy = 35;
    public int m_MegaBlastEnergy = 65;

    public bool[] m_UnlockedSkills;

    /// <summary>
    /// All Game object that are in pool (ex: Enemies and bullets) are here
    /// </summary>
    PoolManager m_PoolManager;

    //Skills
    public GameObject m_MindBullet;

    //// Once used for Telekinesis
    //GameObject m_MovingObject;

    public bool m_Telekinesis;
    Teleport m_Teleport;

    PlayerSheild m_Sheild;
    public int SheildHealth = 125;


    Triple_Floor_Blast m_FloorBlast;
    // Morgan
    MorganStateMachine Morgan;


    public float m_TeleportTimer = 5;

    m_Abilities m_LastUsed = m_Abilities.MaxSize;

    void Awake()
    {
        
        // If can we cant find object we create object in the level ( Should be the level)
        if (GameObject.Find("PoolManager") != null)
        {
            m_PoolManager = GameObject.Find("PoolManager").GetComponent<PoolManager>();
        }
        else
        {
            m_PoolManager = ((GameObject)Instantiate(Resources.Load("Abilities/PoolManager"))).GetComponent<PoolManager>();
            Debug.Log("Forgot create PoolManager prefab in level");
            m_PoolManager.gameObject.name = "PoolManager";

        }
        // If can we cant find object we create object in the level ( Should be the level)
        if (GameObject.Find("Teleport") != null)
            m_Teleport = GameObject.Find("Teleport").GetComponent<Teleport>();
        else
        {
            m_Teleport=((GameObject)Instantiate(Resources.Load("Abilities/Teleport"))).GetComponent<Teleport>();
            Debug.Log("Forgot create Teleport prefab in level");
            m_Teleport.gameObject.name = "Teleport";

        }
        // If this is missing someone really Messed up.
        if (GameObject.Find("Morgan") != null)
            Morgan = GameObject.Find("Morgan").GetComponent<MorganStateMachine>();
        else
        {
            Debug.Log("Forgot create Morgan in level");

        }
        // If can we cant find object we create object in the level ( Should be the level)
        if (GameObject.Find("PlayerSheild") != null)
        {
            m_Sheild = GameObject.Find("PlayerSheild").GetComponent<PlayerSheild>();
            m_Sheild.gameObject.SetActive(false);
        }
        else
        {
            Debug.Log("Forgot create PlayerSheild prefab in level");
        }
        // If can we cant find object we create object in the level ( Should be the level)
        if (GameObject.Find("TripleBlast") != null)
            m_FloorBlast = GameObject.Find("TripleBlast").GetComponent<Triple_Floor_Blast>();
        else
        {
            m_FloorBlast = ((GameObject)Instantiate(Resources.Load("Abilities/TripleBlast"))).GetComponent<Triple_Floor_Blast>();
            Debug.Log("Forgot TripleBlast Teleport prefab in level");
            m_FloorBlast.gameObject.name = "TripleBlast";
        }

    }
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    /// <summary>
    /// Send in abilities you like to use and return true if you can 
    /// use that abilties return false if you cant
    /// </summary>
    /// <param name="Type">
    /// Look up if you can use ability
    /// </param>
    /// <returns></returns>
    public bool CanUseAbilities(m_Abilities Type)
    {
        switch (Type)
        {
            case m_Abilities.SimpleBullet:
                {
                    if (m_UnlockedSkills[0])
                        return true;
                    else
                        return false;
                }

            case m_Abilities.MindBullet:
                {
                    if (m_UnlockedSkills[1])
                        return true;
                    else
                        return false;
                }

            case m_Abilities.Dash:
                {
                    if (m_UnlockedSkills[2])
                        return true;
                    else
                        return false;
                }

            case m_Abilities.Teleport:
                {
                    if (m_UnlockedSkills[3])
                        return true;
                    else
                        return false;
                }

            case m_Abilities.Sheild:
                {
                    if (m_UnlockedSkills[4])
                        return true;
                    else
                        return false;
                }

            case m_Abilities.Triple_Floor_Blast:
                {
                    if (m_UnlockedSkills[5])
                        return true;
                    else
                        return false;
                }

            case m_Abilities.MaxSize:
                return false;

            default:
                return false;

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
    /// <summary>
    /// Turn on Dash do i really need explain this? xD
    /// Set gravity scale to 0 meaning gravity is off
    /// Set velocity and base on direction player will move right or left
    /// </summary>

    /// <param name="speed">
    /// How fast player should move
    /// </param>
    /// <param name="Direction">
    ///  Which way the player will move
    /// </param>
    /// <returns>
    /// If we can dash
    /// </returns>
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

    /// <summary>
    /// Turn off player dash and set gravity to its default gravity
    /// </summary>
    public void DashOff(GameObject Player)
    {
        Player.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
        Player.GetComponent<Rigidbody2D>().gravityScale = Player.GetComponent<MorganStateMachine>().GetDefaultGravity();
    }


    //public void TelekinesisOn()
    //{
    //    Vector3 temp = Input.mousePosition;
    //    temp = Camera.main.ScreenToWorldPoint(temp);
    //
    //    temp.z = -10;
    //
    //    int Bits = 0;
    //    for (int i = 0; i < 16; i++)
    //    {
    //        Bits |= 1 << i;
    //    }
    //
    //    Bits = ~Bits;
    //    RaycastHit2D hit;
    //
    //    hit = Physics2D.Raycast(temp, Camera.main.transform.forward, 25f, Bits);
    //    if (hit.collider)
    //    {
    //        if (hit.collider.tag == "Blocks")
    //        {
    //            m_MovingObject = hit.collider.gameObject;
    //            m_MovingObject.SendMessage("MoveBlock");
    //            m_Telekinesis = true;
    //        }
    //    }
    //}
    //public void TelekinesisOff()
    //{
    //    m_MovingObject.SendMessage("StopMoveBlock");
    //    m_MovingObject = null;
    //    m_Telekinesis = false;
    //}

        /// <summary>
        /// Teleport....
        /// Teleport on script tell script to turn on and how long to run for.
        /// </summary>
        /// <returns></returns>
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
        m_LastUsed = num;
    }
    /// <summary>
    /// Use to get last ability player selected
    /// </summary>
    /// <returns></returns>
    public m_Abilities GetAbility()
    {
        return m_LastUsed;
    }
    /// <summary>
    /// Sheild....
    /// Set sheild script how much health should have and how long to stay on and turn on sheild.
    /// </summary>
    /// <returns></returns>
    public bool SheildUp()
    {
        if (m_Sheild)
        {
            if (!m_Sheild.gameObject.activeSelf && Morgan.CanUseAbilbity(m_ShieldEnergy))
            {
                m_Sheild.SetHealth(SheildHealth);
                m_Sheild.SetTimer();
                m_Sheild.gameObject.SetActive(true);
                return true;
            }
        }
        return false;
    }

    /// <summary>
    /// Turn on  triple floor blast object and script and run script
    /// </summary>
    /// <returns></returns>
    public bool TripleFloorBlast()
    {
        if(m_FloorBlast)
        {
            if (!m_FloorBlast.gameObject.activeSelf && Morgan.CanUseAbilbity(m_FloorBlastEnergy))
            {
                m_FloorBlast.gameObject.SetActive(true);
                m_FloorBlast.Reset();
                return true;
            }
        }
        return false;
    }

}
