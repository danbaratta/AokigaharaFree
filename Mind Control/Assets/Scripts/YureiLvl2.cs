using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class YureiLvl2 : MonoBehaviour {

	private enum Yurei2StateMachine
	{
		AI_CONTROLLED,

		NUM_STATE
	}

	enum AIMode 
	{
		STANDARD,
		RAMMING,											// Just have to get ramming and steering to turn off and on
		STEERTOWARDS
	}

	Dictionary<Yurei2StateMachine, Action> ysm = new Dictionary<Yurei2StateMachine, Action>();

	// Reference variables
	Morgan morganF;			// Morgan State Machine



	//animation variables
	Animator a;
	[SerializeField]
	bool right = true;
	private float yureiX;

	// X/Y variables
	float curTransformX;
	float curTransformY;


	[SerializeField]
	float MoveSpeed = 2.0f;
	[SerializeField]
	float RamSpeed = 3.0f;

	[SerializeField]
	Yurei2StateMachine curState;

	private AIMode CurrentAIState; 

	private MorganStateMachine msm; 

	//Attacking player variables
	GameObject Morgan; 
	PlayerHealth playerHealth; 
	public int attackDamage = 10;

	//attacking boss variables
	GameObject Boss; 
	BossHealth bossHealth; 
	public int bossAttackDamage = 20;
	void Awake()
	{
		Morgan = GameObject.FindGameObjectWithTag ("Player"); 
		playerHealth = Morgan.GetComponent<PlayerHealth> (); 
		//Boss = GameObject.FindGameObjectWithTag ("Boss"); 
		//bossHealth = Boss.GetComponent<BossHealth> ();
	}
	// Use this for initialization
	void Start () 
	{
		ysm.Add (Yurei2StateMachine.AI_CONTROLLED, StateAI_Controlled);		

		SetState(Yurei2StateMachine.AI_CONTROLLED);

		morganF = GameObject.Find("Morgan").GetComponent<Morgan> ();
		msm = GameObject.Find ("Morgan").GetComponent<MorganStateMachine> ();


		SetState (Yurei2StateMachine.AI_CONTROLLED);
	}

	void Update ()
	{
		ysm [curState].Invoke();



		//yureiX = Input.GetAxis ("Horizontal");

		if (curState == Yurei2StateMachine.AI_CONTROLLED) 
		{
			StateAI_Controlled ();
		}
			
		DetermineAIState ();

		switch (CurrentAIState)
		{
			case AIMode.STANDARD:
			UpdateStandard();
			break;

		case AIMode.RAMMING:
			UpdateRamming ();
			break;

		case AIMode.STEERTOWARDS:
			UpdateSteerTowards ();
			break;
		default:
			Debug.Log ("Unknown AI state: " + CurrentAIState);
			break;
		}

	}

	// Update is called once per frame
	void FixedUpdate () 
	{
//		a.SetFloat("speed", Mathf.Abs (yureiX));

		GetComponent<Rigidbody2D> ().velocity = new Vector2 (yureiX * MoveSpeed, GetComponent<Rigidbody2D> ().velocity.y);

		if (yureiX > 0 && !right) 
			Flip (); 

		else
		{
			if (yureiX < 0 && right)
				Flip ();	
		}
	}

	void Flip()
	{
		right = !right;
		Vector2 scale = transform.localScale;
		scale.x *= -1;
		transform.localScale = scale; 
	}

	// STATE FUNCTIONS

	void SetState(Yurei2StateMachine nextState)
	{
		curState = nextState;
		Debug.Log ("New State = " + curState);
	}


	void StateAI_Controlled()
	{
		YureiAIMovement ();
	}

	//Targeting States
	void DetermineAIState()
	{
		Vector3 directionToPlayer = morganF.transform.position - transform.position;		// Finds the vector to the player
		Vector3 DirToPlayerNorm = directionToPlayer.normalized;

		float product = Vector3.Dot ((transform.right * -1), DirToPlayerNorm);
		float angle = Mathf.Acos (product);

		angle = angle * Mathf.Rad2Deg;													// Finds the vector to the player

		bool canSee = CanSeeTarget("Morgan");
//		Debug.Log ("Can see Morgan.");
		if (canSee) 
		{
			CurrentAIState = AIMode.RAMMING;
//			Debug.Log ("Ramming");
			GetComponent<Renderer> ().material.color = Color.red;
		} 
		else if (product > 0 && angle < 90) 
		{
			CurrentAIState = AIMode.STEERTOWARDS;
		} 
		else 
		{
			CurrentAIState = AIMode.STANDARD;
		}
	}

	// HELPER FUNCTIONS

	void YureiAIMovement()
	{
		transform.position -= transform.right * Time.deltaTime * MoveSpeed;
	}
		

	bool CanSeeTarget(string _targetTag)
	{
		//contains data about collision
		RaycastHit hitInfo;

		//performs a ray cast
		bool hitAny = Physics.Raycast(transform.position, (transform.right * -1), out hitInfo);
//		Debug.Log ("Raycast Hit");
		if (hitAny)
		{
			if (hitInfo.collider.gameObject.tag == _targetTag)
			{
				return true;
			}
		}
		return false;
	}

	void UpdateStandard()
	{
		YureiAIMovement ();
	}

	void UpdateRamming()
	{
		transform.position += transform.right * Time.deltaTime * RamSpeed;
	}

	void UpdateSteerTowards()
	{
		GrabPosition ();
/*		Transform target = GameObject.Find ("Morgan").GetComponent<Transform> ();
		Transform myTransform = transform;
		Vector2 dist = Vector2.Distance(target.position, myTransform.position);
		Vector2 lookDir = target.position - myTransform.position;

		if (dist < 10f)
		{
			myTransform.rotation = Quaternion.Slerp( myTransform.rotation, Quaternion.LookRotation(lookDir), 2 * Time.deltaTime );

			if(dist > 0.5f)
			{
				myTransform.position += (myTransform.right * -1) * MoveSpeed * Time.deltaTime;
			}
		}
*/



//		Vector2 directionToPlayer = morganF.transform.position - transform.position;
		transform.right = new Vector2(curTransformX, curTransformY) - new Vector2 (morganF.transform.position.x, morganF.transform.position.y);
		// Appears to work. Unsure Debug.Log ("Steering towards player");

//		UpdateStandard ();
	}
		
	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.gameObject.tag == "MindBullet") 
		{
			if (msm.right == false)
			{
				msm.Right(true);
			}

			msm.IsYurei(true);
			GrabPosition ();
			SendPosition ();
			Destroy (gameObject.gameObject);
		}
		if ((other.tag == "Player") && (msm.isPossessing == true)) {
			msm.TransitionFromYurei ();
			Destroy (gameObject.gameObject);
		} 
		else if (other.gameObject.tag == "Player") 
		{
			if (playerHealth.currentHealth > 0) 
			{
                float direction = gameObject.transform.position.x - Morgan.transform.position.x;
                if (direction <= 0)
                    msm.GetThrown(false);
                else
                    msm.GetThrown(true);


                playerHealth.TakeDamage (attackDamage);       //deals damage to player
				Debug.Log ("Plaeyer is taking damage");
				/*a.SetTrigger ("Flinch");                       //plays damage animation
				{
					Debug.Log ("play damage animation");
				} */
				Debug.Log ("Health works");
				msm.Possess (false); // Update this once we get a health and damage system.
				Destroy (gameObject);
			}
		}
        if (other.tag == "Bullet")
            Destroy(this.gameObject);
	}

	void GrabPosition()
	{
		curTransformX = GetComponent<Transform>().position.x;
		curTransformY = GetComponent<Transform> ().position.y;
	}

	void SendPosition()
	{
		msm.GetTargetX (curTransformX);
		msm.GetTargetY (curTransformY);
	}

    void TakeDamage(int dmg)
    {
        Destroy(this.gameObject);
    }
	//void OnBecameInvisible ()
	//{
	//	Destroy (gameObject);
	//}
		
}
