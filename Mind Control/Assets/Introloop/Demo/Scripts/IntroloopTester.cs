/* 
/// Copyright (c) 2015 Sirawat Pitaksarit, Exceed7 Experiments LP 
/// http://www.exceed7.com/introloop
*/

using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class IntroloopTester : MonoBehaviour {

    public IntroloopAudio[] allIntroloopAudio;
    public IntroloopTestSwitches ts;

    public void PlayIndex(int arrayMember)
    {
        if(ts.useFade)
        {
            IntroloopPlayer.Instance.PlayFade(allIntroloopAudio[arrayMember],ts.fadeTime);
        }
        else
        {
            IntroloopPlayer.Instance.Play(allIntroloopAudio[arrayMember]);
        }
        UpdateSongInformation(arrayMember);
    }

    public Text songNameText;
    public Text genreText;
    private void UpdateSongInformation(int arrayMember)
    {
        float length = allIntroloopAudio[arrayMember].ClipLength;
        string songName = "", genre = "";
        switch (arrayMember) 
        {
            case 0:
            {
                songName = "5argon - Assault";
                genre = "Psy Techno";
                break;
            }
            case 1:
            {
                songName = "5argon - Assault (End)";
                genre = "Psy Techno";
                break;
            }
            case 2:
            {
                songName = "5argon - Compete";
                genre = "Latin Beats";
                break;
            }
            case 3:
            {
                songName = "5argon - Compete (End)";
                genre = "Latin Beats";
                break;
            }
            case 4:
            {
                songName = "5argon - Otter's Celebration (Prepare)";
                genre = "Funk";
                break;
            }
            case 5:
            {
                songName = "5argon - Otter's Celebration";
                genre = "Funk";
                break;
            }
            case 6:
            {
                songName = "5argon - Maid Battle (RPG Arrange)";
                genre = "Jazz Fusion";
                break;
            }
        }
        TimeSpan lengthTime = TimeSpan.FromSeconds(length);
        songName += " [" +  string.Format("{0:D1}:{1:D2}",lengthTime.Minutes,lengthTime.Seconds) + "]";
        songNameText.text = songName;
        genreText.text = genre;
    }

    private void Play(int arrayMember)
    {
        IntroloopPlayer.Instance.Play(allIntroloopAudio[arrayMember]);
    }

    private void PlayFade(int arrayMember)
    {
        IntroloopPlayer.Instance.PlayFade(allIntroloopAudio[arrayMember]);
    }

    public void Pause()
    {
        if(ts.useFade)
        {
            IntroloopPlayer.Instance.PauseFade(ts.fadeTime);
        }
        else
        {
            IntroloopPlayer.Instance.Pause();
        }
    }

    public void Resume()
    {
        if(ts.useFade)
        {
            IntroloopPlayer.Instance.ResumeFade(ts.fadeTime);
        }
        else
        {
            IntroloopPlayer.Instance.Resume();
        }
    }

    public void Stop()
    {
        if(ts.useFade)
        {
            IntroloopPlayer.Instance.StopFade(ts.fadeTime);
        }
        else
        {
            IntroloopPlayer.Instance.Stop();
        }
        songNameText.text = "";
        genreText.text = "";
    }

    public void GetTime()
    {
        Debug.Log(IntroloopPlayer.Instance.GetTime());
    }

    void Start()
    {
        songNameText.text = "";
        genreText.text = "";
    }

    public Button[] playButtons;
    public Button pauseButton;
    public Button resumeButton;
    public Button stopButton;
    public Toggle useFadeToggle;
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.A))
        {
            playButtons[0].onClick.Invoke();
        }
        if(Input.GetKeyDown(KeyCode.S))
        {
            playButtons[1].onClick.Invoke();
        }
        if(Input.GetKeyDown(KeyCode.D))
        {
            playButtons[2].onClick.Invoke();
        }
        if(Input.GetKeyDown(KeyCode.F))
        {
            playButtons[3].onClick.Invoke();
        }
        if(Input.GetKeyDown(KeyCode.G))
        {
            playButtons[4].onClick.Invoke();
        }
        if(Input.GetKeyDown(KeyCode.H))
        {
            playButtons[5].onClick.Invoke();
        }
        if(Input.GetKeyDown(KeyCode.J))
        {
            playButtons[6].onClick.Invoke();
        }

        if(Input.GetKeyDown(KeyCode.Z))
        {
            pauseButton.onClick.Invoke();
        }
        if(Input.GetKeyDown(KeyCode.X))
        {
            resumeButton.onClick.Invoke();
        }
        if(Input.GetKeyDown(KeyCode.C))
        {
            stopButton.onClick.Invoke();
        }

        if(Input.GetKeyDown(KeyCode.Q))
        {
			useFadeToggle.isOn = !useFadeToggle.isOn;
			useFadeToggle.onValueChanged.Invoke(useFadeToggle.isOn);
        }
    }

}
