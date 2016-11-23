using UnityEngine;
using System.Collections;

public class Shield : MonoBehaviour
{

    // Use this for initialization
    public int Max_Health;
    public int Health;
    public int Def;

    void Start()
    {
        Max_Health = Health;
    }

    // Update is called once per frame
    void Update()
    {

    }

    void TakeDamage(int dmg)
    {
        if (Def < dmg)
            Health -= dmg;
        if (Health <= 0)
            Destroy(this.gameObject);
        Color temp = GetComponent<Renderer>().material.color;
        temp.g = (float)Health / (float)Max_Health;
        temp.b = (float)Health / (float)Max_Health;
        GetComponent<Renderer>().material.color = temp;
    }
}
