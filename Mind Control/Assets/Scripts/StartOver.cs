using UnityEngine;
using System.Collections;

public class StartOver : MonoBehaviour {

	public GameObject winScreen; 

	// Use this for initialization
	void Start () 
	{
		winScreen.SetActive (false); 
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void LoadScene(int lvlnum)
	{
		Application.LoadLevel (lvlnum); 
	}


	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.gameObject.tag == "Player")  
		{
			winScreen.SetActive (true);
		}
	}
}
