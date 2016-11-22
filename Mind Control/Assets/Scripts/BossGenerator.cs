using UnityEngine;
using System.Collections;

public class BossGenerator : MonoBehaviour {

	[SerializeField]
	GameObject[] EnemyTypes;
	bool generateEnemies;

	[SerializeField]
	float TimeMin = 3.0f;
	[SerializeField]
	float TimeMax = 6.0f;

	//Time between enemies spawning
	private float TimeLimit = 0;
	private float Timer = 0;

	//loop variables
	public int enemyCount; 

	// Use this for initialization
	void Start () 
	{
		InvokeRepeating ("SpawnEnemy", TimeMax, TimeMin);
	}

	// Update is called once per frame
	void Update () 
	{
		UpdateTimer ();
	}

	void UpdateTimer()
	{
		Timer += Time.deltaTime;

		if (Timer >= TimeLimit) 
		{
			TimeLimit = Random.Range (TimeMin, TimeMax);
			SpawnEnemy ();
			Timer = 0;
		}
	}

	void SpawnEnemy()
	{
		//if(GetComponent<BossHealth>() <= 999)
		{
			{
				if (generateEnemies) 
				{
					int randIndex = Random.Range (0, EnemyTypes.Length);
					Instantiate (EnemyTypes [randIndex], transform.position, transform.rotation);

				}
			} 
		}
	}


	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.tag == "Player") 
		{
			generateEnemies = true;
		}
	}

	void OnTriggerExit2D(Collider2D other)
	{
		if (other.tag == "Player") 
		{
			generateEnemies = false;
		}	
	}
}
