using UnityEngine;
using System.Collections;

public class Projectile : MonoBehaviour {

	float moveSpeed = 9.0f;

	public GameObject bullet; 
	bool DirectionRight = true; 


	// Use this for initialization
	void Start () 
	{
		Destroy (gameObject, 3f); 
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
		if (other.tag == "Enemy") 											
		{
			Destroy (gameObject);
			Destroy (other.gameObject); 
		}
	}

	void OnBecameInvisible ()
	{
		Destroy (gameObject);
	}
}
