using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class RadialBtn : MonoBehaviour
{

    public Text Title;
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

        gameObject.transform.parent.gameObject.SetActive(false);
        Time.timeScale = 1;
    }


    public void EnterText()
    {
        Title.gameObject.transform.parent.gameObject.SetActive(true);
        Title.text = Type.ToString();
    }


    public void ExitText()
    {
        Title.gameObject.transform.parent.gameObject.SetActive(false);
        Title.text = "";
    }
    void OnMouseOver()
    {
        Title.gameObject.transform.parent.gameObject.SetActive(true);
        Title.text = Type.ToString();
    }

    void OnMouseExit()
    {
        Title.gameObject.transform.parent.gameObject.SetActive(false);
        Title.text = "";
    }
}
