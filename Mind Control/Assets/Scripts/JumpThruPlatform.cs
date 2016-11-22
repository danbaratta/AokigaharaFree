using UnityEngine;
using System.Collections;

public class JumpThruPlatform : MonoBehaviour {

	private float Timer = 3.0f;

	private bool inTrigger = false;



	// - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
	//GameObject Functions
	// Use this for initialization
	void Start () 
	{
		inTrigger = false;
	}

	// Update is called once per frame
	void Update () 
	{
		if (inTrigger == true)
		{
			Timer -= Time.deltaTime;

			if (Timer <= 0f) 
			{
				GameObject crumPlatform = GameObject.Find ("CrumblePlatform");
				crumPlatform.GetComponent<BoxCollider2D> ().enabled = false;
				gameObject.GetComponent<BoxCollider2D> ().enabled = false;
				inTrigger = false;
			}
		}

		if (inTrigger == false) 
		{
			Timer += Time.deltaTime;

			if (Timer >= 3.0f) 
			{
				GameObject crumPlatform = GameObject.Find ("CrumblePlatform");
				Timer = 3.0f;
				crumPlatform.GetComponent<BoxCollider2D> ().enabled = true;
				gameObject.GetComponent<BoxCollider2D> ().enabled = true;
			}
		}
	}

	void OnTriggerEnter2D(Collider2D col){

		if (col.tag == "Player") {
			inTrigger= true;
		}
	}

	void OnTriggerExit2D(Collider2D col){

		if (col.tag == "Player") {
			inTrigger = false;
		}
	}
}
