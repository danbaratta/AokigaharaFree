using UnityEngine;
using System.Collections;

public class Morgan : MonoBehaviour {

	private float walkSpeed = 5f;
	private float walkAccel = 1f;

	public float dashSpeed = 20f;
	public float dashTimer = 0f;
    public float ConstDashTimer;
    public bool Dash;

	private float jumpAccel = 0.5f;
	private float initJumpSpeed = 8.5f;
	private float jumpTimer = 0f;
	private float maxJumpTimer = 0.1f;

	private SpriteRenderer morganSprite;


	public float WalkSpeed()
	{
		return walkSpeed;
	}

	public float WalkAccel()
	{
		return walkAccel;
	}

	public float DashSpeed()
	{
		return dashSpeed;
	}

	public float DashTimer()
	{
		return dashTimer;
	}

	public float JumpAccel()
	{
		return jumpAccel;
	}

	public float InitJumpSpeed()
	{
		return initJumpSpeed;
	}

	public float JumpTimer()
	{
		return jumpTimer;
	}

	public float MaxJumpTimer()
	{
		return maxJumpTimer;
	}

	public SpriteRenderer MorganSprite()
	{
		return morganSprite;
	}
}
