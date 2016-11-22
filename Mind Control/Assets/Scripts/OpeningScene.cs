using UnityEngine;
using System.Collections;

public class OpeningScene : MonoBehaviour {

	public GameObject Background; 
	public GameObject Text;
	public GameObject Text1;
	public GameObject Text2; 
	public GameObject Text3; 
	public GameObject NextButton;

	public GameObject PlayButton; 
	public GameObject Text4; 
	public GameObject Controls; 


	// Use this for initialization
	void Start () 
	{
		FirstPage (); 
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}

	public void FirstPage()
	{
		Background.SetActive (true);
		Text.SetActive (true);
		Text1.SetActive (true);
		Text2.SetActive (true);
		Text3.SetActive (true);
		NextButton.SetActive (true);

		PlayButton.SetActive (false);
		Text4.SetActive (false);
		Controls.SetActive (false); 
	}

	public void SecondPage()
	{
		Background.SetActive (true);
		Text.SetActive (false);
		Text1.SetActive (false);
		Text2.SetActive (false);
		Text3.SetActive (false);
		NextButton.SetActive (false);

		PlayButton.SetActive (true);
		Text4.SetActive (true);
		Controls.SetActive (true);
	}

	public void LoadScene(int lvlnum)
	{
		Application.LoadLevel (lvlnum); 
	}
}
