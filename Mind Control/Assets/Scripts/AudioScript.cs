using UnityEngine;
using System.Collections;

public class AudioScript : MonoBehaviour
{
    public AudioClip BackGroundMusic;
    public AudioClip BattleMusic;
    AudioSource AudioPlayer;
    // play another track
    public bool TrackChange;
    AudioClip CurrentTrack;
    //
    public float TransitionSpeed = 5;

    // Use this for initialization
    void Start()
    {
        AudioPlayer = GetComponent<AudioSource>();
        AudioPlayer.Play();
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.I))
        {
            Track(1);
        }
        if (TrackChange)
        {
            FadeOut();
        }
        else if (AudioPlayer.volume < 1)
        {
            FadeIn();
        }

    }

    public void Track(int Type)
    {
        switch (Type)
        {
            case 0:
                {
                    CurrentTrack = BackGroundMusic;
                }
                break;
            case 1:
                {
                    CurrentTrack = BattleMusic;
                }
                break;
            default:
                break;
        }
        TrackChange = true;
    }


    void Fade()
    {

    }
    void FadeOut()
    {
        if (AudioPlayer.volume > 0)
            AudioPlayer.volume -= .1f * TransitionSpeed* Time.deltaTime;
        else
        {
            TrackChange = false;
            AudioPlayer.clip = CurrentTrack;
            AudioPlayer.Play();
        }

    }
    void FadeIn()
    {
        if (AudioPlayer.volume < 1)
            AudioPlayer.volume += .1f * TransitionSpeed* Time.deltaTime;
    }
}