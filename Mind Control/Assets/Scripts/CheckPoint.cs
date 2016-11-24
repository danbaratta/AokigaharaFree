using UnityEngine;
using System.Collections;

public class CheckPoint : MonoBehaviour
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
        if (other.tag == "Player")
        {
            other.gameObject.SendMessage("CheckPointUpdate", this.gameObject.transform.position);
            this.gameObject.SetActive(false);
        }
    }

}
