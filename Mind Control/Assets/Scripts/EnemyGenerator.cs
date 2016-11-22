using UnityEngine;
using System.Collections;

public class EnemyGenerator : MonoBehaviour
{

    // References
    //MorganStateMachine Morgan;

    //allows different enemy types to spawn
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

    // Distance that player must be in order to spawn Enemy
    public float AllowedSpawnDistance;

    //Player Ref
    GameObject Player;

    // Use this for initialization
    void Start()
    {
        //Morgan = GetComponent<MorganStateMachine> ();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateTimer();
    }

    void UpdateTimer()
    {
        Timer += Time.deltaTime;

        if (Timer >= TimeLimit)
        {
            TimeLimit = Random.Range(TimeMin, TimeMax);
            SpawnEnemy();
            Timer = 0;
        }
    }

    void SpawnEnemy()
    {
        if (generateEnemies)
        {
            if (Vector2.Distance(transform.position, Player.transform.position) > AllowedSpawnDistance)
            {
                // Dev safe Check
                if (EnemyTypes.Length != 0)
                {
                    int randIndex = Random.Range(0, EnemyTypes.Length);
                    Instantiate(EnemyTypes[randIndex], transform.position, transform.rotation);

                    // Dynamic way spawning enemies
                    if (transform.childCount != 0)
                        Instantiate(EnemyTypes[randIndex], transform.GetChild(0).transform.position, Quaternion.identity);
                }
            }
            else
                Debug.Log("Cant Spawn Player To close");
        }
    }


    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            generateEnemies = true;
            Player = other.gameObject;
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
