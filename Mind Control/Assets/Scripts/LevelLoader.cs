using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{


    bool LastForever = false;

    void Awake()
    {
        var test = GameObject.Find("LevelLoader");
        if (LastForever)
            DontDestroyOnLoad(this.gameObject);

        if (test)
            Destroy(this.gameObject);
        else
            this.gameObject.name = "LevelLoader";
    }

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    static public void LoadLevel(string LevelName = "")
    {
        if (LevelName.Length != 0)
            SceneManager.LoadScene(LevelName);
        //
    }
    static public void LoadLevel(int LevelIndex = -1)
    {
        if (LevelIndex != -1)
            SceneManager.LoadScene(LevelIndex);
        //
    }
}
