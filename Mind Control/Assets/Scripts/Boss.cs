using UnityEngine;
using System.Collections;

public class Boss : MonoBehaviour {

	// Script References
	MorganStateMachine msm;

	//attacking boss variables
	GameObject boss;  
	BossHealth bossHealth;  
	public int bossAttackDamage = 20;
	public int bulletBossDamage = 5; 
    
	// Use this for initialization
	void Start () {
	
	}

	void Awake()
	{
        //Morgan = GameObject.FindGameObjectWithTag ("Player"); 
        //playerHealth = Morgan.GetComponent<PlayerHealth> (); 
        boss = gameObject;// GameObject.FindGameObjectWithTag ("Boss");  
		bossHealth = gameObject.GetComponent<BossHealth> (); 
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}

	void OnTriggerEnter2D(Collider2D other)  
	{
		if (other.tag == "Player")      
		{
			//if (msm.isPossessing == true)  
			//{
			//	if (bossHealth.currentHealth > 0)  
			//	{
			//	bossHealth.BossTakeDamage (bossAttackDamage);        
            //
			//	Debug.Log ("Boss health works");
			//	msm.Possess (false); // Update this once we get a health and damage system.
			//	Destroy (gameObject);
			//	}
		    //
			//}
		}
		if (other.tag == "Bullet") 
		{
			if (bossHealth.currentHealth > 0) 
			{
				bossHealth.BossTakeDamage (bulletBossDamage);
			}
		}
	}
}


