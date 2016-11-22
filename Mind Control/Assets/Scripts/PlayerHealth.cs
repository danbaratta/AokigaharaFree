using UnityEngine;
using System.Collections;
using UnityEngine.UI; 

public class PlayerHealth : MonoBehaviour {

	public int startingHealth = 100;                            
	public int currentHealth;                                   
	public Slider healthSlider;                                 

	Animator anim;                                              
	AudioSource playerAudio;                                    

	bool isDead;                                                
	bool damaged;                                               



	void Start ()
	{
		
		anim = GetComponent <Animator> ();
		playerAudio = GetComponent <AudioSource> ();

		currentHealth = startingHealth;
	}


	void Update ()
	{
		if(damaged)
		{
			//play "Flinch" animation
		}

		damaged = false;
	}


	public void TakeDamage (int amount)
	{
		damaged = true;
		currentHealth -= amount;
		healthSlider.value = currentHealth;
		if(currentHealth <= 0 && !isDead)
		{
			Death ();
		}
	}


	void Death ()
	{

		isDead = true;

		//anim.SetTrigger ("Death");
		GetComponent<MorganStateMachine> ().SpawnPlayer (); 

	}   

}
