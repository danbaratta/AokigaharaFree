using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class ButtonHandler : MonoBehaviour {

	//Main start screen
	public GameObject MainMenu;
	public GameObject StartButton;
	public GameObject OptionsButton;
	public GameObject CreditsButton;
	public GameObject QuitButton; 

	//Options screen
	public GameObject OptionsScreen;
	public GameObject OptionsBackground;
	public GameObject BackButtonOpt;
	public GameObject ControlsScreen;
	public GameObject AudioScreen; 
	public GameObject ControlsButton; 

	//Credits screen
	public GameObject CreditsBackground; 
	public GameObject CreditsScreen;
	public GameObject BackButtonCred; 


	void Start()
	{
		//OptionsMenu ();
		StartMenu ();
		/*MainMenu.SetActive (true);
		StartButton.SetActive (true);
		OptionsButton.SetActive (true);
		CredtisButton.SetActive (true);
		QuitButton.SetActive (true); 
		OptionsScreen.SetActive (false);
		CreditsScreen.SetActive (false);*/


	}

	public void StartMenu()
	{
		MainMenu.SetActive (true);
		StartButton.SetActive (true);
		OptionsButton.SetActive (true);
		CreditsButton.SetActive (true);
		QuitButton.SetActive (true); 

		OptionsScreen.SetActive (false);
		OptionsBackground.SetActive (false);
		BackButtonOpt.SetActive (false); 
		ControlsScreen.SetActive (false);
		ControlsButton.SetActive (false); 
		AudioScreen.SetActive (false); 

		CreditsScreen.SetActive (false);
		CreditsBackground.SetActive (false);	
		BackButtonCred.SetActive (false);
	}

	public void LoadScene(int lvlnum)
	{
		//Application.LoadLevel (lvlnum);
        SceneManager.LoadScene("Level1");


    }

	public void ExitProgram()
	{
		Application.Quit ();
	}

	public void OptionsMenu()
	{
		MainMenu.SetActive (true);
		StartButton.SetActive (false);
		OptionsButton.SetActive (false);
		CreditsButton.SetActive (false);
		QuitButton.SetActive (false); 

		OptionsScreen.SetActive (true);
		OptionsBackground.SetActive (true);
		BackButtonOpt.SetActive (true);
		ControlsScreen.SetActive (false);
		ControlsButton.SetActive (true); 
		AudioScreen.SetActive (false);

		CreditsScreen.SetActive (false);
		CreditsBackground.SetActive (false);
		BackButtonCred.SetActive (false);
	}

	public void CtrlScreen()
	{
		MainMenu.SetActive (true);
		StartButton.SetActive (false);
		OptionsButton.SetActive (false);
		CreditsButton.SetActive (false);
		QuitButton.SetActive (false); 

		OptionsScreen.SetActive (true);
		OptionsBackground.SetActive (true);
		BackButtonOpt.SetActive (true);
		ControlsScreen.SetActive (true);
		ControlsButton.SetActive (false); 
		AudioScreen.SetActive (false); 

		CreditsScreen.SetActive (false);
		CreditsBackground.SetActive (false);
		BackButtonCred.SetActive (false);
	}

	public void AudioScrn()
	{
		MainMenu.SetActive(true);
		StartButton.SetActive (false);
		OptionsButton.SetActive (false);
		CreditsButton.SetActive (false);
		QuitButton.SetActive (false);

		OptionsScreen.SetActive (true);
		OptionsBackground.SetActive (true);
		BackButtonOpt.SetActive (true);
		ControlsScreen.SetActive (false);
		ControlsButton.SetActive (false); 
		AudioScreen.SetActive (true);

		CreditsScreen.SetActive (false);
		CreditsBackground.SetActive (false);
		BackButtonCred.SetActive (false);
	}

	public void CreditScreen()
	{
		MainMenu.SetActive (true);
		StartButton.SetActive (false);
		OptionsButton.SetActive (false);
		CreditsButton.SetActive (false);
		QuitButton.SetActive (false);

		OptionsScreen.SetActive (false);
		OptionsBackground.SetActive (false);
		BackButtonOpt.SetActive (false);
		ControlsScreen.SetActive (false);
		ControlsButton.SetActive (false); 
		AudioScreen.SetActive (false);

		CreditsScreen.SetActive (true);
		CreditsBackground.SetActive (true);
		BackButtonCred.SetActive (true);
	}
}
