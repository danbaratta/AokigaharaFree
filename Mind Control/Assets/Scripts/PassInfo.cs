using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassInfo : MonoBehaviour
{

    public bool IsParent;
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
        if (other.gameObject.tag == "Enemy")
        {
            if (IsParent)
                gameObject.transform.parent.gameObject.SendMessage("SendDamage", other.gameObject);
            else
                gameObject.transform.parent.gameObject.SendMessage("SendDamage", other.gameObject);
        }
    }
}
