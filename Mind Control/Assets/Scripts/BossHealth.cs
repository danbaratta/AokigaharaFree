using UnityEngine;
using System.Collections;
using UnityEngine.UI; 

public class BossHealth : MonoBehaviour {

	MorganStateMachine msm;

	public int startingHealth = 1000;                            
	public int currentHealth;                                   
	public Slider bossHealthSlider;   
	public GameObject BossCanvas;  
	public GameObject rearBossWall; 

	bool isDead;                                                 
	bool damaged;        

	// Use this for initialization
	void Start () 
	{
		msm = GameObject.Find("Morgan").GetComponent<MorganStateMachine> ();

		rearBossWall.SetActive (true);
		BossCanvas.SetActive (false); 
		currentHealth = startingHealth; 
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(damaged)
		{
			//change sprite color to red for a moment 
		}

		damaged = false;
	}

	public void BossTakeDamage (int amount) 
	{
		damaged = true;
		currentHealth -= amount;
		bossHealthSlider.value = currentHealth;
		if(currentHealth <= 0 && !isDead)
		{
			Death ();
		}
	}

	void Death ()
	{

		isDead = true;
		//Play particle system?
		//remove invisible walls to allow player to complete level
		rearBossWall.SetActive(false); 
		BossCanvas.SetActive (false);
		Destroy(gameObject);
	} 

	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.gameObject.tag == "Player") 
		{
			BossCanvas.SetActive (true);
		}
	}

}
