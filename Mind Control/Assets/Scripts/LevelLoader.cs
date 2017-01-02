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
            DontDestroyOnLoad(this);

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

    public void LoadLevel(string LevelName = "", int LevelIndex = -1)
    {
        if (LevelName.Length != 0)
            SceneManager.LoadScene(LevelName);
        else
            SceneManager.LoadScene(LevelName);
    }
}
