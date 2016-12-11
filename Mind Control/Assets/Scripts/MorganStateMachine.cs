using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine.UI;

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

    public int m_MaxPowerBar = 100;
    public int m_CurrentPower = 0;

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
        m_Abilities = GameObject.Find("Abilities").GetComponent<Abilities>();
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
            m_Abilities.Teleport();
            TeleportOn();
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
    }

    void TransitionFromOni()
    {
        isPossessing = false;
        SetState(PlayerStateMachine.IDLE);
        a.SetBool("Walk", false);
    }

    void HandleYureiControls()
    {
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

            if (Input.GetKey(KeyCode.T) && Morgan.Dash)
            {
                m_Abilities.DashOn(gameObject, Morgan.dashSpeed, Reflect);
                SetState(PlayerStateMachine.DASH);
                Morgan.Dash = false;
            }
        }
    }

    void CheckForJump()
    {
        if ((Input.GetKeyDown(KeyCode.Space) || Input.GetButtonDown("joystick button 0")) && canJump)
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

    // POSSESSION FUNCTIONS

    //void FireMindBullet()                                   // Need to fix firing to the LEFT
    //{
    //    GameObject TempBullet = (GameObject)Instantiate(MindBullet, Morgan.transform.position, Morgan.transform.rotation);
    //    if (!Reflect)
    //        TempBullet.SendMessage("FlipAxisLeft");
    //    else
    //        TempBullet.SendMessage("FlipAxisRight");
    //}

    //void FireBullet()
    //{
    //    GameObject TempBullet= m_PoolManager.FindClass(PoolManager.EnemiesType.PlayerBullets);
    //    TempBullet.transform.position = Morgan.transform.position;
    //    TempBullet.transform.rotation = Quaternion.identity;
    //    if (!Reflect)
    //        TempBullet.SendMessage("FlipAxisLeft");
    //    else
    //        TempBullet.SendMessage("FlipAxisRight");
    //}
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
            m_Abilities.FireMindBullet(gameObject.transform.position, Reflect);
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
    }

    public void TeleportOff()
    {
        GetComponent<BoxCollider2D>().enabled = true;
        GetComponent<SpriteRenderer>().enabled = true;
        GetComponent<Rigidbody2D>().gravityScale = 1;
    }
}