using UnityEngine;

public class Pause : MonoBehaviour
{
    public bool Paused;
	
	// Update is called once per frame
	void Update ()
    {
        if (Input.GetKeyDown(KeyCode.P))
            Paused = !Paused;
            
        Time.timeScale = Paused ? 0 : 1;
    }
}
