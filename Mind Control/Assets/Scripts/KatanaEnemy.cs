using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class KatanaEnemy : MonoBehaviour {

	private enum KatanaStateMachine
	{
		IDLE,
		ATTACK,
		POSSESSED,

		NUM_STATES
	}

	//Animation Variables
	Animator a;
	bool right = true; 
	private float katanaX; 

	//Movement Variables
	float MoveSpeed = 3.0f;
	float speed;
	Vector2 movement = Vector2.zero; 

	//Position Variables
	float curTransformX;
	float curTransformY; 

	//State Machine Variables
	[SerializeField]
	KatanaStateMachine curState;

	bool someTypeOfSwitchThatIDontKnowWhatItDoes = false; 

	Dictionary<KatanaStateMachine, Action> ksm = new Dictionary<KatanaStateMachine, Action>(); 

	// Use this for initialization
	void Start () 
	{
		ksm.Add(KatanaStateMachine.IDLE, StateIdle);
		ksm.Add (KatanaStateMachine.ATTACK, StateAttack);
		ksm.Add (KatanaStateMachine.POSSESSED, StatePossessed);

		curTransformX = transform.position.x;
		curTransformY = transform.position.y;


	}
	
	// Update is called once per frame
	void Update () 
	{
		ksm [curState].Invoke(); 

		if (curState == KatanaStateMachine.IDLE) 
		{
			StateIdle ();
		}

		if (curState == KatanaStateMachine.ATTACK) 
		{
			StateAttack ();
		}
		if (curState == KatanaStateMachine.POSSESSED) 
		{
			StatePossessed ();
		}

		//Needed to allow AI to move in player control state. Still gives an error. Doesn't effect play.
		a.SetFloat("Speed", Mathf.Abs (katanaX));
		GetComponent<Rigidbody2D> ().velocity = new Vector2 (katanaX * MoveSpeed, GetComponent<Rigidbody2D> ().velocity.y);
	}

	void FixedUpdate()
	{
		if (katanaX > 0 && !right) 
			Flip (); 

		else
		{
			if (katanaX < 0 && right)
				Flip ();	
		}
	}

	//Animation functions
	void Flip()
	{
		right = !right;
		Vector2 scale = transform.localScale;
		scale.x *= -1;
		transform.localScale = scale; 
	}

	//Enemy States
	void SetState(KatanaStateMachine nextState)
	{
		curState = nextState;
	}

	void StateIdle()
	{
		
	}

	void StateAttack()
	{
		
	}

	void StatePossessed()
	{
		
	}
}
