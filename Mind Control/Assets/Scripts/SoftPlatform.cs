using UnityEngine;
using System.Collections;

public class SoftPlatform : MonoBehaviour {

	void OnTriggerEnter2D(Collider2D col)
	{
		if (col.tag == "Player") 
		{
			Physics2D.IgnoreCollision (col, transform.parent.GetComponent<Collider2D>(), true);
		}
	}

	void OnTriggerExit2D(Collider2D col)
	{
		if (col.tag == "Player") 
		{
			Physics2D.IgnoreCollision (col, transform.parent.GetComponent<Collider2D>(), false);
		}
	}
}
