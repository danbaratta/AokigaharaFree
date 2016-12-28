using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadialBtn : MonoBehaviour
{

    public Abilities.m_Abilities Type;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void PickAbility()
    {
        GameObject.Find("Abilities").SendMessage("SetAbility", Type);
    }
}
