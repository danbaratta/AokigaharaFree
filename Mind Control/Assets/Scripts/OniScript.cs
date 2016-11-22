using UnityEngine;
using System.Collections;

public class OniScript : MonoBehaviour 
{

	MorganStateMachine msm;

	// Model Info variables
	float oniJump = 6f;

	// Jump Timer variables
	[SerializeField]
	float jumpTimer = 0f;
	float jumpTimerMax = 5f;

	// Attack variables
	[SerializeField]
	float attackTimer = 0f;
	float attackTimerMax = 3f;
	[SerializeField]
	bool willAttack = false;
	Transform attackCheckPoint;
	LayerMask Player;

	float curTransformX;
	float curTransformY;

	// Use this for initialization
	void Start () 
	{
		msm = GameObject.Find ("Morgan").GetComponent<MorganStateMachine> ();
		attackCheckPoint = GameObject.Find ("AttackCheck").GetComponent<Transform>();
	}

	// Update is called once per frame
	void Update () 
	{
		UpdateTimers ();
		CheckForJump ();
		CheckForAttack ();
	}

	void CheckForAttack()
	{
		willAttack = Physics2D.OverlapCircle (attackCheckPoint.position, 1.0f, Player);

		if (willAttack == true) 
		{
			gameObject.GetComponent<SpriteRenderer> ().color = Color.red; 

			Debug.Log ("willAttack = " + willAttack);
		} else if (willAttack == false)
		{
			gameObject.GetComponent<SpriteRenderer> ().color = Color.white;
		}

		if (attackTimer >= attackTimerMax) 
		{
			Debug.Log ("bool willAttack = " + willAttack);
			attackTimer = 0f;
		}
	}

	void CheckForJump()
	{
		if (jumpTimer >= jumpTimerMax)
		{
			GetComponent<Rigidbody2D> ().velocity += new Vector2 (0, oniJump);
			jumpTimer = 0f;
		}
	}

	void UpdateTimers()
	{
		jumpTimer += Time.deltaTime;
		attackTimer += Time.deltaTime;
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


	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.tag == "MindBullet") 
		{
			GrabPosition ();
			SendPosition ();
			msm.Possess (true);
			msm.IsOni (true);
			Destroy (gameObject);
		}
	}


}
