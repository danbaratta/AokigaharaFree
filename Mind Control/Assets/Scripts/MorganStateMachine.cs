using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine.UI;
using XInputDotNetPure;

public class MorganStateMachine : MonoBehaviour
{

    private enum PlayerStateMachine
    {
        IDLE,
        WALK,
        ENTER_JUMP,
        IN_AIR,
        DASH,
        STATE_YUREI,
        STATE_ONI,

        NUM_STATE
    }
    //pool manager
    PoolManager m_PoolManager;


    Dictionary<PlayerStateMachine, Action> psm = new Dictionary<PlayerStateMachine, Action>();

    Morgan Morgan;  // Data Model
    Rigidbody2D MorganBody2D;

    public GameObject Yurei1;

    Sprite YureiSprite;
    Sprite MorganSprite;

    // X/Y targeting Variables
    float possessedLocX;
    float possessedLocY;

    // State Machine and Possession variables
    [SerializeField]
    PlayerStateMachine curState;
    bool possess = false;
    bool isYurei = false;
    [SerializeField]
    bool isOni = false;
    public bool isPossessing = false;                               // used to determine, in other scripts, if the player is possessing an enemy or not
    float possessTimer = 0f;
    float possessLimit = 6f;
    // next enemy bool here
    //public GameObject MindBullet;

    // Ground Check Variables    
    public Transform groundCheck;
    public LayerMask walkableLayer;
    // Made visable to debug
    public float groundCheckRadius = .1f;
    [SerializeField]
    bool onGround = false;

    // Control variables
    [SerializeField]
    bool canControl = true;
    float yureiMoveSpeed = 3.0f;
    Vector2 yureiMove = Vector2.zero;
    Vector2 oniMove = Vector2.zero;


    // CheckPoint
    Vector2 m_CheckPoint;

    // Jumping Variables
    [SerializeField]
    bool canJump = true;

    // Animation Variables
    Animator a;
    public bool right = true;
    private float playerXAnim;
    bool ground = false;

    bool Reflect = true;

    // Firing and Timing variables
    [SerializeField]
    float bulletTimer = 0f;
    [SerializeField]
    float bulletTimeMax = 0.5f;
    [SerializeField]
    bool bulletCanFire;
    [SerializeField]
    float mindTimer = 0f;
    [SerializeField]
    float mindTimeMax = 20f;
    [SerializeField]
    bool mindCanFire;
    public GameObject bulletIcon;

    //Skills
    Abilities m_Abilities;
    bool m_Teleport;
    // Power Bar
    public Slider PowerSlider;
    public int m_MaxPowerBar = 100;
    public int m_CurrentPower = 0;

    //RaidalMenu
    public GameObject RadialCanvus;
    bool m_RadialBool;

    void Awake()
    {
        if (GameObject.Find("Abilities") != null)
        {
            m_Abilities = GameObject.Find("Abilities").GetComponent<Abilities>();
        }
        else
        {
            m_Abilities = ((GameObject)Instantiate(Resources.Load("Abilities/Abilities"))).GetComponent<Abilities>();
            m_Abilities.gameObject.name = "Abilities";
            Debug.Log("Did not create Abilties prefab in level but i'll create it this once");
        }

        if (RadialCanvus == null)
        {
            RadialCanvus = ((GameObject)Instantiate(Resources.Load("Abilities/RadialMenu")));
            Debug.Log("Did not create RadialMenu prefab in level but i'll create it this once");
        }
    }

    // Use this for initialization
    void Start()
    {
        gameObject.GetComponent<SpriteRenderer>().sprite = Resources.Load("Kodama_iddle9") as Sprite;                                                   // change the sprite

        canControl = true;
        mindCanFire = true;
        mindTimer = 20f;
        bulletCanFire = true;
        bulletTimer = 0.5f;

        Morgan = GetComponent<Morgan>();                                                    // get Data Model reference
        MorganBody2D = gameObject.GetComponent<Rigidbody2D>();
        MorganSprite = GetComponent<SpriteRenderer>().sprite;
        YureiSprite = GetComponent<SpriteRenderer>().sprite;
        a = GetComponent<Animator>();                                                   // get Animator reference

        psm.Add(PlayerStateMachine.IDLE, StateIdle);        // adding states to the dictionary
        psm.Add(PlayerStateMachine.WALK, StateWalk);
        psm.Add(PlayerStateMachine.ENTER_JUMP, StateEnterJump);
        psm.Add(PlayerStateMachine.IN_AIR, StateInAir);
        psm.Add(PlayerStateMachine.STATE_YUREI, State_Yurei);
        psm.Add(PlayerStateMachine.STATE_ONI, State_Oni);
        psm.Add(PlayerStateMachine.DASH, StateDash);       // adding states to the dictionary

        SetState(PlayerStateMachine.IDLE);                  // setting default state to Idle

        m_PoolManager = GameObject.Find("PoolManager").GetComponent<PoolManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!m_Teleport)
        {
            psm[curState].Invoke();

            CheckForPossession();
        }
        else
        {
            // Save Check Zero out all movement
            GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
        }
        //if(Input.GetKeyDown(KeyCode.O))
        //{
        //    if (!m_Abilities.m_Telekinesis)
        //        m_Abilities.TelekinesisOn();
        //    else
        //        m_Abilities.TelekinesisOff();
        //}

        if (Input.GetKeyDown(KeyCode.O))
        {
            if (m_Abilities.Teleport())
            {
                TeleportOn();
                //Test
                //Time.timeScale = .01f;
            }
        }
        // Temp just for testing
        if (Input.GetButtonDown("joystick button 9") || Input.GetKeyDown(KeyCode.Backspace) && !m_RadialBool)
        {
            m_RadialBool = true;
            RadialMenu();
        }
        if (RadialCanvus && !RadialCanvus.activeSelf)
        {
            m_RadialBool = false;
        }
    }

    void Flip(bool Turn)
    {
        Vector3 scale = transform.localScale;
        if (Turn)
            scale.x = Math.Abs(scale.x);
        else
            scale.x = -Math.Abs(scale.x);
        transform.localScale = scale;
    }
    // STATE MACHINE FUNCTIONS

    void SetState(PlayerStateMachine nextState)
    {
        //		if (nextState != curState) 
        //		{
        curState = nextState;
        //		}
    }

    void StateIdle()
    {
        a.SetBool("Walk", false);
        a.speed = 1;
        HandleMorganControls();
        //a.Play("Idle");
        if (Input.GetAxis("Horizontal") != 0)
        {
            a.Play("Walk");
            SetState(PlayerStateMachine.WALK);
        }
    }

    void StateWalk()
    {
        HandleMorganControls();
        //a.Play("Walk");
        a.speed = Math.Abs(playerXAnim);
        a.SetBool("Walk", true);
        if (!onGround)
        {
            a.Play("Jump");
            SetState(PlayerStateMachine.IN_AIR);
        }

        if (Input.GetAxis("Horizontal") == 0)
        {
            a.Play("Idle");
            SetState(PlayerStateMachine.IDLE);
        }
    }

    void StateEnterJump()
    {
        canJump = false;
        GetComponent<Rigidbody2D>().velocity += new Vector2(GetComponent<Rigidbody2D>().velocity.x, Morgan.InitJumpSpeed());
        SetState(PlayerStateMachine.IN_AIR);
        Debug.Log(GetComponent<Rigidbody2D>().velocity.y);

    }

    void StateInAir()
    {
        HandleMorganControls();
        a.speed = 1;
    }

    void StateDash()
    {
        Morgan.Dash = false;
        Morgan.dashTimer -= Time.deltaTime;
        if (Morgan.dashTimer <= 0)
        {
            m_Abilities.DashOff(gameObject);
            SetState(PlayerStateMachine.IDLE);
            Morgan.dashTimer = Morgan.ConstDashTimer;
        }
    }

    void State_Oni()
    {
        Debug.Log("In State_Oni");
        HandleOniControls();
    }

    void State_Yurei()
    {
        HandleYureiControls();
    }

    // HELPER FUNCTIONS

    void HandleOniControls()
    {
        //		gameObject.GetComponent<SpriteRenderer> ().sprite = Resources.Load<Sprite>("Sprites/Kodama_pass11_edit"); 

        CheckForGround();
        CheckForJump();


        //		oniMove.x = Input.GetAxisRaw ("Horizontal") * Time.deltaTime * oniMoveSpeed;
        //		GetComponent<Rigidbody2D> ().velocity = new Vector2 (oniMove.x * oniMoveSpeed, GetComponent<Rigidbody2D> ().velocity.y);
    }

    void TransitionToOni()
    {
        isPossessing = true;
        SetLocationToTarget();
        SetState(PlayerStateMachine.STATE_ONI);
        Debug.Log("made it this far... setstate.state_oni");
        GetComponent<Rigidbody2D>().velocity = new Vector2();

    }

    void TransitionFromOni()
    {
        isPossessing = false;
        SetState(PlayerStateMachine.IDLE);
        a.SetBool("Walk", false);
        GetComponent<Rigidbody2D>().gravityScale = 1;

    }

    void HandleYureiControls()
    {
        GetComponent<Rigidbody2D>().velocity = new Vector2();

        gameObject.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Sprites/Kodama_pass11_edit");

        yureiMove.x = Input.GetAxisRaw("Horizontal") * Time.deltaTime * yureiMoveSpeed;
        gameObject.transform.Translate(yureiMove);

        yureiMove.y = Input.GetAxisRaw("Vertical") * Time.deltaTime * yureiMoveSpeed;
        gameObject.transform.Translate(yureiMove);


        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TransitionFromYurei();
            SetState(PlayerStateMachine.IN_AIR);
        }
    }

    void TransitionToYurei()
    {
        right = true;
        isPossessing = true;
        SetLocationToTarget();
        //gameObject.GetComponent<Rigidbody2D>().isKinematic = true;
        SetState(PlayerStateMachine.STATE_YUREI);
    }

    public void TransitionFromYurei()
    {
        isPossessing = false;
        gameObject.GetComponent<Rigidbody2D>().isKinematic = false;
        SetState(PlayerStateMachine.IN_AIR);
        possess = false;
        isYurei = false;
        possessTimer = 0f;
        GetComponent<Rigidbody2D>().gravityScale = 1;

    }

    void SetLocationToTarget()
    {
        transform.position = new Vector2(possessedLocX, possessedLocY);
    }

    void HandleMorganControls()
    {
        CheckForGround();
        CheckForJump();
        CheckForFiring();

        ground = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, walkableLayer);
        /*
		float moveMorganX = Input.GetAxis("Horizontal") * Morgan.WalkSpeed() * 100f;

		Vector2 movement = new Vector2 (moveMorganX, 0f);
		MorganBody2D.AddForce(movement);
*/
        if (canControl)
        {
            a.SetBool("Ground", ground);
            a.SetFloat("VerticalSpeed", GetComponent<Rigidbody2D>().velocity.y);

            playerXAnim = Input.GetAxis("Horizontal");

            if (Input.GetAxis("Horizontal") > 0)
            {
                if (Reflect != true)
                {
                    Flip(true);
                    Reflect = true;
                }
            }
            else if (Input.GetAxis("Horizontal") < 0)
            {
                if (Reflect == true)
                {
                    Flip(false);
                    Reflect = false;
                }
            }

            a.SetFloat("Speed", playerXAnim);
            GetComponent<Rigidbody2D>().velocity = new Vector2(playerXAnim * Morgan.WalkSpeed(), GetComponent<Rigidbody2D>().velocity.y);

            if (Input.GetKey(KeyCode.LeftShift) && Morgan.Dash)
            {
                if (m_Abilities.DashOn(gameObject, Morgan.dashSpeed, Reflect))
                {
                    SetState(PlayerStateMachine.DASH);
                }
            }

            // Last Ability Picked
            if (Input.GetButtonDown("joystick button 3") || Input.GetKeyDown(KeyCode.K))
            {
                if (m_Abilities.GetAbility() != Abilities.m_Abilities.MaxSize)
                {
                    switch (m_Abilities.GetAbility())
                    {
                        case Abilities.m_Abilities.Dash:
                            {
                                if (Morgan.Dash)
                                {
                                    if (m_Abilities.DashOn(gameObject, Morgan.dashSpeed, Reflect))
                                    {
                                        SetState(PlayerStateMachine.DASH);
                                    }
                                }
                            }
                            break;
                        case Abilities.m_Abilities.Telekinesis:
                            break;
                        case Abilities.m_Abilities.Teleport:
                            {
                                if (m_Abilities.Teleport())
                                {
                                    TeleportOn();
                                }
                            }
                            break;
                        case Abilities.m_Abilities.Sheild:
                            {
                                m_Abilities.SheildUp();
                            }
                            break;
                        case Abilities.m_Abilities.Triple_Floor_Blast:
                            {
                                m_Abilities.TripleFloorBlast();
                            }
                            break;
                        default:
                            break;
                    }
                }
            }
        }
    }

    void CheckForJump()
    {
        if ((Input.GetKeyDown(KeyCode.Space) || Input.GetButtonDown("joystick button 0")) && canJump && !m_RadialBool)
        {
            SetState(PlayerStateMachine.ENTER_JUMP);
        }
    }

    void CheckForGround()
    {
        onGround = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, walkableLayer);

        if (!onGround)
        {
            canJump = false;
            a.Play("Jump");
            SetState(PlayerStateMachine.IN_AIR);

        }
        else if (onGround)
        {
            HandleLandOnGround();
            Morgan.Dash = true;
        }
    }

    void HandleLandOnGround()
    {
        if (onGround)
        {
            canJump = true;
        }

        float direction = Input.GetAxis("Horizontal");

        if (direction != 0f)
        {
            SetState(PlayerStateMachine.WALK);
            a.Play("Walk");
            a.SetBool("Walk", true);
        }
        else
        {
            a.Play("Idle");
            SetState(PlayerStateMachine.IDLE);
            a.SetBool("Walk", false);
        }
    }

    void CheckForFiring()
    {
        Mathf.Clamp(bulletTimer += Time.deltaTime, 0, bulletTimeMax);
        mindTimer += Time.deltaTime;

        if (bulletTimer >= bulletTimeMax)
        {
            bulletCanFire = true;
        }

        else
        {
            bulletCanFire = false;
        }

        if (mindTimer >= mindTimeMax)
        {
            mindCanFire = true;
            bulletIcon.SetActive(true);
        }

        else
        {
            mindCanFire = false;
            bulletIcon.SetActive(false);
        }

        if ((Input.GetKeyDown(KeyCode.Mouse1) || Input.GetAxis("PrimaryAttack") == 1) && (mindCanFire == true))
        {
            // FireMindBullet();
            if (m_Abilities.FireMindBullet(gameObject.transform.position, Reflect))
                mindTimer = 0f;
        }
        else if ((Input.GetKeyDown(KeyCode.Mouse0) || Input.GetAxis("PrimaryAttack") == -1) && (bulletCanFire == true))
        {
            //FireBullet();
            m_Abilities.FireSimpleBullet(gameObject.transform.position, Reflect);
            bulletTimer = 0f;
        }
    }

    void CheckForPossession()                               // We want to run this every frame. Keep in update()						
    {
        if (possess == true)
        {
            Debug.Log("possess = " + possess + "\n isYurei = " + isYurei);
            possessTimer += Time.deltaTime;                 // The IF statements here will decide which enemy you possessed
            GetComponent<Rigidbody2D>().gravityScale = 0;
            if (isYurei == true)
            {
                TransitionToYurei();
                possess = false;
            }
            else if (isOni == true)
            {
                TransitionToOni();
                Debug.Log("isOni = " + isOni);
                possess = false;
            }
            else
                possess = false;
        }
        if (curState == PlayerStateMachine.STATE_YUREI)
        {
            possessTimer += Time.deltaTime;
            if (possessTimer >= possessLimit)
            {
                TransitionFromYurei();
            }
        }
    }

    public void IsYurei(bool _IsYurei)      // lets Possession function know which state to enter when possessing
    {
        isYurei = _IsYurei;
    }

    public void IsOni(bool _IsOni)
    {
        isOni = _IsOni;
    }

    public void GetTargetX(float _GetTargetX)
    {
        possessedLocX = _GetTargetX;
    }

    public void GetTargetY(float _GetTargetY)
    {
        possessedLocY = _GetTargetY;
    }

    public void Possess(bool _Possess)
    {
        possess = _Possess;
    }

    public void Right(bool _Right)
    {
        right = _Right;
    }

    public bool IsPossessing()
    {
        return isPossessing;
    }

    public void SwitchCanControl()
    {
        canControl = true;
        GamePad.SetVibration(PlayerIndex.One, 0, 0);

    }

    public void GetThrown(bool Direction)
    {
        //		gameObject.GetComponent<Rigidbody2D> ().velocity += new Vector2 (GetComponent<Rigidbody2D> ().velocity.x * -20f, 0f);	
        //		GetComponent<Rigidbody2D>().AddForce(Vector2.left * 10, ForceMode2D.Impulse);
        Vector2 throwDir;
        if (Direction)
            throwDir = new Vector2(-30, 10);
        else
            throwDir = new Vector2(30, 10);

        canControl = false;
        Debug.Log("canControl = " + canControl);
        gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
        gameObject.GetComponent<Rigidbody2D>().AddForce(throwDir, ForceMode2D.Impulse);
        //		gameObject.GetComponent<Rigidbody2D> ().AddForce (throwDir, ForceMode2D.Impulse);
        Invoke("SwitchCanControl", 0.3f);
        GamePad.SetVibration(PlayerIndex.One, 1, 1);


    }

    /// <summary>
    /// Direction normlized and power it factor in later
    /// </summary>
    public void GetThrown(Vector2 Direction, int PowerX = 30, int PowerY = 30)
    {
        Direction.x *= PowerX;
        Direction.y *= PowerY;

        canControl = false;
        Debug.Log("canControl = " + canControl);
        gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
        gameObject.GetComponent<Rigidbody2D>().AddForce(Direction, ForceMode2D.Impulse);
        //		gameObject.GetComponent<Rigidbody2D> ().AddForce (throwDir, ForceMode2D.Impulse);
        Invoke("SwitchCanControl", 0.3f);
        GamePad.SetVibration(PlayerIndex.One, 1, 1);
    }
    /// <summary>
    /// Direction normlized and power already factor in
    /// </summary>
    public void GetThrown(Vector2 Direction)
    {
        canControl = false;
        Debug.Log("canControl = " + canControl);
        gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
        gameObject.GetComponent<Rigidbody2D>().AddForce(Direction, ForceMode2D.Impulse);
        //		gameObject.GetComponent<Rigidbody2D> ().AddForce (throwDir, ForceMode2D.Impulse);
        Invoke("SwitchCanControl", 0.3f);
        GamePad.SetVibration(PlayerIndex.One, 1, 1);
    }


    public void SpawnPlayer()
    {
        //transform.position = SpawnPoint.transform.position;
        //Morgan = GetComponent<Morgan>();
        gameObject.transform.position = m_CheckPoint;

        Base_Enemy[] gos = (Base_Enemy[])GameObject.FindObjectsOfType(typeof(Base_Enemy));
        foreach (Base_Enemy go in gos)
        {

            go.gameObject.BroadcastMessage("PlayerReset", SendMessageOptions.DontRequireReceiver);
        }
    }

    public void CheckPointUpdate(Vector3 location)
    {
        m_CheckPoint = new Vector2(location.x, location.y);
    }

    public void PowerBar(int PowerBar)
    {
        m_CurrentPower += PowerBar;
        if (m_CurrentPower < 0)
            m_CurrentPower = 0;
        if (m_CurrentPower > m_MaxPowerBar)
            m_CurrentPower = m_MaxPowerBar;
        if (PowerSlider)
            PowerSlider.value = (float)m_CurrentPower / (float)m_MaxPowerBar;
    }

    public bool CanUseAbilbity(int Power)
    {
        int temp = m_CurrentPower - Power;
        if (temp < 0)
        {
            return false;
        }
        PowerBar(-Power);
        return true;
    }

    public void TeleportComplete()
    {
        m_Teleport = false;
    }

    public void TeleportOn()
    {
        GetComponent<BoxCollider2D>().enabled = false;
        GetComponent<SpriteRenderer>().enabled = false;
        GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
        GetComponent<Rigidbody2D>().gravityScale = 0;
        m_Teleport = true;
    }

    public void TeleportOff()
    {
        GetComponent<BoxCollider2D>().enabled = true;
        GetComponent<SpriteRenderer>().enabled = true;
        GetComponent<Rigidbody2D>().gravityScale = 1;
        m_Teleport = false;
    }


    void RadialMenu()
    {
        if (RadialCanvus)
        {
            RadialCanvus.SetActive(true);
            Time.timeScale = .001f;
        }
        else
        {
            Debug.Log("Forgot to Add RadialCanvus error ;)");
            m_RadialBool = false;
        }
    }

    public bool GetReflect()
    {
        return Reflect;
    }

    public bool isDashing()
    {
        return Morgan.Dash;
    }
}