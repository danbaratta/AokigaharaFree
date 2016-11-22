using UnityEngine;
using System.Collections;

public class BossWall : MonoBehaviour {

	public GameObject bossWall; 

	// Use this for initialization
	void Start () 
	{
		bossWall.SetActive (false); 
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.gameObject.tag == "Player") 
		{
			bossWall.SetActive (true);
		}
	}
}
