using UnityEngine;
using System.Collections;

public class Movement : MonoBehaviour {
	

	private float playerX;

	[SerializeField]
	private float moveSpeed;

	bool right = true;

	Animator a;

	bool ground = false;
	public Transform groundCheck;
	float groundRadious = 0.1f; 
	public float jumpForce = 700f; 
	public LayerMask walkable;

	void Start()
	{
		a = GetComponent<Animator> ();
	}

	void Update()
	{
		if (ground && Input.GetKeyDown (KeyCode.W)) 
		{
			a.SetBool ("Ground", false);
			GetComponent<Rigidbody2D>().AddForce(new Vector2(0, jumpForce));
		}
	}

	void FixedUpdate()
	{
		ground = Physics2D.OverlapCircle (groundCheck.position, groundRadious, walkable);
		a.SetBool ("Ground", ground);
		a.SetFloat ("VerticalSpeed", GetComponent<Rigidbody2D> ().velocity.y);

		playerX = Input.GetAxis ("Horizontal");

		a.SetFloat("speed", Mathf.Abs (playerX));
		GetComponent<Rigidbody2D> ().velocity = new Vector2 (playerX * moveSpeed, GetComponent<Rigidbody2D> ().velocity.y);

		if (playerX > 0 && !right) 
			Flip (); 
		
		else
		{
			if (playerX < 0 && right)
				Flip ();	
		}
	}

	void Flip()
	{
		right = !right;
		Vector3 scale = transform.localScale;
		scale.x *= -1;
		transform.localScale = scale; 
	}
}
