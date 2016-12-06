using UnityEngine;
using System.Collections;

public class PitFallScript : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag =="Player")
        {
            other.SendMessage("SpawnPlayer");
        }

        if(other.tag == "Enemy")
        {
            other.SendMessage("PlayerReset");
        }
    }
}
