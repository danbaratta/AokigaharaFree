using UnityEngine;
using System.Collections;

public class MindProjectileScript : MonoBehaviour {

	float moveSpeed = 9.0f;
	MorganStateMachine msm;

	public GameObject MindBullet;
	bool DirectionRight = true;

	// Target Transport variables
	float targetX;
	float targetY; 
		
	// Use this for initialization
	void Start () 
	{
		Destroy (gameObject, 1f);
	}
	
	// Update is called once per frame
	void Update () 
	{
		MoveProjectile ();
	}

	void MoveProjectile()
	{
		if (DirectionRight) 
		{
			transform.position += transform.right * Time.deltaTime * moveSpeed;
		}
		else 
		{
			transform.position -= transform.right * Time.deltaTime * moveSpeed;
		}
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.tag == "Enemy") 												// V Testing new code here
		{
			msm = GameObject.Find ("Morgan").GetComponent<MorganStateMachine> ();
			msm.Possess (true);
			Destroy (gameObject);
		}
	}

    public void FlipAxisLeft()
    {
        DirectionRight = false;

        Vector3 scale = transform.localScale;
        scale.x = -Mathf.Abs(scale.x);
        transform.localScale = scale;
    }

    public void FlipAxisRight()
    {
        DirectionRight = true;
        Vector3 scale = transform.localScale;
        scale.x = Mathf.Abs(scale.x);
        transform.localScale = scale;
    }

    void OnBecameInvisible ()
	{
		Destroy (gameObject);
	}
		
}
