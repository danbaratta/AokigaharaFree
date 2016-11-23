using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Actions : MonoBehaviour {


    public enum Action 
    {
        Attack,
        Idle,
        
    }

    Animator anim;
    public static Action actualAction = Action.Idle;
    int actualAnim = (int)Action.Idle;
    public Text actText;
	// Use this for initialization
	void Start () {
       
        anim = this.gameObject.GetComponent<Animator>();
        //actText.text = actualAction.ToString();
        PlayAnimation();
	}

	public void Reset()
	{
		actualAnim = 1;
		NextAnimation ();
	}

    public void NextAnimation()
    { 
        //switch(((int)actualAnim + 1))
		switch(((int)actualAnim))
        {
            case 0:
                actualAction = Action.Attack;
                actualAnim = (int)actualAction;
                break;
            case 1:
                actualAction = Action.Idle;
                actualAnim = (int)actualAction;
                break;
           
            default:
                actualAction = Action.Attack;
                actualAnim = (int)actualAction;
                break;
        }

        actText.text = actualAction.ToString();
		anim.Play(actualAction.ToString());
    }


	public void Next()
	{
		actualAnim++;
		NextAnimation ();
	}

	public void Back()
	{
		actualAnim--;
		NextAnimation ();
	}
	
    public void PlayAnimation()
    {
        anim.Play(actualAction.ToString());
    }

    public IEnumerator BackToIdle() 
    {
        yield return new WaitForSeconds(.7f);
        anim.Play(Action.Idle.ToString());
    }
}
